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
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).Use("testauth");
            var expected = "USE testauth\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void AddColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfTableExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='U')\nBEGIN\nEXEC('";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("')END", y);
            });
            var query = new AlterQuery("igrok_be@hotmail.com",dbDriverMock.Object).
                TableIfExists("test").
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
            var expected = "IF EXISTS (SELECT * FROM sysobjects WHERE name='test' AND xtype='U')\nBEGIN\nEXEC('\nALTER TABLE test\nADD  test1 NVARCHAR(25) NULL, test2 NVARCHAR(25) NULL\n')END\nGO";
            Assert.AreEqual(expected, query.GetResultingString());
        }


        [Test]
        public void AlterDropColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfTableExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='U')\nBEGIN\nEXEC('";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("')END", y);
            });
            var query = new AlterQuery("igrok_be@hotmail.com", dbDriverMock.Object).
                TableIfExists("test").
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
            var expected = "IF EXISTS (SELECT * FROM sysobjects WHERE name='test' AND xtype='U')\nBEGIN\nEXEC('\nALTER TABLE test\nDROP COLUMN test1 , test2 \n')END\nGO";
            Assert.AreEqual(expected, query.GetResultingString());
        }

        [Test]
        public void AlterColumnsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfTableExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='U')\nBEGIN\nEXEC('";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("')END", y);
            });
            var query = new AlterQuery("igrok_be@hotmail.com", dbDriverMock.Object).
                TableIfExists("test").
                Alter().
                Column(new TableField
                {
                    Name = "test1",
                    CanHaveNull = true,
                    Primary = false,
                    Type = TableField.TypeNvarchar(25),
                    Generated = false,
                    DefValue = ""
                }).
                Go();
            var expected = "IF EXISTS (SELECT * FROM sysobjects WHERE name='test' AND xtype='U')\nBEGIN\nEXEC('\nALTER TABLE test\nALTER COLUMN test1 NVARCHAR(25) NULL\n')END\nGO";
            Assert.AreEqual(expected, query.GetResultingString());
        }

        [Test]
        public void AlterColumnsQueryShouldFailOnCallingNext()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.Dialect).Returns(Enums.DialectEnum.MSSQL);
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfTableExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='U')\nBEGIN\nEXEC('";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("')END", y);
            });
            Assert.Throws<Exception>(()=> {
                var query = new AlterQuery("igrok_be@hotmail.com", dbDriverMock.Object).
                TableIfExists("test").
                Alter().
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
                Go();
                });
        }

        [Test]
        public void CreateDatabaseQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).Create().Database("testdb");
            var expected = "CREATE DATABASE testdb\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateDatabaseIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfDatabaseNotExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF NOT EXISTS (SELECT * FROM sysdatabases WHERE name='{x}')\nBEGIN";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("END", y);
            });
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).Create().Database("testdb").IfNotExists();
            var expected = "IF NOT EXISTS (SELECT * FROM sysdatabases WHERE name='testdb')\nBEGIN\nCREATE DATABASE testdb\nEND\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateTableQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.GetDbAutoGenFor(It.IsAny<Type>(),It.IsAny<int>())).Returns(" IDENTITY(1,1)");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com",dbDriverMock.Object).Create().Table("test", ()=> new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null)
            });
            var expected = "CREATE TABLE test(id BIGINT NOT NULL IDENTITY(1,1),name NVARCHAR(255) NULL,CONSTRAINT PK_test PRIMARY KEY(id))\nGO";
            Assert.AreEqual(expected,query.ToString());
        }

        [Test]
        public void CreateStoredProcedureQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).
                Create().
                StoredProcedure("sp_test",IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = @name","igrok_be@hotmail.com",dbDriverMock.Object),()=> new List<Tuple<string,Type,int>>(){
                    Tuple.Create("name",typeof(string),255)
                });
            var expected = "CREATE PROCEDURE sp_test @name NVARCHAR(255)\nAS\nSELECT * FROM test WHERE name = @name\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateStoredProcedureIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfStoredProcedureNotExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string,IGNQueriable>((x,y) =>
            {
                var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='P')\nBEGIN";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("END", y);
            });
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).
                Create().
                StoredProcedure("sp_test", IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = @name","igrok_be@hotmail.com",dbDriverMock.Object), () => new List<Tuple<string, Type, int>>(){
                    Tuple.Create("name",typeof(string),255)
                }).
                IfNotExists();
            var expected = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='sp_test' AND xtype='P')\nBEGIN\nCREATE PROCEDURE sp_test @name NVARCHAR(255)\nAS\nSELECT * FROM test WHERE name = @name\nEND\nGO";
            Assert.AreEqual(expected, query.ToString());
        }


        [Test]
        public void CreateViewQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).
                Create().
                View("dm_test", IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'","igrok_be@hotmail.com",dbDriverMock.Object));
            var expected = "CREATE VIEW dm_test\nAS\nSELECT * FROM test WHERE name = 'test'\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateViewIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.IfViewNotExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string, IGNQueriable>((x, y) =>
            {
                var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='V')\nBEGIN";
                IGNQueriable.PrefixWith(prefix, y);
                IGNQueriable.SuffixWith("END", y);
            });
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).
                Create().
                View("dm_test", IGNQueriable.FromQueryString("SELECT * FROM test WHERE name = 'test'","igrok_be@hotmail.com",dbDriverMock.Object)).
                IfNotExists();
            var expected = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='dm_test' AND xtype='V')\nBEGIN\nCREATE VIEW dm_test\nAS\nSELECT * FROM test WHERE name = 'test'\nEND\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void CreateTableIfNotExistsQueryShouldGiveCorrectSyntax()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.GetDbAutoGenFor(It.IsAny<Type>(), It.IsAny<int>())).Returns(" IDENTITY(1,1)");
            dbDriverMock.Setup(x => x.IfTableNotExists(It.IsAny<string>(), It.IsAny<IGNQueriable>())).Callback<string,IGNQueriable>((x,y) =>
               {
                   var prefix = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{x}' AND xtype='U')\nBEGIN";
                   IGNQueriable.PrefixWith(prefix, y);
                   IGNQueriable.SuffixWith("END", y);
               });
            var query = IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).Create().Table("test", () => new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null)
            }).IfNotExists();
            var expected = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='test' AND xtype='U')\nBEGIN\nCREATE TABLE test(id BIGINT NOT NULL IDENTITY(1,1),name NVARCHAR(255) NULL,CONSTRAINT PK_test PRIMARY KEY(id))\nEND\nGO";
            Assert.AreEqual(expected, query.ToString());
        }

        [Test]
        public void TableQueryShouldFailWithoutCreate()
        {
            var dbDriverMock = new Mock<IDataDriver>();
            dbDriverMock.Setup(x => x.GoTerminator()).Returns("\nGO");
            dbDriverMock.Setup(x => x.GetDbAutoGenFor(It.IsAny<Type>(), It.IsAny<int>())).Returns(" IDENTITY(1,1)");
            Assert.Throws<Exception>(()=>IGNQueriable.Begin("igrok_be@hotmail.com", dbDriverMock.Object).Table("test", () => new List<TableColumnConfiguration>()
            {
                TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null)
            }));            
        }
    }
}