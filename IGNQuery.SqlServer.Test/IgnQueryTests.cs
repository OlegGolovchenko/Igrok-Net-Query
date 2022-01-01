using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
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
                    DefValue = "'true'",
                    Primary = false
                }
            };
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                                      Create().
                                      Table("ignusers", paramList.Select(x=> TableColumnConfiguration.FromTableField(x))).
                                      IfNotExists().
                                      Go();
            dataProvider.Execute(query);
        }

        [Test]
        public void ADeleteAllUsers()
        {
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Delete().
                From("ignusers").
                Go();

            dataProvider.Execute(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Alter().
                Table("ignusers").
                IfExists().
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
                IfNotExists().
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
                IfNotExists().
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Alter().
                Table("ignusers").
                IfExists().
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
                IfExists().
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Alter().
                Table("ignusers").
                IfExists().
                Drop("test1").
                IfExists().
                Drop("test2").
                IfExists().
                Go();
            dataProvider.Execute(query);
            var dataDriver = new MsSqlDataDriver("igrok_be@hotmail.com");
            var column = new TableField
            {
                Name = "createdOn",
                CanHaveNull = true,
                Primary = false,
                Generated = true,
                DefValue = "",
                Type = TableField.TYPE_DATE
            };
            var altquery = IGNQueriable.Begin("igrok_be@hotmail.com", dataDriver).
                Alter().
                Table("ignusers").
                IfExists().
                Add().
                Column(column).
                IfNotExists().
                Go();
            dataDriver.Execute(altquery);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                    Insert().
                    Into("ignusers", new List<string>() { "mail" }).
                    IfExists().
                    ValuesWithParams(new List<int> { 0 }).
                    Go();

            dataProvider.ExecuteWithParameters(query, new List<IGNParameterValue>
                {
                    IGNParameterValue.FromConfig(0, "igrok_be@hotmail.com")
                });
        }

        [Test]
        public void BCreateStoredProcedureIfNotExists()
        {
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Drop().
                StoredProcedure("testProc").
                IfExists().
                Go();
            dataProvider.Execute(query);
            var spQuery = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Select().
                From("ignusers").
                IfExists().
                Go();
            query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Create().
                StoredProcedure("testProc", spQuery).
                IfNotExists().
                Go();

            dataProvider.Execute(query);
        }

        [Test]
        public void CDeleteTable()
        {
            var dataProvider = new MsSqlDataDriver("igrok_be@hotmail.com");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dataProvider).
                Drop().
                Table("ignusers").
                IfExists().
                Go();

            dataProvider.Execute(query);
        }
    }
}