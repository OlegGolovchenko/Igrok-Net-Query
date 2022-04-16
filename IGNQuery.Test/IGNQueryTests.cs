using IGNQuery.BaseClasses;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace IGNQuery.Test
{
    public class IGNQueryTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void UseQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).Use("testauth").Go();
            var expected = "USE [testauth] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void AddColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter().
                Table("test").
                IfExists().
                Add().
                Column(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, "")).
                IfNotExists().
                Add().
                Column(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, "")).
                IfNotExists().
                Go();
            var expected = "ALTER TABLE [test] ADD  [test1] NVARCHAR(25) NULL,   [test2] NVARCHAR(25) NULL \nGO";
            Assert.AreEqual(expected, query.ToString());
        }


        [Test]
        public void AlterDropColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter().
                Table("test").
                IfExists().
                Drop("test1").
                IfExists().
                Drop("test2").
                IfExists().
                Go();
            var expected = "ALTER TABLE [test] DROP COLUMN [test1],  [test2] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void AlterColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter().
                Table("test").
                IfExists().
                Alter().
                Column(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, "")).
                IfExists().
                Go();
            var expected = "ALTER TABLE [test] ALTER COLUMN [test1] NVARCHAR(25) NULL \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateDatabaseQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                Database("testdb").
                IfNotExists().
                Go();
            var expected = "CREATE DATABASE [testdb] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateDatabaseIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                Database("testdb").
                IfNotExists().
                Go();
            var expected = "CREATE DATABASE [testdb] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateTableQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.GetDbAutoGenFor(It.IsAny<Type>(), It.IsAny<int>())).Returns(" IDENTITY(1,1)");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).Create().Table("test", new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null)
            }).IfNotExists().Go();
            var expected = "CREATE TABLE [test]([id] BIGINT NOT NULL IDENTITY(1,1),[name] NVARCHAR(255) NULL,CONSTRAINT PK_test PRIMARY KEY([id]))  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        //[Test]
        //public void CreateStoredProcedureQueryShouldGiveCorrectSyntax()
        //{
        //    var dbDriverMock = new Mock<IDataDriver>();
        //    dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
        //    dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
        //    var subquery = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
        //       Select().
        //       From("test").
        //       IfExists().
        //       WithCondition().
        //       Where(IGNConditionWithParameter.FromConfig("name", Enums.IGNSqlCondition.Eq, 0)).
        //       Go();
        //    var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
        //        Create().
        //        StoredProcedure("sp_test", subquery, new List<IGNParameter>(){
        //            IGNParameter.FromConfig(0,typeof(string),255)
        //        }).
        //        Go();
        //    var expected = "CREATE PROCEDURE [sp_test] @p0 NVARCHAR(255)\nAS\nSELECT * FROM [test] WHERE [name] = @p0 \nGO  \nGO";
        //    Assert.AreEqual(expected, query.ToString());
        //}

        //[Test]
        //public void CreateStoredProcedureIfNotExistsQueryShouldGiveCorrectSyntax()
        //{
        //    var dbDriverMock = new Mock<IDataDriver>();
        //    dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
        //    dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
        //    var subquery = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
        //        Select().
        //        From("test").
        //        IfExists().
        //        WithCondition().
        //        Where(IGNConditionWithParameter.FromConfig("name", Enums.IGNSqlCondition.Eq, 0)).
        //        Go();
        //    var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
        //        Create().
        //        StoredProcedure("sp_test", subquery, new List<IGNParameter>(){
        //            IGNParameter.FromConfig(0,typeof(string),255)
        //        }).
        //        IfNotExists().
        //        Go();
        //    var expected = "CREATE PROCEDURE [sp_test] @p0 NVARCHAR(255)\nAS\nSELECT * FROM [test] WHERE [name] = @p0 \nGO  \nGO";
        //    Assert.AreEqual(expected, query.ToString());
        //}


        [Test]
        public void CreateViewQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                View("dm_test", IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'", "igntest@igrok-net.org", dbDriverMock.Object)).
                IfNotExists().
                Go();
            var expected = "CREATE VIEW [dm_test]\nAS \nSELECT * FROM test WHERE name = 'test'  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateViewIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                View("dm_test", IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'", "igntest@igrok-net.org", dbDriverMock.Object)).
                IfNotExists().
                Go();
            var expected = "CREATE VIEW [dm_test]\nAS \nSELECT * FROM test WHERE name = 'test'  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateTableIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.GetDbAutoGenFor(It.IsAny<Type>(), It.IsAny<int>())).Returns(" IDENTITY(1,1)");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                Table("test", new List<TableColumnConfiguration>()
                {
                    TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                    TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                    TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null),
                    TableColumnConfiguration.FromConfig("testDate",typeof(DateTime),0,false,false,false,null)
                }).
                IfNotExists().
                Go();
            var expected = "CREATE TABLE [test]([id] BIGINT NOT NULL IDENTITY(1,1),[userId] BIGINT NOT NULL,[name] NVARCHAR(255) NULL,[testDate] DATETIME NULL,CONSTRAINT PK_test PRIMARY KEY([id]))  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void SelectQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Select(new List<string> { "test.id", "test2.test", "test.test" }).
                From("test").
                IfExists().
                Join("test", "test2", "test2id", "id", true).
                Go();
            var expected = "SELECT [test].[id],[test2].[test],[test].[test] FROM [test] INNER JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }


        [Test]
        public void SelectDistinctQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Select(new List<string> { "test.id", "test2.test", "test.test" }, true).
                From("test").
                IfExists().
                Join("test", "test2", "test2id", "id", true).
                Go();
            var expected = "SELECT DISTINCT [test].[id],[test2].[test],[test].[test] FROM [test] INNER JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        //[Test]
        //public void UpdateQueryShouldHaveCorrectSyntax()
        //{
        //    var dbDriverMock = new Mock<IDataDriver>();
        //    dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
        //    dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
        //    var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
        //        Update().
        //        Table("users").
        //        IfExists().
        //        SetParametrizedWithCondition("loggedInDateTime",0).
        //        Where(IGNConditionWithParameter.FromConfig("Id",Enums.IGNSqlCondition.Eq,1)).
        //        Go();
        //    var expected = "UPDATE [users] SET [loggedInDateTime] = @p0 WHERE [Id] = @p1 \nGO";
        //    Assert.AreEqual(expected , query.ToString());
        //}

        [Test]
        public void DropQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                Database("testdb").
                IfExists().
                Go();
            var expected = "DROP DATABASE [testdb] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DeleteQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Delete().
                From("test").
                IfExists().
                Go();
            var expected = "DELETE FROM [test] \nGO";
            Assert.AreEqual(expected,query.ToString());
        }

        [Test]
        public void DeleteQueryWithConditionShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Delete().
                From("test").
                IfExists().
                Where(IGNConditionWithParameter.FromConfig("tc",Enums.IGNSqlCondition.Eq,0)).
                Go();
            var expected = "DELETE FROM [test] WHERE [tc] = @p0 \nGO";
            Assert.AreEqual(expected, query.ToString());
        }
    }
}