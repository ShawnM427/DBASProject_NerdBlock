﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdBlock.Sandbox.Backend
{
    public class DataModel : Attribute
    {
        public string TableName;

        public DataModel(string tableName)
        {
            TableName = tableName;
        }
    }

    public class DataField : Attribute
    {
        public string FieldName;
        public QueryParamType FieldType;

        public DataField(string fieldName, QueryParamType fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }
    }
    
    public class PrimaryKey : Attribute
    {

    }

    public class ForeignKey : Attribute
    {
        public string TableName;
        public string FieldName;

        public ForeignKey(string tableName, string fieldName)
        {
            TableName = tableName;
            FieldName = fieldName;
        }
    }

    public class AutoGenerated : Attribute
    {

    }

    public class Nullable : Attribute
    {

    }
}