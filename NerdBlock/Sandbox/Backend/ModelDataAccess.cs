﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Sandbox.Backend
{
    public class ModelDataAccess<T> where T : new()
    {
        string myTableName;

        PropertyInfo[] myModelProperties;
        PropertyInfo[] myDependencies;

        public ModelDataAccess()
        {
            DataModel modelAttrib = typeof(T).GetCustomAttribute<DataModel>();

            if (modelAttrib == null)
                throw new MissingMemberException("No attribute defined for data model");

            myTableName = modelAttrib.TableName;

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);

            myModelProperties = props.Where(Property => Property.GetCustomAttribute<DataField>() != null).ToArray();
            myDependencies = props.Where(Property => Property.GetCustomAttribute<ForeignKey>() != null && Property.GetCustomAttribute<DataField>() != null).ToArray();
        }
        
        /// <summary>
        /// Handles inserting a model instance into the database
        /// </summary>
        /// <param name="model">The model isntance to insert</param>
        /// <returns>True if the insert was successful, false if otherwise</returns>
        public bool Insert(T model)
        {
            // Iterate over dependancies
            for(int index = 0; index < myDependencies.Length; index ++)
            {
                // Get the value from the property
                object pValue = myDependencies[index].GetValue(model);

                if (pValue == null && myDependencies[index].GetCustomAttribute<Nullable>() == null)
                    throw new InvalidOperationException("Cannot insert with a missing dependancy");

                // IF the database does not contain something that matches, add one
                if (!DataAccess.ExistsWeak(myDependencies[index].PropertyType, pValue))
                    DataAccess.InsertWeak(myDependencies[index].PropertyType, pValue);

                myDependencies[index].SetValue(model, DataAccess.MatchWeak(myDependencies[index].PropertyType, pValue)[0]);
            }

            // We need to build two sides of the query, allocate strings
            string insertLeft, insertRight;
            insertLeft = insertRight = "";

            // We also need to store a list of values to insert as well as a list of query parameters
            List<object> insertParams = new List<object>();
            List<QueryParam> queryParams = new List<QueryParam>();

            // Iterate over each property
            for(int index = 0; index < myModelProperties.Length; index ++)
            {
                // Get the value of the property
                object value = myModelProperties[index].GetValue(model);
                // Get the DataField attribute
                DataField dField = myModelProperties[index].GetCustomAttribute<DataField>();

                // We only include in insert if auto-generaed is false
                if (myModelProperties[index].GetCustomAttribute<AutoGenerated>() == null)
                {
                    // Check for a null value
                    if (value == null)
                    {
                        // If we do not allow null, throw an exception, otherwise ignore
                        if (myModelProperties[index].GetCustomAttribute<Nullable>() == null)
                            throw new InvalidOperationException("Cannot insert with non-nullable field not set");
                    }
                    // Next see if this is a foreign key
                    else if (myModelProperties[index].GetCustomAttribute<ForeignKey>() != null)
                    {
                        // Use the DataAccess to get the primary key value for the instance
                        object key = DataAccess.GetPrimaryKeyWeak(myModelProperties[index].PropertyType, value);

                        // Append the field name to the left string
                        insertLeft += dField.FieldName + ",";
                        // Append the field name as a parameter to the right string
                        insertRight += string.Format("@{0},", dField.FieldName);

                        // Make and insert the query parameter
                        queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                        // Add that key value we got
                        insertParams.Add(key);
                    }
                    // Otherwise it is a regular data field
                    else
                    {
                        // Append the field name to the left string
                        insertLeft += dField.FieldName + ",";
                        // Append the field name as a parameter to the right string
                        insertRight += string.Format("@{0},", dField.FieldName);

                        // Make and insert the query parameter
                        queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                        // Add that key value we got
                        insertParams.Add(value);
                    }
                }

            }

            // Prep the final query
            string query = string.Format("insert into {0} ({1}) VALUES ({2})", myTableName, insertLeft.Trim(','), insertRight.Trim(','));
            // Execute the query
            int results = DataAccess.ExecuteStatement(query, queryParams.ToArray(), insertParams.ToArray());

            // Return true if at least one row was effected
            return results > 0;
        }

        public object GetPrimaryKey(T value) 
        {
            for (int index = 0; index < myModelProperties.Length; index ++)
            {
                if (myModelProperties[index].GetCustomAttribute<PrimaryKey>() != null)
                    return myModelProperties[index].GetValue(value);
            }

            return null;
        }

        public bool Exists(T match, bool matchNull)
        {
            // Iterate over dependancies
            for (int index = 0; index < myDependencies.Length; index++)
            {
                // Get the value from the property
                object pValue = myDependencies[index].GetValue(match);

                // IF the database does not contain something that matches, we know that we cannot have this instance
                if (!DataAccess.ExistsWeak(myDependencies[index].PropertyType, pValue))
                {
                    return false;
                }
            }

            // We need to build two sides of the query, allocate strings
            string searchTerms = "";
            string resultValues = "";

            // We also need to store a list of values to insert as well as a list of query parameters
            List<object> insertParams = new List<object>();
            List<QueryParam> queryParams = new List<QueryParam>();

            // Iterate over each property
            for (int index = 0; index < myModelProperties.Length; index++)
            {
                // Get the value of the property
                object value = myModelProperties[index].GetValue(match);
                // Get the DataField attribute
                DataField dField = myModelProperties[index].GetCustomAttribute<DataField>();

                // Check for a null value
                if (value == null)
                {
                    // If we do not allow null, throw an exception, otherwise ignore
                    if (myModelProperties[index].GetCustomAttribute<Nullable>() == null)
                        throw new InvalidOperationException("Cannot search with non-nullable field not set");
                }
                // Next see if this is a foreign key
                else if (myModelProperties[index].GetCustomAttribute<ForeignKey>() != null)
                {
                    // Use the DataAccess to get the primary key value for the instance
                    object key = DataAccess.GetPrimaryKeyWeak(myModelProperties[index].PropertyType, value);

                    // Append the the field name to the search
                    searchTerms += string.Format("{0}=@{0},", dField.FieldName);

                    // Make and insert the query parameter
                    queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                    // Add that key value we got
                    insertParams.Add(key);
                }
                // Otherwise it is a regular data field
                else if (myModelProperties[index].GetCustomAttribute<PrimaryKey>() == null)
                {
                    // Append the the field name to the search
                    searchTerms += string.Format("{0}=@{0} AND ", dField.FieldName);

                    // Make and insert the query parameter
                    queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                    // Add that key value we got
                    insertParams.Add(value);
                }

                resultValues += dField.FieldName + ",";
            }

            // Prep the final query
            string query = string.Format("select {1} from {0} where {2}", myTableName, resultValues.Trim(','), searchTerms.Remove(searchTerms.Length - 5, 4).Trim());

            // Execute the query
            IQueryResult result = DataAccess.ExecuteQuery(query, queryParams.ToArray(), insertParams.ToArray());

            // Return true if at least one row was effected
            return result.NumRows > 0;
        }

        /// <summary>
        /// Gets a collection of models from the database that match the fields set in the given model
        /// </summary>
        /// <param name="match">The model instance to match</param>
        /// <returns>A collection of models from the database</returns>
        public T[] GetMatches(T match)
        {
            // Iterate over dependancies
            for (int index = 0; index < myDependencies.Length; index++)
            {
                // Get the value from the property
                object pValue = myDependencies[index].GetValue(match);

                // IF the database does not contain something that matches, we know that we cannot have this instance
                if (!DataAccess.ExistsWeak(myDependencies[index].PropertyType, pValue))
                {
                    return new T[0];
                }
            }

            // We need to build two sides of the query, allocate strings
            string searchTerms = "";
            string resultValues = "";

            // We also need to store a list of values to insert as well as a list of query parameters
            List<object> insertParams = new List<object>();
            List<QueryParam> queryParams = new List<QueryParam>();

            // Iterate over each property
            for (int index = 0; index < myModelProperties.Length; index++)
            {
                // Get the value of the property
                object value = myModelProperties[index].GetValue(match);
                // Get the DataField attribute
                DataField dField = myModelProperties[index].GetCustomAttribute<DataField>();

                // Check for a null value
                if (value == null)
                {
                    // just ignore
                }
                // Next see if this is a foreign key
                else if (myModelProperties[index].GetCustomAttribute<ForeignKey>() != null)
                {
                    // Use the DataAccess to get the primary key value for the instance
                    object key = DataAccess.GetPrimaryKeyWeak(myModelProperties[index].PropertyType, value);

                    // Append the the field name to the search
                    searchTerms += string.Format("{0}=@{0},", dField.FieldName);

                    // Make and insert the query parameter
                    queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                    // Add that key value we got
                    insertParams.Add(key);
                }
                // Otherwise it is a regular data field
                else if (myModelProperties[index].GetCustomAttribute<PrimaryKey>() == null)
                {
                    // Append the the field name to the search
                    searchTerms += string.Format("{0}=@{0} AND ", dField.FieldName);

                    // Make and insert the query parameter
                    queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));
                    // Add that key value we got
                    insertParams.Add(value);
                }

                resultValues += dField.FieldName + ",";
            }

            // Prep the final query
            string query = string.Format("select {1} from {0} where {2}", myTableName, resultValues.Trim(','), searchTerms.Remove(searchTerms.Length - 5, 4).Trim());

            // Execute the query
            IQueryResult result = DataAccess.ExecuteQuery(query, queryParams.ToArray(), insertParams.ToArray());

            T[] results = new T[result.NumRows];

            for (int index = 0; index < results.Length; index++)
            {
                results[index] = __Decode(result);
                result.MoveNext();
            }

            // Return true if at least one row was effected
            return results;
        }

        private T __Decode(IQueryResult queryResult)
        {
            T result = new T();

            for(int index = 0; index < myModelProperties.Length; index ++)
            {
                DataField field = myModelProperties[index].GetCustomAttribute<DataField>();
                myModelProperties[index].SetValue(result, Convert.ChangeType(queryResult.Row[field.FieldName], myModelProperties[index].PropertyType));
            }

            return result;
        }
    }
}