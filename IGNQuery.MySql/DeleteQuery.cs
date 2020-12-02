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

namespace IGNQuery.MySql
{
    internal class DeleteQuery : IDeleteQuery
    {
        private string _query;

        public DeleteQuery(string query)
        {
            _query = query;
        }

        public IQueryResult From(string table)
        {
            _query += $"DELETE FROM {table};";
            return new QueryResult(_query);
        }

        public IConditionalQuery FromWithCondition(string table)
        {
            _query += $"DELETE FROM {table} ";
            return new ConditionalQuery(_query);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}