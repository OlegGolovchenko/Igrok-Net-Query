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
using IGNQuery.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace IGNQuery.MySql
{
    public class MySqlDataDriver : IGNDbDriver
    {
        private readonly string email;

        public MySqlDataDriver(string email, string connectionString):base(connectionString)
        {
            this.email = email;
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
        }

        public MySqlDataDriver(string email, string server, int port, string uName, string pwd) : base(server, port, uName, pwd)
        {
            this.email = email;
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
        }

        public MySqlDataDriver(string email) : base()
        {
            this.email = email;
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
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

        public override void IfColumnExists(string name, string table, IGNQueriable queriable)
        {
            SetColumnExists(name, table, queriable, ExistsEnum.Exists);
        }

        public override void IfColumnNotExists(string name, string table, IGNQueriable queriable)
        {
            SetColumnExists(name, table, queriable, ExistsEnum.NotExists);
        }

        private void SetColumnExists(string name, string table, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`COLUMNS` WHERE `TABLE_SCHEMA` = database() AND `TABLE_NAME` = '{table}' AND `COLUMN_NAME` = '{name}'";
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
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`SHEMATA` WHERE `CATALOG_NAME`='{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }

        public override void IfIndexExists(string name, string table, IGNQueriable queriable)
        {
            SetIndexExists(name, table, queriable, ExistsEnum.Exists);
        }

        public override void IfIndexNotExists(string name, string table, IGNQueriable queriable)
        {
            SetIndexExists(name, table, queriable, ExistsEnum.NotExists);
        }

        private void SetIndexExists(string name, string table, IGNQueriable queriable, ExistsEnum existsFunc)
        {
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`STATISTICS` WHERE `TABLE_SCHEMA` = database() AND `TABLE_NAME` = '{table}' AND `INDEX_NAME` = '{name}'";
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
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`ROUTINES` WHERE `ROUTINE_SCHEMA` = database() AND `ROUTINE_NAME` = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
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
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`TABLES` WHERE `TABLE_SCHEMA` = database() AND `TABLE_NAME` = '{name}'";
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
            var checkQuery = $"SELECT * FROM `INFORMATION_SCHEMA`.`VIEWS` WHERE `TABLE_SCHEMA` = database() AND `TABLE_NAME` = '{name}'";
            var query = IGNQueriable.FromQueryString(checkQuery, this.email, this);
            var result = ReadDataWithParameters(query, new List<IGNParameterValue>());
            IGNQueriable.SetExists(result.Rows.Count > 0, queriable);
            IGNQueriable.SetCanExecute(existsFunc, queriable);
        }
    }
}