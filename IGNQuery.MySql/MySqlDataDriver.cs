//###########################################################################
//    MySql Connector for IGNQuery library
//    Copyright(C) 2020 Oleg Golovchenko
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//###########################################################################
using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace IGNQuery.MySql
{
    public class MySqlDataDriver : IGNDbDriver
    {
        public MySqlDataDriver(string email, string connectionString, string key) : base(email, connectionString, key)
        {
            this.dialect = Enums.DialectEnum.MySQL;
        }

        public MySqlDataDriver(string email, string server, int port, string uName, string pwd, string key) : base(email, server, port, uName, pwd, key)
        {
            this.dialect = Enums.DialectEnum.MySQL;
        }

        public MySqlDataDriver(string email, string key) : base(email, key)
        {
            this.dialect = Enums.DialectEnum.MySQL;
        }

        protected override string ConstructDefaultConnectionString()
        {
            return Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        }

        protected override string ConstructConnectionString(string server, int port, string uName, string pwd)
        {
            return $"server={server}:{port};uid={uName};pwd={pwd};";
        }

        protected override DbConnection OpenConnection()
        {
            var connection = new MySqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }

        protected override DbCommand PrepareDbCommand(string query, DbConnection connection)
        {
            return new MySqlCommand(query)
            {
                Connection = (MySqlConnection)connection
            };
        }

        protected override void AddParameters(DbCommand dbc, IEnumerable<IGNParameterValue> args)
        {
            foreach (var arg in args)
            {
                ((MySqlCommand)dbc).Parameters.AddWithValue($"@p{arg.Position}", arg.Value);
            }
        }

        protected override DataTable InitDataTable(DbDataReader reader)
        {
            var result = new DataTable();
            result.Load(reader);
            return result;
        }

        public override string GetDbAutoGenFor(Type clrType, int length)
        {
            if (clrType == typeof(long) ||
                clrType == typeof(int) ||
                clrType == typeof(short) ||
                clrType == typeof(byte))
            {
                return " AUTO_INCREMENT";
            }
            if (clrType == typeof(Guid))
            {
                return " DEFAULT (uuid())";
            }
            if(clrType == typeof(DateTime))
            {
                return " DEFAULT (UTC_DATE())";
            }
            return "";
        }

        public override string GoTerminator()
        {
            return ";";
        }
    }
}