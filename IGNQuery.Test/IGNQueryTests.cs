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
                Alter("test",true).
                AddColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""),true).
                Add(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, ""),true).
                Go();
            var expected = "ALTER TABLE [test] ADD [test1] NVARCHAR(25) NULL, [test2] NVARCHAR(25) NULL \nGO";
            Assert.AreEqual(expected, query.ToString());
        }


        [Test]
        public void AlterDropColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter("test",true).
                DropColumn("test1",true).
                Drop("test2",true).
                Go();
            var expected = "ALTER TABLE [test] DROP COLUMN [test1], [test2] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void AlterColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter("test", true).
                AlterColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""),true).
                Go();
            var expected = "ALTER TABLE [test] ALTER COLUMN [test1] NVARCHAR(25) NULL \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void AlterColumnsQueryShouldGiveCorrectSyntaxForMySql()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MySQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns(";");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Alter("test", true).
                AlterColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""), true).
                Go();
            var expected = "ALTER TABLE `test` MODIFY COLUMN `test1` NVARCHAR(25) NULL ;";
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
                Database("testdb",true).
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
                Database("testdb", true).
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
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                Table("test", true, new List<TableColumnConfiguration>()
                {
                    TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                    TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null)
                }).
                Go();
            var expected = "CREATE TABLE [test]([id] BIGINT NOT NULL IDENTITY(1,1),[name] NVARCHAR(255) NULL,CONSTRAINT PK_test PRIMARY KEY([id]))  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateStoredProcedureQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var subquery = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
               Select().
               ConditionalFrom("test", true).
               Where(IGNConditionWithParameter.FromConfig("name", Enums.IGNSqlCondition.Eq, 0)).
               Go();
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                StoredProcedure("sp_test", true, subquery, new List<IGNParameter>(){
                    IGNParameter.FromConfig(0,typeof(string),255)
                }).
                Go();
            var expected = "CREATE PROCEDURE [sp_test] @p0 NVARCHAR(255)\nAS\nSELECT * FROM [test] WHERE [name] = @p0 \nGO  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateStoredProcedureIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var subquery = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Select().
                ConditionalFrom("test", true).
                Where(IGNConditionWithParameter.FromConfig("name", Enums.IGNSqlCondition.Eq, 0)).
                Go();
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                StoredProcedure("sp_test", true, subquery, new List<IGNParameter>(){
                    IGNParameter.FromConfig(0,typeof(string),255)
                }).
                Go();
            var expected = "CREATE PROCEDURE [sp_test] @p0 NVARCHAR(255)\nAS\nSELECT * FROM [test] WHERE [name] = @p0 \nGO  \nGO";
            Assert.AreEqual(expected, query.ToString());
        }


        [Test]
        public void CreateViewQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Create().
                View("dm_test", true, IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'", "igntest@igrok-net.org", dbDriverMock.Object)).
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
                View("dm_test", true, IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'", "igntest@igrok-net.org", dbDriverMock.Object)).
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
                Table("test", true, new List<TableColumnConfiguration>()
                {
                    TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                    TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                    TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null),
                    TableColumnConfiguration.FromConfig("testDate",typeof(DateTime),0,false,false,false,null)
                }).
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
                JoinableFrom("test", true).
                InnerJoin("test2", true).
                On("test2id", "id", true).
                Go();
            var expected = "SELECT [test].[id],[test2].[test],[test].[test] FROM [test] INNER JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void SelectQueryWithLeftJoinShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Select(new List<string> { "test.id", "test2.test", "test.test" }).
                JoinableFrom("test", true).
                LeftJoin("test2", true).
                On("test2id", "id",true).
                Go();
            var expected = "SELECT [test].[id],[test2].[test],[test].[test] FROM [test] LEFT JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void SelectQueryWithRightJoinShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Select(new List<string> { "test.id", "test2.test", "test.test" }).
                JoinableFrom("test", true).
                RightJoin("test2", true).
                On("test2id", "id", true).
                Go();
            var expected = "SELECT [test].[id],[test2].[test],[test].[test] FROM [test] RIGHT JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
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
                JoinableFrom("test", true).
                InnerJoin("test2", true).
                On("test2id", "id", true).
                Go();
            var expected = "SELECT DISTINCT [test].[id],[test2].[test],[test].[test] FROM [test] INNER JOIN [test2] ON [test].[test2id] = [test2].[id] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void UpdateQueryWithConditionShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Update("users",true).
                Set("loggedInDateTime", 0, true).
                Where(IGNConditionWithParameter.FromConfig("Id", Enums.IGNSqlCondition.Eq, 1)).
                Go();
            var expected = "UPDATE [users] SET [loggedInDateTime] = @p0 WHERE [Id] = @p1 \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void UpdateQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Update("users", true).
                Set("loggedInDateTime", 0, true).
                Go();
            var expected = "UPDATE [users] SET [loggedInDateTime] = @p0 \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DropDatabaseQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                Database("testdb", true).
                Go();
            var expected = "DROP DATABASE [testdb] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DropIndexQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                Index("IX_test", "test", true).
                Go();
            var expected = "DROP INDEX [IX_test] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DropStoredProcedureQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                StoredProcedure("test_proc", true).
                Go();
            var expected = "DROP PROCEDURE [test_proc] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DropTableQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                Table("test", true).
                Go();
            var expected = "DROP TABLE [test] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DropViewQueryShouldHaveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Drop().
                View("vw_test", true).
                Go();
            var expected = "DROP VIEW [vw_test] \nGO";
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
                From("test", true).
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
                ConditionalFrom("test", true).
                Where(IGNConditionWithParameter.FromConfig("tc",Enums.IGNSqlCondition.Eq,0)).
                Go();
            var expected = "DELETE FROM [test] WHERE [tc] = @p0 \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void DeleteQueryWithJoinConditionShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Delete().
                JoinableFrom("test", true).
                InnerJoin("test2",true).
                On("test2Id","id",true).
                Where(IGNConditionWithParameter.FromConfig("tc", Enums.IGNSqlCondition.Eq, 0)).
                Go();
            var expected = "DELETE FROM [test] INNER JOIN [test2] ON [test].[test2Id] = [test2].[id] WHERE [tc] = @p0 \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void InsertQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Insert().
                Into("tempTest", new List<string> { "test1", "test2" }, true).
                Values(new List<int> { 0, 1 }).
                Go();
            var expected = "INSERT INTO [tempTest] ([test1],[test2]) VALUES (@p0,@p1) \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void InsertQueryShouldGiveCorrectSyntaxWithMultipleRows()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Insert().
                Into("tempTest", new List<string> { "test1", "test2" }, true).
                Values(new List<int> { 0, 1 }).
                Values(new List<int> { 2, 3 }).
                Go();
            var expected = "INSERT INTO [tempTest] ([test1],[test2]) VALUES (@p0,@p1), (@p2,@p3) \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void InsertSelectQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Insert().
                IntoSelect("tempTest", new List<string> { "test1", "test2" }, 
                           new List<string> { "test", "test2" }, false, true).
                From("test",true).
                Go();
            var expected = "INSERT INTO [tempTest] ([test1],[test2]) SELECT [test],[test2] FROM [test] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void InsertSelectDistinctQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igntest@igrok-net.org", dbDriverMock.Object).
                Insert().
                IntoSelect("tempTest", new List<string> { "test1", "test2" },
                           new List<string> { "test", "test2" }, true, true).
                From("test", true).
                Go();
            var expected = "INSERT INTO [tempTest] ([test1],[test2]) SELECT DISTINCT [test],[test2] FROM [test] \nGO";
            Assert.AreEqual(expected, query.ToString());
        }
    }
}