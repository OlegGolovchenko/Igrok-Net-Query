using System;
using System.Data.Common;
using System.Data.SqlClient;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using IGNQuery.Enums;

namespace IGNQuery.SqlServer
{
    public class MsSqlDataProvider : IDataProvider
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        public bool queryToOutput = false;

        public MsSqlDataProvider(string email)
        {
            //Activation.Activate(email);
            _connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
        }

        public void ExecuteNonQuery(IQueryResult query)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new SqlCommand(query.GetResultingString(), _connection);
            command.ExecuteNonQuery();
        }

        public void ExecuteNonQueryWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new SqlCommand(query.GetResultingString(), _connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
            }
            command.ExecuteNonQuery();
        }

        public void ExecuteStoredProcedure(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine($"calling stored procedure [{procname}]");
            }
            var command = new SqlCommand(procname, _connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
                }
            }
            command.ExecuteNonQuery();
        }

        public DbDataReader ExecuteReader(IQueryResult query)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new SqlCommand(query.GetResultingString(), _connection);
            DbDataReader result = command.ExecuteReader();
            return result;
        }

        public DbDataReader ExecuteReaderWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new SqlCommand(query.GetResultingString(), _connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
            }
            DbDataReader result = command.ExecuteReader();
            return result;
        }

        public DbDataReader ExecuteStoredProcedureReader(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            ResetConnection();
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine($"calling stored procedure [{procname}]");
            }
            var command = new SqlCommand(procname, _connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
                }
            }
            DbDataReader result = command.ExecuteReader();
            return result;
        }

        public IQuery Query()
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
            return new SqlQuery() { Dialect = DialectEnum.MSSQL };
        }

        public void ResetConnection()
        {
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
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
            //if (!Activation.IsActive)
            //{
            //    throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            //}
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
