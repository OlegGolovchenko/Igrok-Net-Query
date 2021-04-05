using System;
using System.Data.Common;
using System.Data.SqlClient;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using IGNQuery.Enums;
using System.Linq;
using System.Data;
using IGNQuery.BaseClasses.Business;

namespace IGNQuery.SqlServer
{
    public class MsSqlDataProvider : IDataProvider
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private readonly IDataDriver dataDriver;
        private readonly string email;
        public bool queryToOutput = false;

        public MsSqlDataProvider(string email)
        {
            Activation.Activate(email);
            _connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
            this.dataDriver = new MsSqlDataDriver(email, _connectionString);
            this.email = email;
        }

        public void ExecuteNonQuery(IQueryResult query)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            this.dataDriver.Execute(query.AsIgnQueriable());
        }

        public void ExecuteNonQueryWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            this.dataDriver.ExecuteWithParameters(query.AsIgnQueriable(), 
                parameters.Select(x=> IGNParameterValue.FromConfig(x.ParamNumber,x.ParamValue)));
        }

        public void ExecuteStoredProcedure(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            this.dataDriver.ExecuteStoredProcedure(procname,
                parameters.Select(x => IGNParameterValue.FromConfig(x.ParamNumber, x.ParamValue)));
        }

        public DataTable ExecuteReader(IQueryResult query)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            return this.dataDriver.ReadData(query.AsIgnQueriable());
        }

        public DataTable ExecuteReaderWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            return this.dataDriver.ReadDataWithParameters(query.AsIgnQueriable(),
                parameters.Select(x => IGNParameterValue.FromConfig(x.ParamNumber, x.ParamValue)));
        }

        public DataTable ExecuteStoredProcedureReader(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            return this.dataDriver.ReadDataFromStoredProcedure(procname,
                parameters.Select(x => IGNParameterValue.FromConfig(x.ParamNumber, x.ParamValue)));
        }

        public IQuery Query()
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            return new SqlQuery(this.email, this.dataDriver) { Dialect = DialectEnum.MSSQL };
        }

        public void ResetConnection()
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            try
            {
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _connection = null;
        }

        public void Dispose()
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            try
            {
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {

                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
