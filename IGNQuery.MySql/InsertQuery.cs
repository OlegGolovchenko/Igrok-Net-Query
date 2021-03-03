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
using System.Globalization;
using System.Linq;

namespace IGNQuery.MySql
{
    internal class InsertQuery : IInsertQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public InsertQuery(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IInsertQuery AddRow(IEnumerable<FieldValue> values)
        {

            _query += $" ({string.Join(",", GetValuesAsString(values))}) ";
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        public IInsertQuery AddRowWithParams(IEnumerable<int> paramNumbers)
        {
            _query += $" ({string.Join(",", paramNumbers.Select(x=>$"@p{x}"))}) ";
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IInsertQuery Into(string table, IEnumerable<string> fields)
        {
            _query += $"INSERT INTO {table}({string.Join(",", fields)}) ";
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        public IInsertQuery Next()
        {
            _query += ",";
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        public IInsertQuery Values()
        {
            _query += "VALUES";
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        private IEnumerable<string> GetValuesAsString(IEnumerable<FieldValue> values)
        {
            var result = new List<string>();
            foreach(var value in values)
            {
                if (value.BooleanValue.HasValue)
                {
                    result.Add(value.BooleanValue.Value ? "1" : "0");
                }
                if (value.LongValue.HasValue)
                {
                    result.Add(value.LongValue.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrWhiteSpace(value.StringValue))
                {
                    result.Add($"'{value.StringValue}'");
                }
            }
            return result;
        }
    }
}