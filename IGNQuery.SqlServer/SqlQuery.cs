using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Enums;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System;

namespace IGNQuery.SqlServer
{
    internal class SqlQuery : IQuery
    {
        private readonly string _query;

        public DialectEnum Dialect { get; set; }

        private readonly string email;
        private readonly IDataDriver dataDriver;

        public SqlQuery(string email, IDataDriver dataDriver)
        {
            _query = string.Empty;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public ICreateQuery Create()
        {
            if (Dialect == DialectEnum.MySQL)
            {
                throw new InvalidOperationException("Please use IGNQuery.MySQL to use mysql support");
            }
            else if(Dialect == DialectEnum.MSSQL)
            {
                return new CreateQuery(this.email,this.dataDriver);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IDeleteQuery Delete()
        {
            return new DeleteQuery(_query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IInsertQuery Insert()
        {
            return new InsertQuery(_query, this.email, this.dataDriver);
        }

        public ISelectQuery Select()
        {
            return new SelectQuery(_query, this.email, this.dataDriver);
        }

        public IUpdateQuery Update()
        {
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IDropQuery Drop()
        {
            return new DropQuery(this.email, this.dataDriver);
        }

        public IAlterQuery Alter()
        {
            if (Dialect == DialectEnum.MySQL)
            {
                throw new InvalidOperationException("Please use IGNQuery.MySQL to use mysql support");
            }
            else if (Dialect == DialectEnum.MSSQL)
            {
                return new AlterQuery(this.email, this.dataDriver);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }
    }
}
