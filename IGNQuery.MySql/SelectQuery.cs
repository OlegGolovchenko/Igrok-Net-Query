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
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;

namespace IGNQuery.MySql
{
    internal class SelectQuery : ISelectQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public SelectQuery(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IQueryResult AllFrom(string table)
        {
            _query += $"SELECT * FROM {table};";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery AllFromWithCondition(string table)
        {
            _query += $"SELECT * FROM {table} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }

        public IQueryResult FieldsFrom(string table, IEnumerable<string> fieldNames)
        {
            _query += $"SELECT {string.Join(",", fieldNames)} FROM {table};";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery FieldsFromWithCondition(string table, IEnumerable<string> fieldNames)
        {
            _query += $"SELECT {string.Join(",", fieldNames)} FROM {table} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}