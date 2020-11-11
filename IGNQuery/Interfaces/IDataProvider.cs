using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace IGNQuery.Interfaces
{
    public interface IDataProvider: IDisposable
    {
        IQuery Query();
        DbDataReader ExecuteReader(IQueryResult query);
        DbDataReader ExecuteReaderWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters);
        DbDataReader ExecuteStoredProcedureReader(string procname, [Optional] IEnumerable<ParameterValue> parameters);
        void ExecuteNonQuery(IQueryResult query);
        void ExecuteNonQueryWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters);
        void ExecuteStoredProcedure(string procname, [Optional] IEnumerable<ParameterValue> parameters);
        void ResetConnection();
    }
}
