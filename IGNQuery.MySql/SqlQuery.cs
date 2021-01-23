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
using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;

namespace IGNQuery.MySql
{
    internal class SqlQuery : IQuery
    {
        private readonly string _query;

        public DialectEnum Dialect { get; set; }

        public SqlQuery()
        {
            _query = string.Empty;
        }

        public ICreateQuery Create()
        {
            if (Dialect == DialectEnum.MySQL)
            {
                return new MySQLCreateQuery(_query);
            }
            else if(Dialect == DialectEnum.MSSQL)
            {
                throw new InvalidOperationException("Please use IGNQuery.SQLServer to use ms sql server support");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IDeleteQuery Delete()
        {
            return new DeleteQuery(_query);
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IInsertQuery Insert()
        {
            return new InsertQuery(_query);
        }

        public ISelectQuery Select()
        {
            return new SelectQuery(_query);
        }

        public IUpdateQuery Update()
        {
            return new UpdateQuery(_query);
        }

        public IDropQuery Drop()
        {
            return new DropQuery(_query);
        }

        public IAlterQuery Alter()
        {
            if (Dialect == DialectEnum.MySQL)
            {
                return new MySQLAlterQuery(_query);
            }
            else if (Dialect == DialectEnum.MSSQL)
            {
                throw new InvalidOperationException("Please use IGNQuery.SQLServer to use ms sql server support");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
