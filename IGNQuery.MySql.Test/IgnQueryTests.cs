using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.QueryProviders;
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
            var myqlconnectionString = "server=127.0.0.1;user=testusr;pwd=IgRok-NET_t35t_u5r;database=igroknettest";
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", myqlconnectionString);
        }

        [Test]
        public void ACreateTableIfNotExists()
        {
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
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
                    Name = "userId",
                    Type = TableField.TYPE_LONG,
                    CanHaveNull = false,
                    Generated = false,
                    DefValue = string.Empty,
                    Primary = false
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
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
            var query = dataProvider.Query().
                Delete().
                From("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
            var query = dataProvider.Query().
                Alter().
                TableIfExists("ignusers").
                Add().
                ColumnIfNotExists(new TableField
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
                ColumnIfNotExists(new TableField
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
                ColumnIfExists(new TableField
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
                ColumnIfExists(new TableField
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
                ColumnIfExists(new TableField
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
            var dataDriver = new MySqlDataDriver("igrok_be@hotmail.com");
            var addQuery = IGNQueriable.Begin("igrok_be@hotmail.com", dataDriver).
                Add().
                Column("createdOn", typeof(DateTime), 0, true, true);
            var altquery = IGNQueriable.Begin("igrok_be@hotmail.com", dataDriver).
                Alter().
                Table("ignusers", addQuery).
                IfExists();
            dataDriver.Execute(altquery);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
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
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
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
            var dataProvider = new MySqlDataProvider("igntest@igrok-net.org");
            var query = dataProvider.Query().
                Drop().
                TableIfExists("ignusers");

            dataProvider.ExecuteNonQuery(query);
        }
    }
}