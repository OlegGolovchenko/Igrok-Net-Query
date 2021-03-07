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

If you need to use this package it is adwisable to use official nuget packages from NuGet.org