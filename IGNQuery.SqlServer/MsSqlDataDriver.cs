using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace IGNQuery.SqlServer
{
    public class MsSqlDataDriver : IGNDbDriver
    {
        public MsSqlDataDriver(string email, string server, int port,string user,string password):base(email, server,port,user,password)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
        }

        public MsSqlDataDriver(string email) : base(email)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
        }

        public MsSqlDataDriver(string email, string connectionString):base(email, connectionString)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
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
    }
}
