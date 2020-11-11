using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IGNQuery.Interfaces.QueryProvider
{
    public interface ICreateQuery : IQueryResult
    {
        IQueryResult TableIfNotExists(string name, IEnumerable<TableField> fields);

        IQueryResult StoredProcedureIfNotExists(string name, IQueryResult content, [Optional] IEnumerable<TableField> parameters);
    }
}
