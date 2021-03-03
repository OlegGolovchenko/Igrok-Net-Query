using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IGNQuery.SqlServer
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
            return new InsertQuery(_query, this.email ,this.dataDriver);
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