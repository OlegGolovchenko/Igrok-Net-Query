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
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class Create : DropCreateSelector, ICreate
    {
        internal Create(IGNQueriable queriable) : base(queriable)
        {
            isDropQuery = false;
            sqloperand = "CREATE";
        }

    }

    internal class DropCreateSelector : QueryResult, IDropCreateSelector
    {
        internal bool isDropQuery;
        internal string sqloperand;
        internal DropCreateSelector(IGNQueriable queriable) : base(queriable)
        {
            isDropQuery = false;
        }

        public IQueryResult Database(string name, bool existsCheck)
        {
            queriable.AddOperation($"{sqloperand} DATABASE", queriable.SanitizeName(name), "");
            if (existsCheck)
            {
                if (isDropQuery)
                {
                    queriable.IfExists(Enums.IGNDbObjectTypeEnum.Database, name, string.Empty);
                }
                else
                {
                    queriable.IfNotExists(Enums.IGNDbObjectTypeEnum.Database, name, string.Empty);
                }
            }
            return this;
        }

        public IQueryResult Index(string name, string table, bool unique, bool existsCheck, IEnumerable<string> columns = null)
        {
            if (isDropQuery)
            {
                if (queriable.dataDriver.Dialect != Enums.DialectEnum.MySQL)
                {
                    queriable.AddOperation($"{sqloperand} {(unique && isDropQuery ? "UNIQUE INDEX" : "INDEX")}", queriable.SanitizeName(name), "");
                }
                else
                {
                    queriable.AddOperation($"ALTER TABLE", queriable.SanitizeName(table), "");
                    queriable.AddOperation("DROP INDEX", queriable.SanitizeName(name), "");
                }
            }
            else
            {
                queriable.AddOperation($"{sqloperand} {(unique && isDropQuery ? "UNIQUE INDEX" : "INDEX")}", queriable.SanitizeName(name), "");
                queriable.AddOperation("ON", table, " ");
                queriable.AddOperation($"({string.Join(",", columns.Select(x => queriable.SanitizeName(x)))})", "", "");
            }
            if (existsCheck)
            {
                if (isDropQuery)
                {
                    queriable.IfExists(Enums.IGNDbObjectTypeEnum.Index, name, table);
                }
                else
                {
                    queriable.IfNotExists(Enums.IGNDbObjectTypeEnum.Index, name, table);
                }
            }
            return this;
        }

        public IQueryResult StoredProcedure(string name, bool checkExists, IGNQueriable content = null, IEnumerable<IGNParameter> parameters = null)
        {
            queriable.AddOperation($"{sqloperand} PROCEDURE", queriable.SanitizeName(name), "");
            if (!isDropQuery)
            {
                queriable.AddOperation(queriable.FormatSpSubqueryAndParams(parameters ?? new List<IGNParameter>(), content), "", "");
            }
            if (checkExists)
            {
                if (isDropQuery)
                {
                    queriable.IfExists(Enums.IGNDbObjectTypeEnum.StoredProcedure, name, string.Empty);
                }
                else
                {
                    queriable.IfNotExists(Enums.IGNDbObjectTypeEnum.StoredProcedure, name, string.Empty);
                }
            }
            return this;
        }

        public IQueryResult Table(string name, bool existsCheck, IEnumerable<TableColumnConfiguration> fields = null)
        {
            queriable.AddOperation($"{sqloperand} TABLE", queriable.SanitizeName(name), "");
            if (!isDropQuery)
            {
                queriable.AddOperation($"({string.Join(",", queriable.CompileFieldsInfo(name, fields))})", "", "");
            }
            if (existsCheck)
            {
                if (isDropQuery)
                {
                    queriable.IfExists(Enums.IGNDbObjectTypeEnum.Table, name, string.Empty);
                }
                else
                {
                    queriable.IfNotExists(Enums.IGNDbObjectTypeEnum.Table, name, string.Empty);
                }
            }
            return this;
        }

        public IQueryResult View(string name, bool existsCheck, IGNQueriable content = null)
        {
            queriable.AddOperation($"{sqloperand} VIEW", queriable.SanitizeName(name), "");
            if (!isDropQuery)
            {
                queriable.AddOperation("AS", "", "\n");
                queriable.AddOperation($"{content}", "", "\n");
            }
            if (existsCheck)
            {
                if (isDropQuery)
                {
                    queriable.IfExists(Enums.IGNDbObjectTypeEnum.View, name, string.Empty);
                }
                else
                {
                    queriable.IfNotExists(Enums.IGNDbObjectTypeEnum.View, name, string.Empty);
                }
            }
            return this;
        }
    }
}
