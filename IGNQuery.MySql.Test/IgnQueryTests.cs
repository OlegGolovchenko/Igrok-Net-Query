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
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var paramList = new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,string.Empty),
                TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                TableColumnConfiguration.FromConfig("mail",typeof(string),254,true,false,false,string.Empty),
                TableColumnConfiguration.FromConfig("active",typeof(bool),0,true,false,false,true)
            };
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Create().
                Table("ignusers", true, paramList).
                Go();
            dataProvider.Execute(query);
        }

        [Test]
        public void ADeleteAllUsers()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Delete().
                From("ignusers", true).
                Go();

            dataProvider.Execute(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Alter("ignusers", true).
                AddColumn(TableColumnConfiguration.FromConfig("test1",typeof(string),25,false,false,false, string.Empty), true).
                Add(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, string.Empty), true).
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Alter("ignusers", true).
                AlterColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 50, false, false, false, string.Empty), true).
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Alter("ignusers", true).
                DropColumn("test1", true).
                Drop("test2", true).
                Go();
            dataProvider.Execute(query);
            var dataDriver = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var column = TableColumnConfiguration.FromConfig("createdOn", typeof(DateTime), 0, false, true, false, string.Empty);
            var altquery = IGNQueriable.Begin("igntest@igrok-net.org", dataDriver, "08303-8981D-B1B8C-00007-5024D").
                Alter("ignusers", true).
                AddColumn(column, true).
                Go();
            dataDriver.Execute(altquery);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                    Insert().
                    Into("ignusers", new List<string>() { "mail", "userId" }, true).
                    Values(new List<int> { 0, 1 }).
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
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Drop().
                StoredProcedure("testProc", true).
                Go();
            dataProvider.Execute(query);
            var spQuery = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Select().
                From("ignusers", true).
                Go();
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Create().
                StoredProcedure("testProc", true, spQuery, new List<IGNParameter>{ }).
                Go();

            dataProvider.Execute(query);
        }

        [Test]
        public void CDeleteTable()
        {
            var dataProvider = new MySqlDataDriver("igntest@igrok-net.org", "08303-8981D-B1B8C-00007-5024D");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider, "08303-8981D-B1B8C-00007-5024D").
                Drop().
                Table("ignusers", true).
                Go();

            dataProvider.Execute(query);
        }
    }
}