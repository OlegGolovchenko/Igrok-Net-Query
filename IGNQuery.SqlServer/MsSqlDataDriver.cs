using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Enums;
using IGNQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IGNQuery.SqlServer
{
    public class MsSqlDataDriver : IGNDbDriver
    {

        private string email;

        public MsSqlDataDriver(string email, string server, int port,string user,string password):base(server,port,user,password)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
            this.email = email;
        }

        public MsSqlDataDriver(string email) : base()
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
            this.email = email;
        }

        public MsSqlDataDriver(string email, string connectionString):base(connectionString)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
            this.email = email;
        }

        protected override string ConstructConnectionString(string server, int port, string uName, string pwd)
        {
            if (!string.IsNullOrWhiteSpace(uName))
            {
                return $"Server={server},{port};database=master;user id={uName};password={pwd};";
            }
            else
            {
                return $"Server={server},{port};database=master;integrated security=true;";
            }
        }

        protected override string ConstructDefaultConnectionString()
        {
            return Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING"); ;
        }

        protected override DbConnection OpenConnection()
        {
            var connection = new SqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }

        protected override DbCommand PrepareDbCommand(string query, DbConnection connection)
        {
            return new SqlCommand(query)
            {
                Connection = (SqlConnection)connection
            };
        }

        protected override void AddParameters(DbCommand dbc, IEnumerable<IGNParameterValue> args)
        {
            foreach(var arg in args)
            {
                ((SqlCommand)dbc).Parameters.AddWithValue($"@p{arg.Position}", arg.Value);
            }
        }

        protected override DataTable InitDataTable(DbDataReader reader)
        {
            var result = new DataTable();
            result.Load(reader);
            return result;
        }

        public override string GoTerminator()
        {
            return "";
        }

        public override string GetDbAutoGenFor(Type clrType, int length)
        {
            if(clrType == typeof(long) ||
               clrType == typeof(int) ||
               clrType == typeof(short) ||
               clrType == typeof(byte))
            {
                return " IDENTITY";
            }
            if(clrType == typeof(Guid))
            {
                return " DEFAULT NEWSEQUENTIALID()";
            }
            if (clrType == typeof(DateTime))
            {
                return " DEFAULT GETUTCDATE()";
            }
            return "";
        }

        public override void IfTableExists(string name, IGNQueriable queriable)
        {
            SetTableExists(name, queriable, ExistsEnum.Exists);
        }

        public override void IfTableNotExists(string name, IGNQueriable queriable)
        {
            SetTableExists(name, queriable, ExistsEnum.NotExists);
        }

        private void SetTableExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_CATALOG] = DB_NAME() AND [TABLE_NAME] = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfDatabaseNotExists(string name, IGNQueriable queriable)
        {
            SetDatabaseExists(name, queriable, ExistsEnum.NotExists);
        }

        public override void IfDatabaseExists(string name, IGNQueriable queriable)
        {
            SetDatabaseExists(name, queriable, ExistsEnum.Exists);
        }

        private void SetDatabaseExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM sysdatabases WHERE name='{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfViewExists(string name, IGNQueriable queriable)
        {
            SetViewExists(name, queriable, ExistsEnum.Exists);
        }

        public override void IfViewNotExists(string name, IGNQueriable queriable)
        {
            SetViewExists(name, queriable, ExistsEnum.NotExists);
        }

        private void SetViewExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE [TABLE_CATALOG] = DB_NAME() AND [TABLE_NAME] = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfStoredProcedureExists(string name, IGNQueriable queriable)
        {
            SetStoresProcedureExists(name, queriable, ExistsEnum.Exists);
        }

        public override void IfStoredProcedureNotExists(string name, IGNQueriable queriable)
        {
            SetStoresProcedureExists(name, queriable, ExistsEnum.NotExists);
        }

        private void SetStoresProcedureExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE [ROUTINE_NAME] = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfIndexNotExists(string name, IGNQueriable queriable)
        {
            SetIndexExists(name, queriable, ExistsEnum.NotExists);
        }

        public override void IfIndexExists(string name, IGNQueriable queriable)
        {
            SetIndexExists(name, queriable, ExistsEnum.Exists);
        }

        private void SetIndexExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM sysindexes WHERE name='{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfColumnExists(string name, IGNQueriable queriable)
        {
            SetColumnExists(name, queriable, ExistsEnum.Exists);
        }

        public override void IfColumnNotExists(string name, IGNQueriable queriable)
        {
            SetColumnExists(name, queriable, ExistsEnum.NotExists);
        }

        private void SetColumnExists(string name, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_CATALOG] = DB_NAME() AND [TABLE_NAME] = '{queriable.TableName}' AND [COLUMN_NAME] = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }
    }
}
