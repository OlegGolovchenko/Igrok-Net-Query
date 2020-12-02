//###########################################################################
//    MySql Connector for IGNQuery library
//    Copyright(C) 2020 Oleg Golovchenko
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
//###########################################################################
using System;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using IGNQuery.Enums;
using MySql.Data.MySqlClient;

namespace IGNQuery.MySql
{
    public class MySqlDataProvider : IDataProvider
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;
        private ILogger _logger;

        public bool queryToOutput = false;

        public MySqlDataProvider(ILogger logger, string email)
        {
            Activation.Activate(email);
            _logger = logger;
            _connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        }

        public void ExecuteNonQuery(IQueryResult query)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new MySqlCommand(query.GetResultingString(), _connection);
            command.ExecuteNonQuery();
        }

        public void ExecuteNonQueryWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new MySqlCommand(query.GetResultingString(), _connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
            }
            command.ExecuteNonQuery();
        }

        public void ExecuteStoredProcedure(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine($"calling stored procedure [{procname}]");
            }
            var command = new MySqlCommand(procname, _connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
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
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new MySqlCommand(query.GetResultingString(), _connection);
            DbDataReader result = command.ExecuteReader();
            return result;
        }

        public DbDataReader ExecuteReaderWithParams(IQueryResult query, IEnumerable<ParameterValue> parameters)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine(query.GetResultingString());
            }
            var command = new MySqlCommand(query.GetResultingString(), _connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue($"@p{param.ParamNumber}", param.ParamValue);
            }
            DbDataReader result = command.ExecuteReader();
            return result;
        }

        public DbDataReader ExecuteStoredProcedureReader(string procname, IEnumerable<ParameterValue> parameters = null)
        {
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            ResetConnection();
            if (_connection == null)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            if (queryToOutput)
            {
                Console.WriteLine($"calling stored procedure [{procname}]");
            }
            var command = new MySqlCommand(procname, _connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
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
            if (!Activation.IsActive)
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
            return new SqlQuery() { Dialect = DialectEnum.MSSQL };
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

            }
            finally
            {

                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
