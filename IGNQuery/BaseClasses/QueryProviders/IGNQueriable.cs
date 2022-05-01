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
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {
        private string fullQuery;
        internal readonly IDataDriver dataDriver = null;
        private readonly IGNQueriable subquery = null;
        internal IDictionary<Type, Type> declaredQueryTypes = null;

        private bool exists;

        private bool canExecute;

        public bool CanExecute
        {
            get
            {
                if(this.subquery != null)
                {
                    return this.canExecute && this.subquery.CanExecute;
                }
                return this.canExecute;
            }
        }

        private readonly IList<IGNParameterValue> paramValues;

        internal IEnumerable<IGNParameterValue> ParamValues
        {
            get
            {
                return paramValues;
            }
        }

        internal IGNQueriable(IDataDriver dataDriver)
        {
            paramValues = new List<IGNParameterValue>();
            this.dataDriver = dataDriver;
            canExecute = true;
            exists = true;
            this.declaredQueryTypes = new Dictionary<Type, Type>
            {
                { typeof(IQueryResult), typeof(QueryResult) }
            };
        }

        internal bool HasSetCommand()
        {
            return fullQuery.Contains("SET");
        }

        internal bool HasDropColumn()
        {
            return fullQuery.Contains("DROP COLUMN");
        }

        internal bool HasAddColumn()
        {
            return fullQuery.Contains("ADD") || fullQuery.Contains("ADD COLUMN");
        }

        internal bool HasAlterColumn()
        {
            return fullQuery.Contains("ADD") || fullQuery.Contains("ALTER COLUMN");
        }

        internal bool HasDropColumnOrSqlServer()
        {
            return IsSqlServer() && HasDropColumn();
        }

        internal bool HasAddColumnOrSqlServer()
        {
            return IsSqlServer() && HasAddColumn();
        }

        internal bool HasAlterColumnOrSqlServer()
        {
            return IsSqlServer() && HasAlterColumn();
        }

        internal bool IsSqlServer()
        {
            return dataDriver.Dialect == DialectEnum.MSSQL;
        }

        internal void AddOperation(string operand, string parameter, string delimiter)
        {
            fullQuery += $"{delimiter}{operand} {parameter}";
        }

        internal string FormatSpSubqueryAndParams(IEnumerable<IGNParameter> fields, IGNQueriable subquery)
        {
            if (dataDriver.Dialect == DialectEnum.MSSQL)
            {
                return $" {GetParams(()=>fields)}\nAS\n{subquery}";
            }
            else if (dataDriver.Dialect == DialectEnum.MySQL)
            {
                return $" ({GetParams(()=>fields)})\nBEGIN\n{subquery}\nEND";
            }
            return string.Empty;
        }

        public IQueryResult Use(string dbName)
        {
            AddOperation("USE", SanitizeName(dbName), "");
            return new QueryResult(this);
        }

        public ICreate Create()
        {
            return new Create(this);
        }

        public IAlter Alter(string table, bool existsCheck)
        {
            return new Alter(this, table, existsCheck);
        }

        public IDrop Drop()
        {
            return new Drop(this);
        }

        public IUpdate Update(string table, bool existsCheck)
        {
            return new Update(this, table, existsCheck);
        }

        public IInsert Insert()
        {
            return new Insert(this);
        }

        public ISelect Select(IEnumerable<string> columns, bool distinct = false)
        {
            return new Select(this, columns, distinct);
        }

        public ISelect Select(bool distinct = false)
        {
            return new Select(this, distinct);
        }

        public IDelete Delete()
        {
            return new Delete(this);
        }

        internal void IfExists(IGNDbObjectTypeEnum objectType,string objectName, string table)
        {
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    dataDriver.IfTableExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    dataDriver.IfDatabaseExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    dataDriver.IfViewExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    dataDriver.IfStoredProcedureExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                    dataDriver.IfIndexExists(objectName, table, this);
                    break;
                case IGNDbObjectTypeEnum.UniqueIndex:
                    dataDriver.IfIndexExists(objectName, table, this);
                    break;
                case IGNDbObjectTypeEnum.Column:
                    dataDriver.IfColumnExists(objectName, table, this);
                    break;
            }
        }

        internal void IfNotExists(IGNDbObjectTypeEnum objectType, string objectName, string table)
        {
            switch (objectType)
            {
                case IGNDbObjectTypeEnum.Table:
                    dataDriver.IfTableNotExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Database:
                    dataDriver.IfDatabaseNotExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.View:
                    dataDriver.IfViewNotExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.StoredProcedure:
                    dataDriver.IfStoredProcedureNotExists(objectName, this);
                    break;
                case IGNDbObjectTypeEnum.Index:
                    dataDriver.IfIndexNotExists(objectName, table, this);
                    break;
                case IGNDbObjectTypeEnum.UniqueIndex:
                    dataDriver.IfIndexNotExists(objectName, table, this);
                    break;
                case IGNDbObjectTypeEnum.Column:
                    dataDriver.IfColumnNotExists(objectName, table, this);
                    break;
            }
        }

        public override string ToString()
        {
            return fullQuery;
        }
    }
}
