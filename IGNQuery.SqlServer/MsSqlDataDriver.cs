using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
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
        public MsSqlDataDriver(string email, string server, int port,string user,string password):base(server,port,user,password)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
        }

        public MsSqlDataDriver(string email) : base()
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
        }

        public MsSqlDataDriver(string email, string connectionString):base(connectionString)
        {
            this.dialect = Enums.DialectEnum.MSSQL;
            Activation.Activate(email);
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
            return $"Server=(localdb)\\MSSQLLocalDB;database=master;integrated security=true;";
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
            return "";
        }

        public override void IfTableNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='U')\nBEGIN\nEXEC('";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("')END", queriable);
        }

        public override void IfTableExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='U')\nBEGIN\nEXEC('";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("')END", queriable);
        }

        public override void IfDatabaseNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM sysdatabases WHERE name='{name}')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfDatabaseExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM sysdatabases WHERE name='{name}')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfViewNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='V')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfViewExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='V')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfStoredProcedureNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='P')\nBEGIN\nEXEC('";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("')END", queriable);
        }

        public override void IfStoredProcedureExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{name}' AND xtype='P')\nBEGIN\nEXEC('";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("')END", queriable);
        }

        public override void IfIndexNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM sysindexes WHERE name='{name}')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfIndexExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM sysindexes WHERE name='{name}')\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfColumnNotExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF NOT EXISTS (SELECT * FROM syscolumns WHERE name='{name}' " +
                $"and Object_ID = Object_ID(N'{name}'))\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }

        public override void IfColumnExists(string name, IGNQueriable queriable)
        {
            var prefix = $"IF EXISTS (SELECT * FROM syscolumns WHERE name='{name}' " +
                $"and Object_ID = Object_ID(N'{name}'))\nBEGIN";
            IGNQueriable.PrefixWith(prefix, queriable);
            IGNQueriable.SuffixWith("END", queriable);
        }
    }
}
