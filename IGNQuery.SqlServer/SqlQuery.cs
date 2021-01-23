using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;

namespace IGNQuery.SqlServer
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
                throw new InvalidOperationException("Please use IGNQuery.MySQL to use mysql support");
            }
            else if(Dialect == DialectEnum.MSSQL)
            {
                return new MSSQLCreateQuery(_query);
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
                throw new InvalidOperationException("Please use IGNQuery.MySQL to use mysql support");
            }
            else if (Dialect == DialectEnum.MSSQL)
            {
                return new MsSqlAlterQuery(_query);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
