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
                Table("test", true, new List<TableColumnConfiguration>()
                {
                    TableColumnConfiguration.FromConfig("id",typeof(long),0,true,true,true,null),
                    TableColumnConfiguration.FromConfig("userId",typeof(long),0,true,false,false,null),
                    TableColumnConfiguration.FromConfig("name",typeof(string),255,false,false,false,null),
                    TableColumnConfiguration.FromConfig("testDate",typeof(DateTime),0,false,false,false,null)
                }).
                Go();
dataProvider.Execute(query);
```

### Create database query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
                Create().
                Database("testdb", true).
                Go()
dataProvider.Execute(query);
```

### Alter query exemples:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Alter("ignusers", true).
    AddColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""),true).
    AddColumn(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, ""),true).
    Go();
dataProvider.Execute(query);
query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Alter("ignusers", true).
    AlterColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""),true).
    Go();
dataProvider.Execute(query);
query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Alter("ignusers", true).
    DropColumn("test1", true).
    Drop("test2", true).
    Go();
dataProvider.Execute(query);
var dataDriver = new MySqlDataDriver("youremail@domain.com");
var altquery = IGNQueriable.Begin("youremail@domain.com", dataDriver).
    Alter("ignusers", true).
    AddColumn(TableColumnConfiguration.FromConfig("test1", typeof(string), 25, false, false, false, ""),true).
    Add(TableColumnConfiguration.FromConfig("test2", typeof(string), 25, false, false, false, ""),true).
    Go();
dataDriver.Execute(altquery);
```

### Delete query exemple:

``` csharp

var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
    Delete().
    From("ignusers", true).
    Go();

dataProvider.Execute(query);
```

### Insert query exemple:

``` csharp
var dataProvider = new MySqlDataDriver("youremail@domain.com");
var query = IGNQueriable.Begin("youremail@domain.com", dataProvider).
        Insert().
        Into("ignusers", new List<string>() { "mail" }, true).
        Values(new List<int> { 0 }).
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
    Table("ignusers", true).
    Go();

dataProvider.Execute(query);
```