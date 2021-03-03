using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class UpdateQuery : IUpdateQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public UpdateQuery(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IQueryResult SetBooleanFieldFalse(string field)
        {
            _query += $"SET {field} = 0 GO";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery SetBooleanFieldFalseWithCondition(string field)
        {
            _query += $"SET {field} = 0 ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IQueryResult SetBooleanFieldTrue(string field)
        {
            _query += $"SET {field} = 1 GO";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery SetBooleanFieldTrueWithCondition(string field)
        {
            _query += $"SET {field} = 1 ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery SetFieldWithConditionWithParam(string fieldName, int paramNb)
        {
            _query += $"SET {fieldName} = @p{paramNb}";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IQueryResult SetFieldWithParam(string fieldName, int paramNb)
        {
            _query += $"SET {fieldName} = @p{paramNb} GO";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IQueryResult SetLongField(string field, long value)
        {
            _query += $"SET {field} = {value} GO";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery SetLongFieldWithCondition(string field, long value)
        {
            _query += $"SET {field} = {value} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IQueryResult SetStringField(string field, string value)
        {
            _query += $"SET {field} = '{value}' GO";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery SetStringFieldWithCondition(string field, string value)
        {
            _query += $"SET {field} = '{value}' ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IUpdateQuery Table(string table)
        {
            _query += $"UPDATE {table} ";
            return new UpdateQuery(_query, this.email, this.dataDriver);
        }
    }
}