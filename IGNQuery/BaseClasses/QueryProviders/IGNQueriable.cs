﻿//#############################################
//
//  MIT License
//
//  Copyright(c) 2020 Oleg Golovchenko
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.  
//
// ############################################

using IGNQuery.Enums;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {
        private IDataDriver dataDriver = null;
        private string query = "";
        private string format = "";
        private IGNDbObjectTypeEnum objectType = IGNDbObjectTypeEnum.None;
        private string objectName;
        private string prefix = "";
        private string suffix = "";
        private string afterObjType = "";
        private string querySpecificPart = "";
        private string insertValuesQueryPart = "";
        private string conditional = "";
        private IEnumerable<string> fieldNames;
        private IGNQueriable subquery = null;

        /// <summary>
        /// Format for create query
        /// {0:prefix} CREATE {1:objtype} {2:afterobjtype} 
        ///     {3:objname} {4:queryspecificpart} {5:suffix} {6:go}
        /// </summary>
        private const string CREATE_QUERY_FORMAT = 
            "{0}CREATE {1}{2} {3}{4}{5}{6}";
        /// <summary>
        /// format for use query
        /// USE {0:objname} {1:go}
        /// </summary>
        private const string USE_QUERY_FORMAT = "USE {0}{1}";
        /// <summary>
        /// format for drop query
        /// DROP {0:objname} {1:go}
        /// </summary>
        private const string DROP_QUERY_FORMAT = "{0}DROP {1}{2} {3}{4}{5}";
        /// <summary>
        /// Format for create query
        /// {0:prefix} ALTER {1:objtype} {2:afterobjtype} 
        ///     {3:objname} {4:add drop or alter column query}
        ///     {5:suffix} {6:go}
        /// </summary>
        private const string ALTER_QUERY_FORMAT = "{0}ALTER {1}{2} {3}\n{4}{5}{6}";
        /// <summary>
        /// Format for add column query
        /// ADD {0:objtype(always column)} {1:name} {2:definition}
        /// </summary>
        private const string ADD_QUERY_FORMAT = "ADD {0} {1} {2}";
        /// <summary>
        /// Format for alter column query
        /// ALTER {0:objtype(always column)} {1:name} {2:definition}
        /// </summary>
        private const string ALTER_COLUMN_QUERY_FORMAT = "ALTER {0} {1} {2}";
        /// <summary>
        /// Format for drop column query
        /// DROP {0:objtype(always column)} {1:name} {2:definition}
        /// </summary>
        private const string DROP_COLUMN_QUERY_FORMAT = "DROP {0} {1} {2}";
        /// <summary>
        /// Format for update query
        /// UPDATE {0:name} {1:set query} {2:condition}
        /// </summary>
        private const string UPDATE_QUERY_FORMAT = "UPDATE {0} {1} {2}";
        /// <summary>
        /// Format for insert query
        /// INSERT {0:into query} {1:values}
        /// </summary>
        private const string INSERT_QUERY_FORMAT = "INSERT INTO {0} VALUES({1})";
        /// <summary>
        /// Format for select query
        /// SELECT {0:fields} FROM {1:objName} {2:query part} {3:condition}
        /// </summary>
        private const string SELECT_QUERY_FORMAT = "SELECT {0} FROM {1} {2} {3}";
        /// <summary>
        /// Format for delete query
        /// DELETE FROM {0:objName} {2:condition}
        /// </summary>
        private const string DELETE_QUERY_FORMAT = "DELETE FROM {0} {1}";

        internal bool IsAddColumn
        {
            get
            {
                return this.format == ADD_QUERY_FORMAT;
            }
        }

        internal bool IsDropColumn
        {
            get
            {
                return this.format == DROP_COLUMN_QUERY_FORMAT ||
                    this.format == DROP_QUERY_FORMAT;
            }
        }

        internal IGNQueriable(IDataDriver dataDriver)
        {
            this.dataDriver = dataDriver;
            this.objectType = IGNDbObjectTypeEnum.None;
        }

        public IGNQueriable Use(string dbName)
        {
            this.format = USE_QUERY_FORMAT;
            this.objectType = IGNDbObjectTypeEnum.Database;
            this.objectName = dbName;
            return this;
        }

        public IGNQueriable Create()
        {
            this.format = CREATE_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Alter()
        {
            this.format = ALTER_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Add()
        {
            this.format = ADD_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Drop()
        {
            this.format = DROP_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Update()
        {
            this.format = UPDATE_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Insert()
        {
            this.format = INSERT_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Select(IEnumerable<string> columns)
        {
            this.fieldNames = columns;
            this.format = SELECT_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Select()
        {
            this.fieldNames = new List<string>();
            this.format = SELECT_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Delete()
        {
            this.format = DELETE_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable From(string tableName)
        {
            this.objectName = tableName;
            return this;
        }

        public IGNQueriable Column(string name)
        {
            if (this.format == DROP_QUERY_FORMAT)
            {
                this.format = DROP_COLUMN_QUERY_FORMAT;
            }
            this.objectType = IGNDbObjectTypeEnum.Column;
            this.objectName = name;
            return this;
        } 

        public IGNQueriable Column(string name, 
            Type colType, 
            int length, 
            bool reqired,
            bool autoGenerated,
            object defvalue=null)
        {
            if(this.format == ALTER_QUERY_FORMAT)
            {
                this.format = ALTER_COLUMN_QUERY_FORMAT;
            }
            this.objectType = IGNDbObjectTypeEnum.Column;
            this.objectName = name;
            this.querySpecificPart = $"{GetDbType(colType, length)} {FormatFieldOptionals(colType, length, reqired, autoGenerated, defvalue)}";
            return this;
        }

        public IGNQueriable Into(string tableName, IEnumerable<string> columns)
        {
            this.querySpecificPart = $"{tableName} ({string.Join(",", columns)})";
            return this;
        }

        public IGNQueriable Values(IEnumerable<object> valuesRow)
        {
            this.insertValuesQueryPart = $"({string.Join(",", valuesRow)})";
            return this;
        }

        public IGNQueriable AddRow(IEnumerable<object> valuesRow)
        {
            this.insertValuesQueryPart += $",({string.Join(",",valuesRow)})";
            return this;
        }

        public IGNQueriable ValuesWithParams(IEnumerable<int> valuesRow)
        {
            this.insertValuesQueryPart = $"({string.Join(",", valuesRow.Select(x=>$"@p{x}"))})";
            return this;
        }

        public IGNQueriable AddRowWithParams(IEnumerable<int> valuesRow)
        {
            this.insertValuesQueryPart += $",({string.Join(",", valuesRow.Select(x => $"@p{x}"))})";
            return this;
        }

        public IGNQueriable SetParametrized(string colName, int parameter)
        {
            this.querySpecificPart = $"SET {colName} = @p{parameter}";
            return this;
        }

        public IGNQueriable Set(string colName, object value)
        {
            var result = $"{value}";
            if(value.GetType() == typeof(string))
            {
                result = "'" + result + "'";
            }
            if(value.GetType() == typeof(bool))
            {
                result = $"{((bool)value ? "'1'" : "'0'")}";
            }
            this.querySpecificPart = $"SET {colName} = {result}";
            return this;
        }

        public IGNQueriable Where()
        {
            this.conditional = "WHERE ";
            return this;
        }

        public IGNQueriable Condition(Func<Tuple<string, IGNSqlCondition, object>> conditionalFunc)
        {
            var result = conditionalFunc?.Invoke();
            var operation = "";
            switch (result.Item2)
            {
                case IGNSqlCondition.In:
                    operation = "IN";
                    break;
                case IGNSqlCondition.Between:
                    operation = "BETWEEN";
                    break;
                case IGNSqlCondition.Like:
                    operation = "LIKE";
                    break;
                case IGNSqlCondition.Eq:
                    operation = "=";
                    break;
                case IGNSqlCondition.Ge:
                    operation = ">=";
                    break;
                case IGNSqlCondition.Gt:
                    operation = ">";
                    break;
                case IGNSqlCondition.Le:
                    operation = "<=";
                    break;
                case IGNSqlCondition.Lt:
                    operation = "<";
                    break;
                case IGNSqlCondition.Ne:
                    operation = "<>";
                    break;
            }
            var value = $"{result.Item3}";
            if (result.Item3.GetType() == typeof(string))
            {
                value = "'" + value + "'";
            }
            if (result.Item3.GetType() == typeof(bool))
            {
                value = $"{((bool)result.Item3 ? "'1'" : "'0'")}";
            }
            this.conditional += $"{result.Item1} {operation} {value}";
            return this;
        }

        public IGNQueriable ConditionWithParams(Func<Tuple<string, IGNSqlCondition, int>> conditionalFunc)
        {
            var result = conditionalFunc?.Invoke();
            var operation = "";
            switch (result.Item2)
            {
                case IGNSqlCondition.In:
                    operation = "IN";
                    break;
                case IGNSqlCondition.Between:
                    operation = "BETWEEN";
                    break;
                case IGNSqlCondition.Like:
                    operation = "LIKE";
                    break;
                case IGNSqlCondition.Eq:
                    operation = "=";
                    break;
                case IGNSqlCondition.Ge:
                    operation = ">=";
                    break;
                case IGNSqlCondition.Gt:
                    operation = ">";
                    break;
                case IGNSqlCondition.Le:
                    operation = "<=";
                    break;
                case IGNSqlCondition.Lt:
                    operation = "<";
                    break;
                case IGNSqlCondition.Ne:
                    operation = "<>";
                    break;
            }
            var value = $"{result.Item3}";
            if (result.Item3.GetType() == typeof(string))
            {
                value = "'" + value + "'";
            }
            this.conditional += $"{result.Item1} {operation} @p{result.Item3}";
            return this;
        }

        public IGNQueriable And()
        {
            this.conditional += " AND ";
            return this;
        }

        public IGNQueriable Or()
        {
            this.conditional += " OR ";
            return this;
        }

        public IGNQueriable Not()
        {
            this.conditional += " NOT ";
            return this;
        }

        public IGNQueriable IfNotExists()
        {
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    this.dataDriver.IfTableNotExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    this.dataDriver.IfDatabaseNotExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    this.dataDriver.IfViewNotExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    this.dataDriver.IfStoredProcedureNotExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                case IGNDbObjectTypeEnum.UniqueIndex:
                    this.dataDriver.IfIndexNotExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.None:
                default:
                    throw new Exception("Please call if not exists after Create update or alter ddl query");
            }
            return this;
        }

        public IGNQueriable IfExists()
        {
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    this.dataDriver.IfTableExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    this.dataDriver.IfDatabaseExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    this.dataDriver.IfViewExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    this.dataDriver.IfStoredProcedureExists(this.objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                case IGNDbObjectTypeEnum.UniqueIndex:
                    this.dataDriver.IfIndexExists(this.objectName, this);
                    break;
                default:
                    throw new Exception("Please call if exists after Create update or alter ddl query");
            }
            return this;
        }

        private void BuildQuery()
        {
            if (!string.IsNullOrWhiteSpace(this.format))
            {
                string objType = "";
                if (this.format != SELECT_QUERY_FORMAT &&
                    this.format != UPDATE_QUERY_FORMAT &&
                    this.format != INSERT_QUERY_FORMAT &&
                    this.format != DELETE_QUERY_FORMAT)
                {
                    switch (objectType)
                    {
                        case IGNDbObjectTypeEnum.Table:
                            objType = "TABLE";
                            break;
                        case IGNDbObjectTypeEnum.Database:
                            objType = "DATABASE";
                            break;
                        case IGNDbObjectTypeEnum.View:
                            objType = "VIEW";
                            break;
                        case IGNDbObjectTypeEnum.StoredProcedure:
                            objType = "PROCEDURE";
                            break;
                        case IGNDbObjectTypeEnum.Index:
                            objType = "INDEX";
                            break;
                        case IGNDbObjectTypeEnum.UniqueIndex:
                            objType = "UNIQUE INDEX";
                            break;
                        case IGNDbObjectTypeEnum.Column:
                            objType = "COLUMN";
                            break;
                        default:
                            throw new Exception("Invalid ddl query syntax");
                    }
                }
                if (string.IsNullOrWhiteSpace(this.query))
                {
                    switch (this.format)
                    {
                        case USE_QUERY_FORMAT:
                            BuildUseQuery();
                            break;
                        case CREATE_QUERY_FORMAT:
                            BuildCreateQuery(objType);
                            break;
                        case DROP_QUERY_FORMAT:
                            BuildDropQuery(objType);
                            break;
                        case ADD_QUERY_FORMAT:
                            BuildAddQuery(objType);
                            break;
                        case ALTER_QUERY_FORMAT:
                            BuildAlterQuery(objType);
                            break;
                        case ALTER_COLUMN_QUERY_FORMAT:
                            BuildAlterColumnQuery(objType);
                            break;
                        case DROP_COLUMN_QUERY_FORMAT:
                            BuildDropColumnQuery(objType);
                            break;
                        case UPDATE_QUERY_FORMAT:
                            BuildUpdateQuery();
                            break;
                        case INSERT_QUERY_FORMAT:
                            BuildInsertQuery();
                            break;
                        case SELECT_QUERY_FORMAT:
                            BuildSelectQuery();
                            break;
                        case DELETE_QUERY_FORMAT:
                            BuildDeleteQuery();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void ReplaceAlterSubquery(IGNQueriable alterQuery)
        {
            this.querySpecificPart = alterQuery.ToString();
            this.subquery = alterQuery;
        }

        public override string ToString()
        {
            this.BuildQuery();
            return query;
        }
    }
}