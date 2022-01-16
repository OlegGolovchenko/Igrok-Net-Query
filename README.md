# Igrok-Net-Query
Query building library based on ADO .NET functionality

Parts of project are licnesed under gplv3 others are under MIT please refer to source code and to license file in each project to see under which license it is licensed.
This project will be split into multiple nuget packages for convinience of use and to be able to use multiple licenses.

Base class library will be under MIT.
Database specific implementations will be licensed under separate licenses
 * MYSQL: GPLV3 to be compatible with community version and connector for mysql
 * Microsoft SQL server: MIT license

If you want to build dev version with prerelease package versions, please add bin\releae folder of your IGNQuery project to package sources.
Otherwise you may run into problems with package versions.

If you need to use this package it is advisable to use official nuget packages from NuGet.org

## How to use
Disclaimer:
We will not send you emails except for service emails (account activation, any resets or any emails that are resulting from actions you do on https://igrok-net.org), email is just used to identify if user have right to use this library.

### Create table query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
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
dataProvider.Execute(query);
```

### Create database query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
                Create().
                Database("testdb").
                IfNotExists().
                Go()
dataProvider.Execute(query);
```

### Alter query exemples:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
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
query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
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
query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Alter().
    Table("ignusers").
    IfExists().
    Drop("test1").
    IfExists().
    Drop("test2").
    IfExists().
    Go();
dataProvider.Execute(query);
var dataDriver = new MySqlDataDriver("youremail@domain.com");
var column = new TableField
{
    Name = "createdOn",
    CanHaveNull = true,
    Primary = false,
    Generated = true,
    DefValue = "",
    Type = TableField.TYPE_DATE
};
var altquery = IGNQueriable.Begin("youremail@domain.com", dataDriver).
    Alter().
    Table("ignusers").
    IfExists().
    Add().
    Column(column).
    IfNotExists().
    Go();
dataDriver.Execute(altquery);
```

### Delete query exemple:

``` csharp

var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Delete().
    From("ignusers").
    Go();

dataProvider.Execute(query);
```

### Insert query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
        Insert().
        Into("ignusers", new List<string>() { "mail" }).
        IfExists().
        ValuesWithParams(new List<int> { 0 }).
        Go();

dataProvider.ExecuteWithParameters(query, new List<IGNParameterValue>
    {
        IGNParameterValue.FromConfig(0, "youremail@domain.com")
    });
```

### Drop query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Drop().
    Table("ignusers").
    IfExists().
    Go();

dataProvider.Execute(query);
```