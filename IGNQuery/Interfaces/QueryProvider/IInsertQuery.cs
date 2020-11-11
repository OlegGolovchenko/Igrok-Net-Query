using System.Collections.Generic;

namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IInsertQuery : IQueryResult
    {
        IInsertQuery Into(string table, IEnumerable<string> fields);

        IInsertQuery Values();

        IInsertQuery AddRow(IEnumerable<FieldValue> values);

        IInsertQuery AddRowWithParams(IEnumerable<int> paramNumbers);

        IInsertQuery Next();
    }
}
