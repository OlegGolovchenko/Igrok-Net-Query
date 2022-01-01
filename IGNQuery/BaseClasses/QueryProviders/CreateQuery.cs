//#############################################
//
//  MIT License
//
//  Copyright(c) 2020 Oleg Golovchenko
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.  
//
// ############################################

using IGNQuery.BaseClasses.Business;
using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class CreateQuery : QueryResult ,ICreateQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        private string table = "";

        internal CreateQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IExistenceCheckQuery Database(string name)
        {
            this.name = name;
            this.objectType = IGNDbObjectTypeEnum.Database;
            queriable.AddOperation("CREATE DATABASE", queriable.SanitizeName(name),"");
            return this;
        }

        public IExistenceCheckQuery StoredProcedure(string name, IGNQueriable content, [Optional] IEnumerable<IGNParameter> parameters)
        {
            this.name = name;
            this.objectType = IGNDbObjectTypeEnum.StoredProcedure;
            queriable.AddOperation("CREATE PROCEDURE", queriable.SanitizeName(name), "");
            queriable.AddOperation(queriable.FormatSpSubqueryAndParams(parameters ?? new List<IGNParameter>(), content), "", "");
            return this;
        }

        public IExistenceCheckQuery Table(string name, IEnumerable<TableColumnConfiguration> fields)
        {
            this.name = name;
            this.objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("CREATE TABLE", queriable.SanitizeName(name),"");
            queriable.AddOperation($"({string.Join(",", queriable.CompileFieldsInfo(name, fields))})","","");
            return this;
        }

        public IExistenceCheckQuery View(string name, IGNQueriable content)
        {
            this.name = name;
            objectType = IGNDbObjectTypeEnum.View;
            queriable.AddOperation("CREATE VIEW", queriable.SanitizeName(name), "");
            queriable.AddOperation("AS", "", "\n");
            queriable.AddOperation($"{content}", "", "\n");
            return this;
        }

        public IExistenceCheckQuery Index(string name, string tableName, IEnumerable<string> columns, bool unique)
        {
            table = tableName;
            this.name = name;
            objectType = unique ? IGNDbObjectTypeEnum.UniqueIndex : IGNDbObjectTypeEnum.Index;
            string operand = unique ? "CREATE INDEX" : "CREATE UNIQUE INDEX";
            queriable.AddOperation(operand, queriable.SanitizeName(name), "");
            queriable.AddOperation("ON", tableName," ");
            queriable.AddOperation($"({string.Join(",", columns.Select(x => queriable.SanitizeName(x)))})", "", "");
            return this;
        }

        public IQueryResult IfExists()
        {
            throw new NotImplementedException("IfExists check is not relevant for create query");
        }

        public IQueryResult IfNotExists()
        {
            queriable.IfNotExists(objectType, name, table);
            return this;
        }
    }
}
