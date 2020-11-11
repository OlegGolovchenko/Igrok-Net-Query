using System.Collections.Generic;

namespace IGNQuery.Interfaces.QueryProvider
{
    public interface ISelectQuery : IQueryResult
    {
        IQueryResult AllFrom(string table);

        IConditionalQuery AllFromWithCondition(string table);

        IQueryResult FieldsFrom(string table, IEnumerable<string> fieldNames);

        IConditionalQuery FieldsFromWithCondition(string table, IEnumerable<string> fieldNames);
    }
}
