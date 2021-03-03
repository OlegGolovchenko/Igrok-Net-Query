using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class Condition : ICondition
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public Condition(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public ICondition And()
        {
            _query += "AND ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition BoolEqual(bool value)
        {
            _query += $"= {(value ? 1 : 0)} ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition BoolEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition BoolNotEqual(bool value)
        {
            _query += $"!= {(value ? 1 : 0)} ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition BoolNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition Field(string name)
        {
            _query += $"{name} ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public IQueryResult Go()
        {
            _query += "";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public ICondition LongEqual(long value)
        {
            _query += $"= {value} ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition LongEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition LongNotEqual(long value)
        {
            _query += $"!= {value} ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition LongNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition Or()
        {
            _query += "OR ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition StringEqual(string value)
        {
            _query += $"= '{value}' ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition StringEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition StringNotEqual(string value)
        {
            _query += $"!= '{value}' ";
            return new Condition(_query, this.email, this.dataDriver);
        }

        public ICondition StringNotEqualToParam(int paramNb)
        {
            _query += $" = @p{paramNb}";
            return new Condition(_query, this.email, this.dataDriver);
        }
    }
}