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

using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;

namespace IGNQuery.BaseClasses.QueryProviders
{

    public class DropQuery : QueryResult, IDropQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        private string table = "";

        public DropQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IExistenceCheckQuery Database(string name)
        {
            this.name = name;
            objectType = IGNDbObjectTypeEnum.Database;
            queriable.AddOperation("DROP DATABASE", queriable.SanitizeName(name), "");
            return this;
        }

        public IQueryResult IfExists()
        {
            queriable.IfExists(objectType, name, table);
            return this;
        }

        public IQueryResult IfNotExists()
        {
            throw new NotImplementedException("IfNotExists check is not relevant for drop query");
        }

        public IExistenceCheckQuery Index(string name, string table, bool unique)
        {
            this.name = name;
            this.table = table;
            objectType = unique?IGNDbObjectTypeEnum.UniqueIndex:IGNDbObjectTypeEnum.Index;
            queriable.AddOperation("DROP INDEX", queriable.SanitizeName(name), "");
            return this;
        }

        public IExistenceCheckQuery StoredProcedure(string name)
        {
            this.name = name;
            objectType = IGNDbObjectTypeEnum.StoredProcedure;
            queriable.AddOperation("DROP PROCEDURE", queriable.SanitizeName(name), "");
            return this;
        }

        public IExistenceCheckQuery Table(string name)
        {
            this.name = name;
            objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("DROP TABLE", queriable.SanitizeName(name), "");
            return this;
        }

        public IExistenceCheckQuery View(string name)
        {
            this.name = name;
            objectType = IGNDbObjectTypeEnum.View;
            queriable.AddOperation("DROP VIEW", queriable.SanitizeName(name), "");
            return this;
        }
    }
}
