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
using System.Text;
using System.Data.Common;
using IGNQuery.Enums;
using System.Data;
using IGNQuery.BaseClasses.Business;
using System.Linq;

namespace IGNQuery.BaseClasses
{
    public class IGNDbDriver: IDataDriver
    {
        protected readonly string connectionString;
        private readonly string nonSpecDriverErr = 
            "Please use DatabaseSpecificDriver to construct connection string";

        protected DialectEnum dialect;
        public DialectEnum Dialect
        {
            get
            {
                return this.dialect;
            }
        }

        public IGNDbDriver()
        {
            this.connectionString = ConstructDefaultConnectionString();
        }

        public IGNDbDriver(string server,int port,string uName, string pwd)
        {
            this.connectionString = ConstructConnectionString(server, port, uName, pwd);
        }

        public IGNDbDriver(string connectionString)
        {
            this.connectionString = connectionString;
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

        public virtual void IfTableNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfDatabaseNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfStoredProcedureNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }
        public virtual void IfColumnNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }
        public virtual void IfViewNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfIndexNotExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfTableExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfDatabaseExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfStoredProcedureExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }
        public virtual void IfColumnExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }
        public virtual void IfViewExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual void IfIndexExists(string name, IGNQueriable queriable)
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
        }

        public virtual string GoTerminator()
        {
            throw new NotImplementedException(this.nonSpecDriverErr);
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
    }
}
