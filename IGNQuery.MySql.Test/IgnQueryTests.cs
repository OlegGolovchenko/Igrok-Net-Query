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
            var paramList = new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,string.Empty),
                TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                TableColumnConfiguration.FromConfig("mail",typeof(string),254,true,false,false,string.Empty),
                TableColumnConfiguration.FromConfig("active",typeof(bool),0,true,false,false,true)
            };
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Create().
                Table("ignusers", paramList).
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
                Column(TableColumnConfiguration.FromConfig("test1",typeof(string),25,false,false,false,string.Empty)).
                IfNotExists().
                Add().
                Column(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, string.Empty)).
                IfNotExists().
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Alter().
                Table("ignusers").
                IfExists().
                Alter().
                Column(TableColumnConfiguration.FromConfig("test1", typeof(string), 50, false, false, false, string.Empty)).
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
            var column = TableColumnConfiguration.FromConfig("createdOn", typeof(DateTime), 0, false, true, false, string.Empty);
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
                    Into("ignusers", new List<string>() { "mail", "userId" }).
                    IfExists().
                    ValuesWithParams(new List<int> { 0, 1 }).
                    Go();

            dataProvider.ExecuteWithParameters(query, new List<IGNParameterValue>
                {
                    IGNParameterValue.FromConfig(0, "igntest@igrok-net.org"),
                    IGNParameterValue.FromConfig(1, -1)
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