﻿using NpgsqlTypes;
using System;

namespace NerdBlock.Engine.Backend.PgImplementation
{
    /// <summary>
    /// A utility class for our postgres implementation
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Converts a QueryParamType to an NpgsqlDbTyp
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The NpgsqlDbType that matches our internal type</returns>
        public static NpgsqlDbType ToNpgsql(this QueryParamType value)
        {
            switch(value)
            {
                case QueryParamType.Bit:
                    return NpgsqlDbType.Bit;
                case QueryParamType.Boolean:
                    return NpgsqlDbType.Boolean;
                case QueryParamType.Box:
                    return NpgsqlDbType.Box;
                case QueryParamType.ByteArray:
                    return NpgsqlDbType.Bytea;
                case QueryParamType.Char:
                    return NpgsqlDbType.Char;
                case QueryParamType.Circle:
                    return NpgsqlDbType.Circle;
                case QueryParamType.Date:
                    return NpgsqlDbType.Date;
                case QueryParamType.Double:
                    return NpgsqlDbType.Double;
                case QueryParamType.Integer:
                    return NpgsqlDbType.Integer;
                case QueryParamType.IpAddress:
                    return NpgsqlDbType.Cidr;
                case QueryParamType.Json:
                    return NpgsqlDbType.Json;
                case QueryParamType.Line:
                    return NpgsqlDbType.Line;
                case QueryParamType.LineSegment:
                    return NpgsqlDbType.LSeg;
                case QueryParamType.Long:
                    return NpgsqlDbType.Bigint;
                case QueryParamType.MacAddress:
                    return NpgsqlDbType.MacAddr;
                case QueryParamType.Money:
                    return NpgsqlDbType.Money;
                case QueryParamType.Numeric:
                    return NpgsqlDbType.Numeric;
                case QueryParamType.Path:
                    return NpgsqlDbType.Path;
                case QueryParamType.Point:
                    return NpgsqlDbType.Point;
                case QueryParamType.Polygon:
                    return NpgsqlDbType.Polygon;
                case QueryParamType.Short:
                    return NpgsqlDbType.Smallint;
                case QueryParamType.Single:
                    return NpgsqlDbType.Real;
                case QueryParamType.Text:
                    return NpgsqlDbType.Text;
                case QueryParamType.Time:
                    return NpgsqlDbType.Time;
                case QueryParamType.TimeSpan:
                    return NpgsqlDbType.Interval;
                case QueryParamType.TimeStamp:
                    return NpgsqlDbType.Timestamp;
                case QueryParamType.Uuid:
                    return NpgsqlDbType.Uuid;
                case QueryParamType.VarBit:
                    return NpgsqlDbType.Varbit;
                case QueryParamType.VarChar:
                    return NpgsqlDbType.Varchar;
                case QueryParamType.Xml:
                    return NpgsqlDbType.Xml;
                default:
                    throw new NotSupportedException("No PGSQL command defined for " + value);
            }

        }
    }
}
