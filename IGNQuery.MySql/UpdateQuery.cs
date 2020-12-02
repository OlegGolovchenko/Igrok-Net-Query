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
    internal class UpdateQuery : IUpdateQuery
    {
        private string _query;

        public UpdateQuery(string query)
        {
            _query = query;
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IQueryResult SetBooleanFieldFalse(string field)
        {
            _query += $"SET {field} = 0;";
            return new UpdateQuery(_query);
        }

        public IConditionalQuery SetBooleanFieldFalseWithCondition(string field)
        {
            _query += $"SET {field} = 0 ";
            return new ConditionalQuery(_query);
        }

        public IQueryResult SetBooleanFieldTrue(string field)
        {
            _query += $"SET {field} = 1;";
            return new UpdateQuery(_query);
        }

        public IConditionalQuery SetBooleanFieldTrueWithCondition(string field)
        {
            _query += $"SET {field} = 1 ";
            return new ConditionalQuery(_query);
        }

        public IConditionalQuery SetFieldWithConditionWithParam(string fieldName, int paramNb)
        {
            _query += $"SET {fieldName} = @p{paramNb}";
            return new ConditionalQuery(_query);
        }

        public IQueryResult SetFieldWithParam(string fieldName, int paramNb)
        {
            _query += $"SET {fieldName} = @p{paramNb};";
            return new UpdateQuery(_query);
        }

        public IQueryResult SetLongField(string field, long value)
        {
            _query += $"SET {field} = {value};";
            return new UpdateQuery(_query);
        }

        public IConditionalQuery SetLongFieldWithCondition(string field, long value)
        {
            _query += $"SET {field} = {value} ";
            return new ConditionalQuery(_query);
        }

        public IQueryResult SetStringField(string field, string value)
        {
            _query += $"SET {field} = '{value}';";
            return new UpdateQuery(_query);
        }

        public IConditionalQuery SetStringFieldWithCondition(string field, string value)
        {
            _query += $"SET {field} = '{value}' ";
            return new ConditionalQuery(_query);
        }

        public IUpdateQuery Table(string table)
        {
            _query += $"UPDATE {table} ";
            return new UpdateQuery(_query);
        }
    }
}