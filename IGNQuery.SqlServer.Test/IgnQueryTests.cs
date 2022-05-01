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
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var paramList = new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,string.Empty),
                TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                TableColumnConfiguration.FromConfig("mail",typeof(string),254,true,false,false,string.Empty),
                TableColumnConfiguration.FromConfig("active",typeof(bool),0,true,false,false,true)
            };
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Create().
                Table("ignusers", true, paramList).
                Go();
            dataProvider.Execute(query);
        }

        [Test]
        public void ADeleteAllUsers()
        {
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Delete().
                From("ignusers", true).
                Go();

            dataProvider.Execute(query);
        }


        [Test]
        public void BAlterTableIfExists()
        {
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Alter("ignusers", true).
                AddColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, string.Empty), true).
                Add(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, string.Empty), true).
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Alter("ignusers", true).
                AlterColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 50, false, false, false, string.Empty), true).
                Go();
            dataProvider.Execute(query);
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Alter("ignusers", true).
                DropColumn("test1", true).
                Drop("test2", true).
                Go();
            dataProvider.Execute(query);
            var dataDriver = new MsSqlDataDriver("igntest@igrok-net.org");
            var column = TableColumnConfiguration.FromConfig("createdOn", typeof(DateTime), 0, false, true, false, string.Empty);
            var altquery = IGNQueriable.Begin("igntest@igrok-net.org", dataDriver).
                Alter("ignusers", true).
                AddColumn(column, true).
                Go();
            dataDriver.Execute(altquery);
        }

        [Test]
        public void BCreateUserIfNotExists()
        {
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
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
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Drop().
                StoredProcedure("testProc", true).
                Go();
            dataProvider.Execute(query);
            var spQuery = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Select().
                From("ignusers", true).
                Go();
            query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Create().
                StoredProcedure("testProc", true, spQuery, new List<IGNParameter>()).
                Go();

            dataProvider.Execute(query);
        }

        [Test]
        public void CDeleteTable()
        {
            var dataProvider = new MsSqlDataDriver("igntest@igrok-net.org");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dataProvider).
                Drop().
                Table("ignusers", true).
                Go();

            dataProvider.Execute(query);
        }
    }
}