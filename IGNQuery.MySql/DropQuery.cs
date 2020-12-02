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
using IGNQuery.Interfaces.QueryProvider;
using System.Text;

namespace IGNQuery.MySql
{
    public class DropQuery : IDropQuery
    {
        private string _query;

        public DropQuery(string query)
        {
            _query = query;
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IQueryResult StoredProcedureIfExists(string name)
        {
            var query = new StringBuilder();
            query.Append("DROP PROCEDURE IF EXISTS ");
            query.Append(name);
            query.Append(";");
            _query += query.ToString();
            return new QueryResult(_query);
        }

        public IQueryResult TableIfExists(string name)
        {
            var query = new StringBuilder();
            query.Append("DROP TABLE IF EXISTS ");
            query.Append(name);
            query.Append(";");
            _query += query.ToString();
            return new QueryResult(_query);
        }
    }
}
