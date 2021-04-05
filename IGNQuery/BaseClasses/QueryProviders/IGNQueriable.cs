//#############################################
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

using IGNQuery.BaseClasses.Business;
using IGNQuery.Enums;
using IGNQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {
        private readonly IDataDriver dataDriver = null;
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
        /// Format for alter column query
        /// ALTER {0:objtype(always column)} {1:name} {2:definition}
        /// </summary>
        private const string ALTER_COLUMN_QUERY_FORMAT_MYSQL = "MODIFY {0} {1} {2}";
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

        private IList<IGNParameterValue> paramValues;

        internal IEnumerable<IGNParameterValue> ParamValues
        {
            get
            {
                return paramValues;
            }
        }

        private int lastParamIndex = 0;

        internal IGNQueriable(IDataDriver dataDriver)
        {
            this.paramValues = new List<IGNParameterValue>();
            this.dataDriver = dataDriver;
            this.objectType = IGNDbObjectTypeEnum.None;
        }

        public IGNQueriable Use(string dbName)
        {
            this.format = USE_QUERY_FORMAT;
            this.objectType = IGNDbObjectTypeEnum.Database;
            this.objectName = SanitizeName(dbName);
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
            this.fieldNames = columns.Select(name=> SanitizeName(name));
            this.format = SELECT_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Select()
        {
            this.fieldNames = new List<string>();
            this.format = SELECT_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable Join(string sourceTableName,
            string destTableName,
            string sourceKeyName, 
            string destKeyName)
        {
            this.querySpecificPart = $"INNER JOIN {SanitizeName(destTableName)} ON " +
                $"{SanitizeName(sourceTableName)}.{SanitizeName(sourceKeyName)} = " +
                $"{SanitizeName(destTableName)}.{SanitizeName(destKeyName)}";
            return this;
        }

        public IGNQueriable Delete()
        {
            this.format = DELETE_QUERY_FORMAT;
            return this;
        }

        public IGNQueriable From(string tableName)
        {
            this.objectName = SanitizeName(tableName);
            return this;
        }

        public IGNQueriable Column(string name)
        {
            if (this.format == DROP_QUERY_FORMAT)
            {
                this.format = DROP_COLUMN_QUERY_FORMAT;
            }
            this.objectType = IGNDbObjectTypeEnum.Column;
            this.objectName = SanitizeName(name);
            return this;
        } 

        public IGNQueriable Column(string name, 
            Type colType, 
            int length, 
            bool reqired,
            bool autoGenerated,
            object defvalue=null,
            int decpos=0)
        {
            if(this.format == ALTER_QUERY_FORMAT)
            {
                this.format = ALTER_COLUMN_QUERY_FORMAT;
                if(this.dataDriver.Dialect == DialectEnum.MySQL)
                {
                    this.format = ALTER_COLUMN_QUERY_FORMAT_MYSQL;
                }
            }
            this.objectType = IGNDbObjectTypeEnum.Column;
            this.objectName = SanitizeName(name);
            this.querySpecificPart = $"{GetDbType(colType, length, decpos)} {FormatFieldOptionals(colType, length, reqired, autoGenerated, defvalue)}";
            return this;
        }

        public IGNQueriable Into(string tableName, IEnumerable<string> columns)
        {
            this.querySpecificPart = $"{SanitizeName(tableName)} ({string.Join(",", columns.Select(name=> SanitizeName(name)))})";
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
            this.querySpecificPart = $"SET {SanitizeName(colName)} = @p{parameter}";
            return this;
        }

        public IGNQueriable Where()
        {
            this.conditional = "WHERE ";
            return this;
        }

        public IGNQueriable ConditionWithParams(Func<IGNConditionWithParameter> conditionalFunc)
        {
            var condition = conditionalFunc?.Invoke();
            condition.SetSanitizedName(SanitizeName(condition.ColumnName));
            this.conditional += condition.ToString();
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
            var objName = DeSanitizeName(this.objectName);
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    this.dataDriver.IfTableNotExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    this.dataDriver.IfDatabaseNotExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    this.dataDriver.IfViewNotExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    this.dataDriver.IfStoredProcedureNotExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                case IGNDbObjectTypeEnum.UniqueIndex:
                    this.dataDriver.IfIndexNotExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.None:
                default:
                    throw new Exception("Please call if not exists after Create update or alter ddl query");
            }
            return this;
        }

        public IGNQueriable IfExists()
        {
            var objName = DeSanitizeName(this.objectName);
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    this.dataDriver.IfTableExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    this.dataDriver.IfDatabaseExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    this.dataDriver.IfViewExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    this.dataDriver.IfStoredProcedureExists(objName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                case IGNDbObjectTypeEnum.UniqueIndex:
                    this.dataDriver.IfIndexExists(objName, this);
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
                            BuildAddQuery();
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

        internal string SanitizeName(string objName)
        {
            var sanitizedFormat = "`{0}`";
            if(this.dataDriver.Dialect == DialectEnum.MSSQL)
            {
                sanitizedFormat = "[{0}]";
            }
            return string.Format(sanitizedFormat, objName);
        }

        internal string DeSanitizeName(string objName)
        {
            return objName.Substring(1, objName.Length - 2);
        }
    }
}
