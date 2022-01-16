using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.MySql;
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
            var myqlconnectionString = "server=127.0.0.1;user=testusr;pwd=IgRok-NET_t35t_u5r;database=igroknettest";
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", myqlconnectionString);
        }

        [Test]
        public void ACreateTableIfNotExists()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
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
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Create().
                Table("ignusers", paramList.Select(x => TableColumnConfiguration.FromTableField(x))).
                IfNotExists().
                Go();
            dataProvider.Execute(query);
        }

        [Test]
        public void ADeleteAllUsers()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Delete().
                From("ignusers").
                Go();

            dataProvider.Execute(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
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
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
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
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Alter().
                Table("ignusers").
                IfExists().
                Drop("test1").
                IfExists().
                Drop("test2").
                IfExists().
                Go();
            dataProvider.Execute(query);
            var dataDriver = new MySqlDataDriver("igntest@igrok-net.org");
            var column = new TableField
            {
                Name = "createdOn",
                CanHaveNull = true,
                Primary = false,
                Generated = true,
                DefValue = "",
                Type = TableField.TYPE_DATE
            };
            var altquery = IGNQueriable.Begin("igntest@igrok-net.org", dataDriver).
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
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                    Insert().
                    Into("ignusers", new List<string>() { "mail" }).
                    IfExists().
                    ValuesWithParams(new List<int> { 0 }).
                    Go();

            dataProvider.ExecuteWithParameters(query, new List<IGNParameterValue>
                {
                    IGNParameterValue.FromConfig(0, "igntest@igrok-net.org")
                });
        }

        [Test]
        public void BCreateStoredProcedureIfNotExists()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Drop().
                StoredProcedure("testProc").
                IfExists().
                Go();
            dataProvider.Execute(query);
            var spQuery = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Select().
                From("ignusers").
                IfExists().
                Go();
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Create().
                StoredProcedure("testProc", spQuery).
                IfNotExists().
                Go();

            dataProvider.Execute(query);
        }

        [Test]
        public void CDeleteTable()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Drop().
                Table("ignusers").
                IfExists().
                Go();

            dataProvider.Execute(query);
        }
    }
}