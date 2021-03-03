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
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace IGNQuery.MySql
{
    internal class MySqlDataDriver : IGNDbDriver
    {
             
        public MySqlDataDriver(string email, string connectionString):base(connectionString)
        {
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
        }

        public MySqlDataDriver(string email, string server, int port, string uName, string pwd) : base(server, port, uName, pwd)
        {
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
        }

        public MySqlDataDriver(string email) : base()
        {
            this.dialect = Enums.DialectEnum.MySQL;
            Activation.Activate(email);
        }

        protected override string ConstructDefaultConnectionString()
        {
            return $"server=localhost;uid=root;pwd=root;";
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
            return "";
        }

        public override string GoTerminator()
        {
            return ";";
        }

        public override void IfColumnExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfColumnNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }

        public override void IfDatabaseExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfDatabaseNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }

        public override void IfIndexExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfIndexNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }

        public override void IfStoredProcedureExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfStoredProcedureNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }

        public override void IfTableExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfTableNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }

        public override void IfViewExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF EXISTS", queriable);
        }

        public override void IfViewNotExists(string name, IGNQueriable queriable)
        {
            IGNQueriable.SetAfterObjectString(" IF NOT EXISTS", queriable);
        }
    }
}