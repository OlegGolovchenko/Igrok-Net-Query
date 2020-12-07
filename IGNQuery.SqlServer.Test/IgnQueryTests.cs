using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace IGNQuery.SqlServer.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var mssqlconnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testauth;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            Environment.SetEnvironmentVariable("SQLSERVER_CONNECTION_STRING", mssqlconnectionString);
        }

        [Test]
        public void ACreateTableIfNotExists()
        {
            var dataProvider = new MsSqlDataProvider("igrok_be@hotmail.com");
            var paramList = new List<TableField>()
            {
                new TableField
                {
                    Name = "id",
                    Type = TableField.TYPE_LONG,
                    CanHaveNull = false,
                    Generated = true,
                    DefValue = string.Empty,
                    Primary = true
                },
                new TableField
                {
                    Name = "mail",
                    Type = TableField.TypeNvarchar(254),
                    CanHaveNull = false,
                    Generated = false,
                    DefValue = string.Empty,
                    Primary = false
                },
                new TableField
                {
                    Name = "active",
                    Type = TableField.TYPE_BOOLEAN,
                    CanHaveNull = false,
                    Generated = false,
                    DefValue = "'true'",
                    Primary = false
                }
            };
            var query = dataProvider.Query().
                                      Create().
                                      TableIfNotExists("ignusers", paramList);
            dataProvider.ExecuteNonQuery(query);
        }

        [Test]
        public void ADeleteAllUsers()
        {
            var dataProvider = new MsSqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                Delete().
                From("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MsSqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                    Insert().
                    Into("ignusers", new List<string>() { "mail" }).
                    Values().
                    AddRowWithParams(new List<int> { 0 });

            dataProvider.ExecuteNonQueryWithParams(query, new List<ParameterValue>
                {
                    new ParameterValue(0, "igrok_be@hotmail.com")
                });
        }

        [Test]
        public void BCreateStoredProcedureIfNotExists()
        {
            var dataProvider = new MsSqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().Drop().StoredProcedureIfExists("testProc");
            dataProvider.ExecuteNonQuery(query);
            var spQuery = dataProvider.Query().Select().AllFrom("ignusers");
            query = dataProvider.Query().
                Create().
                StoredProcedure("testProc", spQuery);

            dataProvider.ExecuteNonQuery(query);
        }

        [Test]
        public void CDeleteTable()
        {
            var dataProvider = new MsSqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                Drop().
                TableIfExists("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }
    }
}