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
using IGNQuery.Interfaces;
using IGNQuery.BaseClasses.QueryProviders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using IGNQuery.Enums;
using System.Data;
using IGNQuery.BaseClasses.Business;
using System.Linq;
using IGNActivation.Client.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.BaseClasses
{
    public class IGNDbDriver: IDataDriver
    {
        protected readonly string connectionString;
        private readonly string nonSpecDriverErr = 
            "Please use DatabaseSpecificDriver to construct connection string";
        protected readonly string email;
        protected readonly string key;

        protected DialectEnum dialect;
        public DialectEnum Dialect
        {
            get
            {
                return this.dialect;
            }
        }

        public IGNDbDriver(string email, string key)
        {
            this.connectionString = ConstructDefaultConnectionString();
            this.email = email;
            this.key = key;
        }

        public IGNDbDriver(string email, string server, int port, string uName, string pwd, string key)
        {
            this.connectionString = ConstructConnectionString(server, port, uName, pwd);
            this.email = email;
            this.key = key;
        }

        public IGNDbDriver(string email, string connectionString, string key)
        {
            this.connectionString = connectionString;
            this.email = email;
            this.key = key;
        }

        public virtual void Dispose()
        {

        }

        protected virtual string ConstructConnectionString(
            string server, int port, string uName, string pwd
        )
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        protected virtual string ConstructDefaultConnectionString()
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        protected virtual DbConnection OpenConnection()
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        protected virtual DbCommand PrepareDbCommand(
            string query, DbConnection connection
        )
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        protected virtual void AddParameters(DbCommand dbc, IEnumerable<IGNParameterValue> args)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        protected virtual DataTable InitDataTable(DbDataReader reader)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public void IfTableNotExists(string name, IGNQueriable queriable)
        {
            SetTableExists(name, queriable, ExistsEnum.NotExists);
        }

        public void IfDatabaseNotExists(string name, IGNQueriable queriable)
        {
            SetDatabaseExists(name, queriable, ExistsEnum.NotExists);
        }

        public virtual void IfStoredProcedureNotExists(string name, IGNQueriable queriable)
        {
            SetStoredProcedureExists(name, queriable, ExistsEnum.NotExists);
        }
        public void IfColumnNotExists(string name, string table, IGNQueriable queriable)
        {
            SetColumnExists(name, table, queriable, ExistsEnum.NotExists);
        }
        public void IfViewNotExists(string name, IGNQueriable queriable)
        {
            SetViewExists(name, queriable, ExistsEnum.NotExists);
        }

        public void IfIndexNotExists(string name, string table, IGNQueriable queriable)
        {
            SetIndexExists(name, table, queriable, ExistsEnum.NotExists);
        }

        public void IfPrimaryKeyNotExists(string name, string table, IGNQueriable queriable)
        {
            SetPrimaryKeyExists(name, table, queriable, ExistsEnum.NotExists);
        }

        public void IfTableExists(string name, IGNQueriable queriable)
        {
            SetTableExists(name, queriable, ExistsEnum.Exists);
        }

        public void IfDatabaseExists(string name, IGNQueriable queriable)
        {
            SetDatabaseExists(name, queriable, ExistsEnum.Exists);
        }

        public virtual void IfStoredProcedureExists(string name, IGNQueriable queriable)
        {
            SetStoredProcedureExists(name, queriable, ExistsEnum.Exists);
        }
        public void IfColumnExists(string name, string table, IGNQueriable queriable)
        {
            SetColumnExists(name, table, queriable, ExistsEnum.Exists);
        }
        public void IfViewExists(string name, IGNQueriable queriable)
        {
            SetViewExists(name, queriable, ExistsEnum.Exists);
        }

        public void IfIndexExists(string name, string table, IGNQueriable queriable)
        {
            SetIndexExists(name, table, queriable, ExistsEnum.Exists);
        }

        public void IfPrimaryKeyExists(string name, string table, IGNQueriable queriable)
        {
            SetPrimaryKeyExists(name, table, queriable, ExistsEnum.Exists);
        }

        public virtual string GoTerminator()
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        private void SetDatabaseExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var query = IGNQueriable.Begin(this.email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.SHEMATA",false).
                Where(IGNConditionWithParameter.FromConfig("CATALOG_NAME",IGNSqlCondition.Eq,0)).
                Go();
            var result = ReadDataWithParameters(query, 
                new List<IGNParameterValue> { IGNParameterValue.FromConfig(0, name) });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        private void SetStoredProcedureExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var firstQueryPart = IGNQueriable.Begin(this.email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.ROUTINES",false);
            IGroupableCondition query = null;
            if(this.dialect == DialectEnum.MSSQL)
            {
                query = firstQueryPart.
                    Where(IGNConditionWithParameter.FromConfig("ROUTINE_CATALOG", IGNSqlCondition.Eq, 0));
            }
            if (this.dialect == DialectEnum.MySQL)
            {
                query = firstQueryPart.
                    Where(IGNConditionWithParameter.FromConfig("ROUTINE_SCHEMA", IGNSqlCondition.Eq, 0));
            }
            query = query.And(IGNConditionWithParameter.FromConfig("ROUTINE_NAME", IGNSqlCondition.Eq, 1));
            var checkQuery = query.Go();
            var result = ReadDataWithParameters(checkQuery, new List<IGNParameterValue> 
            {
                IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                IGNParameterValue.FromConfig(1, name)
            });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        private void SetIndexExists(string name, string table, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            IGNQueriable checkQuery = null;
            DataTable result = null;
            if(Dialect == DialectEnum.MySQL)
            {
                checkQuery = IGNQueriable.Begin(email, this, this.key).
                    Select(false).
                    ConditionalFrom("INFROMATION_SCHEMA.STATISTICS", false).
                    Where(IGNConditionWithParameter.FromConfig("TABLE_SCHEMA", IGNSqlCondition.Eq, 0)).
                    And(IGNConditionWithParameter.FromConfig("TABLE_NAME", IGNSqlCondition.Eq, 1)).
                    And(IGNConditionWithParameter.FromConfig("INDEX_NAME", IGNSqlCondition.Eq, 2)).
                    Go();
                result = ReadDataWithParameters(checkQuery, new List<IGNParameterValue>
                {
                    IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                    IGNParameterValue.FromConfig(1, table),
                    IGNParameterValue.FromConfig(2, name)
                });
            }
            if(Dialect == DialectEnum.MSSQL)
            {
                checkQuery = IGNQueriable.Begin(email, this, this.key).
                    Select(false).
                    ConditionalFrom("sysindexes", false).
                    Where(IGNConditionWithParameter.FromConfig("name", IGNSqlCondition.Eq, 0)).
                    Go();
                result = ReadDataWithParameters(checkQuery, new List<IGNParameterValue>
                {
                    IGNParameterValue.FromConfig(0, name)
                });
            }
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        private void SetTableExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var firstPartQuery = IGNQueriable.Begin(email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.TABLES", false);
            IGroupableCondition partialQuery = null;
            if (this.dialect == DialectEnum.MSSQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_CATALOG", IGNSqlCondition.Eq, 0));
            }
            if (this.dialect == DialectEnum.MySQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_SCHEMA", IGNSqlCondition.Eq, 0));
            }
            partialQuery = partialQuery.And(IGNConditionWithParameter.FromConfig("TABLE_NAME", IGNSqlCondition.Eq, 1));
            var query = partialQuery.Go();
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>
            {
                IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                IGNParameterValue.FromConfig(1, name)
            });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }


        private void SetViewExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var firstPartQuery = IGNQueriable.Begin(email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.VIEWS", false);
            IGroupableCondition partialQuery = null;
            if (this.dialect == DialectEnum.MSSQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_CATALOG", IGNSqlCondition.Eq, 0));
            }
            if (this.dialect == DialectEnum.MySQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_SCHEMA", IGNSqlCondition.Eq, 0));
            }
            partialQuery = partialQuery.And(IGNConditionWithParameter.FromConfig("TABLE_NAME", IGNSqlCondition.Eq, 1));
            var query = partialQuery.Go();
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>
            {
                IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                IGNParameterValue.FromConfig(1, name)
            });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        private void SetColumnExists(string name, string table, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var firstPartQuery = IGNQueriable.Begin(email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.COLUMNS", false);
            IGroupableCondition partialQuery = null;
            if (this.dialect == DialectEnum.MSSQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_CATALOG", IGNSqlCondition.Eq, 0));
            }
            if (this.dialect == DialectEnum.MySQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_SCHEMA", IGNSqlCondition.Eq, 0));
            }
            partialQuery = partialQuery.And(IGNConditionWithParameter.FromConfig("TABLE_NAME", IGNSqlCondition.Eq, 1)).
                And(IGNConditionWithParameter.FromConfig("COLUMN_NAME", IGNSqlCondition.Eq, 2));
            var query = partialQuery.Go();
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>
            {
                IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                IGNParameterValue.FromConfig(1, table),
                IGNParameterValue.FromConfig(2, name)
            });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        private void SetPrimaryKeyExists(string name, string table, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var firstPartQuery = IGNQueriable.Begin(email, this, this.key).
                Select().
                ConditionalFrom("INFORMATION_SCHEMA.TABLE_CONSTRAINTS", false);
            IGroupableCondition partialQuery = null;
            if (this.dialect == DialectEnum.MSSQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_CATALOG", IGNSqlCondition.Eq, 0));
            }
            if (this.dialect == DialectEnum.MySQL)
            {
                partialQuery = firstPartQuery.
                    Where(IGNConditionWithParameter.FromConfig("TABLE_SCHEMA", IGNSqlCondition.Eq, 0));
            }
            partialQuery = partialQuery.And(IGNConditionWithParameter.FromConfig("TABLE_NAME", IGNSqlCondition.Eq, 1)).
                And(IGNConditionWithParameter.FromConfig("CONSTRAINT_NAME", IGNSqlCondition.Eq, 2));
            var query = partialQuery.Go();
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>
            {
                IGNParameterValue.FromConfig(0, GetDatabaseName(queriable.DatabaseNameQuery())),
                IGNParameterValue.FromConfig(1, table),
                IGNParameterValue.FromConfig(2, name)
            });
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public void Execute(IGNQueriable query)
        {
            ExecuteWithParameters(query, query.ParamValues);
        }

        public virtual string GetDbAutoGenFor(Type clrType, int length)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public void ExecuteWithParameters(IGNQueriable query, IEnumerable<IGNParameterValue> args)
        {
            if (query.CanExecute)
            {
                using (var connection = OpenConnection())
                {
                    var dbc = PrepareDbCommand(query.ToString(), connection);
                    AddParameters(dbc, args);
                    dbc.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteStoredProcedure(string procName, IEnumerable<IGNParameterValue> args)
        {
            using (var connection = OpenConnection())
            {
                var query = $"CALL {procName}({string.Join(",",args.Select(x=>$"@p{x.Position}"))})";
                if(dialect == DialectEnum.MSSQL)
                {
                    query = $"EXEC {procName} {string.Join(",", args.Select(x => $"@p{x.Position}"))}";
                }
                var dbc = PrepareDbCommand(query, connection);
                dbc.CommandType = CommandType.StoredProcedure;
                AddParameters(dbc, args);
                dbc.ExecuteNonQuery();
            }
        }

        public DataTable ReadData(IGNQueriable query)
        {
            if (query.CanExecute)
            {
                return ReadDataWithParameters(query, query.ParamValues);
            }
            return null;
        }

        public DataTable ReadDataWithParameters(IGNQueriable query, IEnumerable<IGNParameterValue> args)
        {
            DataTable result = null;
            if (query.CanExecute)
            {
                using (var connection = OpenConnection())
                {
                    var dbc = PrepareDbCommand(query.ToString(), connection);
                    AddParameters(dbc, args);
                    using (var dbReader = dbc.ExecuteReader())
                    {
                        result = InitDataTable(dbReader);
                    }
                }
            }
            return result;
        }

        public DataTable ReadDataFromStoredProcedure(string procName, IEnumerable<IGNParameterValue> args)
        {
            DataTable result = null;
            using (var connection = OpenConnection())
            {
                var query = $"CALL {procName}({string.Join(",", args.Select(x => $"@p{x.Position}"))})";
                if (dialect == DialectEnum.MSSQL)
                {
                    query = $"EXEC {procName} {string.Join(",", args.Select(x => $"@p{x.Position}"))}";
                }
                var dbc = PrepareDbCommand(query, connection);
                dbc.CommandType = CommandType.StoredProcedure;
                AddParameters(dbc, args);
                using (var dbReader = dbc.ExecuteReader())
                {
                    result = InitDataTable(dbReader);
                }
            }
            return result;
        }

        public void AssignActivator(IActivationClient activator, string email, string key)
        {
            Activation.Init(activator);
            Activation.Activate(email, key);
        }

        public void AssignActivator(string email, string key)
        {
            Activation.Activate(email, key);
        }

        public string GetDatabaseName(string dbFunction)
        {
            var query = IGNQueriable.FromQueryString($"SELECT {dbFunction}{GoTerminator()}", email, this, this.key);
            var result = ReadData(query);
            if(result.Rows.Count > 0)
            {
                var row = result.Select().FirstOrDefault();
                if(row != null && !row.IsNull(0))
                {
                    return row[0].ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
