using IGNQuery.BaseClasses;
using IGNQuery.MySql;
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
            var mssqlconnectionString = "server=127.0.0.1;user=root;pwd=IgRok_100692;database=igroknettest";
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", mssqlconnectionString);
        }

        [Test]
        public void ACreateTableIfNotExists()
        {
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
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
                    DefValue = "true",
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
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                Delete().
                From("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                Alter().
                TableIfExists("ignusers").
                Add().
                Column(new TableField
                {
                    Name = "test1",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(25),
                    Generated = false,
                    DefValue = ""
                }).
                Next().
                Add().
                Column(new TableField
                {
                    Name = "test2",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(25),
                    Generated = false,
                    DefValue = ""
                }).
                Go();
            dataProvider.ExecuteNonQuery(query);
            query = dataProvider.Query().
                Alter().
                TableIfExists("ignusers").
                Alter().
                Column(new TableField
                {
                    Name = "test1",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(50),
                    Generated = false,
                    DefValue = ""
                }).
                Go();
            dataProvider.ExecuteNonQuery(query);
            query = dataProvider.Query().
                Alter().
                TableIfExists("ignusers").
                Drop().
                Column(new TableField
                {
                    Name = "test1",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(25),
                    Generated = false,
                    DefValue = ""
                }).
                Next().
                Drop().
                Column(new TableField
                {
                    Name = "test2",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(25),
                    Generated = false,
                    DefValue = ""
                }).
                Go();
            dataProvider.ExecuteNonQuery(query);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
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
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
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
            var dataProvider = new MySqlDataProvider("igrok_be@hotmail.com");
            var query = dataProvider.Query().
                Drop().
                TableIfExists("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }
    }
}