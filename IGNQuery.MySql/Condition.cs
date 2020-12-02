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
    internal class Condition : ICondition
    {
        private string _query;

        public Condition(string query)
        {
            _query = query;
        }

        public ICondition And()
        {
            _query += "AND ";
            return new Condition(_query);
        }

        public ICondition BoolEqual(bool value)
        {
            _query += $"= {(value ? 1 : 0)} ";
            return new Condition(_query);
        }

        public ICondition BoolEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }

        public ICondition BoolNotEqual(bool value)
        {
            _query += $"!= {(value ? 1 : 0)} ";
            return new Condition(_query);
        }

        public ICondition BoolNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }

        public ICondition Field(string name)
        {
            _query += $"{name} ";
            return new Condition(_query);
        }

        public IQueryResult Go()
        {
            _query += ";";
            return new QueryResult(_query);
        }

        public ICondition LongEqual(long value)
        {
            _query += $"= {value} ";
            return new Condition(_query);
        }

        public ICondition LongEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }

        public ICondition LongNotEqual(long value)
        {
            _query += $"!= {value} ";
            return new Condition(_query);
        }

        public ICondition LongNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }

        public ICondition Or()
        {
            _query += "OR ";
            return new Condition(_query);
        }

        public ICondition StringEqual(string value)
        {
            _query += $"= '{value}' ";
            return new Condition(_query);
        }

        public ICondition StringEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }

        public ICondition StringNotEqual(string value)
        {
            _query += $"!= '{value}' ";
            return new Condition(_query);
        }

        public ICondition StringNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query);
        }
    }
}