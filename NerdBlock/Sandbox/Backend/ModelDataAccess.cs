﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Sandbox.Backend
{
    /// <summary>
    /// Represents a generic class that is used to access data models
    /// </summary>
    /// <typeparam name="T">The type of the model to reflect, must implement the DataAccess attributes and all fields should be nullable</typeparam>
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

        /// <summary>
        /// Gets the value from the database with the given primary key
        /// </summary>
        /// <param name="pk">The values for the primary key fields</param>
        /// <returns>The object fetched from the database</returns>
        public T GetFromPrimaryKey(params object[] pk)
        {
            // We need to build two sides of the query, allocate strings
            string searchTerms = "";
            string resultValues = "";

            // We also need to store a list of values to insert as well as a list of query parameters
            List<object> insertParams = new List<object>();
            List<QueryParam> queryParams = new List<QueryParam>();

            // We make a counter to see how many of the PK's we've used
            int pkIndex = 0;

            // Iterate over each property
            for (int index = 0; index < myModelProperties.Length; index++)
            {
                // Get the DataField attribute
                DataField dField = myModelProperties[index].GetCustomAttribute<DataField>();

                // If this attrib is a PK
                if (myModelProperties[index].GetCustomAttribute<PrimaryKey>() != null)
                {
                    // Add the parameter
                    queryParams.Add(new QueryParam(dField.FieldName, dField.FieldType));

                    // Add the pk value and move the counter
                    insertParams.Add(pk[pkIndex++]);

                    // Append to the search terms
                    searchTerms += string.Format("{0}=@{0} AND ", dField.FieldName);
                }

                // We need to select it
                resultValues += dField.FieldName + ",";
            }

            // Prep the final query
            string query = string.Format("select {1} from {0} where {2}", myTableName, resultValues.Trim(','), searchTerms.Remove(searchTerms.Length - 5, 4).Trim());

            // Execute the query
            IQueryResult result = DataAccess.ExecuteQuery(query, queryParams.ToArray(), insertParams.ToArray());

            // Create an array for the result set
            T[] results = new T[result.NumRows];

            // Iterate over all items
            for (int index = 0; index < results.Length; index++)
            {
                results[index] = __Decode(result);
                result.MoveNext();
            }

            // Return true if at least one row was effected
            return results.Length > 0 ? results[0] : default(T);
        }

        /// <summary>
        /// Gets the primary key of an instance
        /// </summary>
        /// <param name="value">The value to get the PK for</param>
        /// <returns>The instance's primary key (at least the first one)</returns>
        public object GetPrimaryKey(T value) 
        {
            for (int index = 0; index < myModelProperties.Length; index ++)
            {
                if (myModelProperties[index].GetCustomAttribute<PrimaryKey>() != null)
                    return myModelProperties[index].GetValue(value);
            }

            return null;
        }

        /// <summary>
        /// Checks whether the given match item exists in the database. Null fields and primary keys are ignored
        /// </summary>
        /// <param name="match">The value to match</param>
        /// <param name="matchChildren">True if children items should be matched, false if otherwise</param>
        /// <returns>True if a match is found, false if otherwise</returns>
        public bool Exists(T match, bool matchChildren)
        {
            // Iterate over dependancies
            for (int index = 0; index < myDependencies.Length; index++)
            {
                // Get the value from the property
                object pValue = myDependencies[index].GetValue(match);

                // IF the database does not contain something that matches, we know that we cannot have this instance
                if (matchChildren && !DataAccess.ExistsWeak(myDependencies[index].PropertyType, pValue))
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
                if (pValue != null && !DataAccess.ExistsWeak(myDependencies[index].PropertyType, pValue))
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
                    searchTerms += string.Format("{0}=@{0} AND ", dField.FieldName);

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

        /// <summary>
        /// Handles decoding an instance from a query result
        /// </summary>
        /// <param name="queryResult">The query result to decode</param>
        /// <returns>The item decoded from the query result</returns>
        private T __Decode(IQueryResult queryResult)
        {
            T result = new T();

            for(int index = 0; index < myModelProperties.Length; index ++)
            {
                PropertyInfo pi = myModelProperties[index];

                DataField field = pi.GetCustomAttribute<DataField>();

                object value = queryResult.Row[field.FieldName];

                if (value.GetType() == typeof(DBNull))
                    value = null;

                else if (pi.PropertyType.IsGenericType && pi.PropertyType.Name.Remove(pi.PropertyType.Name.IndexOf('`')) == "Nullable")
                {
                    Type paramType = pi.PropertyType.GenericTypeArguments[0];

                    if (paramType.IsValueType && Activator.CreateInstance(paramType).Equals(value))
                        value = null;
                    
                    if (value == null)
                        pi.SetValue(result, value);
                    else
                    {
                        if (pi.GetCustomAttribute<ForeignKey>() != null)
                            pi.SetValue(result, DataAccess.FromPrimaryKeyWeak(pi.PropertyType, value));
                        else
                            pi.SetValue(result, Convert.ChangeType(value, pi.PropertyType.GenericTypeArguments[0]));
                    }
                }
                else
                {
                    if (pi.GetCustomAttribute<ForeignKey>() != null)
                        pi.SetValue(result, DataAccess.FromPrimaryKeyWeak(pi.PropertyType, value));
                    else
                        pi.SetValue(result, Convert.ChangeType(value, pi.PropertyType));
                }
            }

            return result;
        }
    }
}
