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

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class Target : GroupedJoinable, ITarget, IGroupableTarget
    {
        internal string operation;
        internal string[] fields;

        internal Target(IGNQueriable queriable) : base(queriable)
        {
        }

        public IConditional ConditionalFrom(string table, bool checkExists)
        {
            return ConditionalFromInto(table, string.Empty, checkExists, string.Empty);
        }

        public IConditional ConditionalFromInto(string table, string targetTable, bool checkExists, string databaseName = null)
        {
            return (IConditional)this.FromInto(table, targetTable, checkExists, databaseName);
        }

        public IQueryResult From(string table, bool checkExists)
        {
            return FromInto(table, string.Empty, checkExists, string.Empty);
        }

        public IQueryResult FromInto(string table, string targetTable, bool checkExists, string databaseName = null)
        {
            source = table;
            string fromQuery = $"{operation} FROM";
            if (!string.IsNullOrWhiteSpace(targetTable))
            {
                queriable.AddOperation($"{operation} INTO", queriable.SanitizeName(targetTable), "");
                if (checkExists)
                {
                    queriable.IfExists(IGNDbObjectTypeEnum.Table, targetTable, "");
                }
                if (!string.IsNullOrWhiteSpace(databaseName))
                {
                    queriable.AddOperation("IN", queriable.SanitizeName(databaseName), " ");
                    if (checkExists)
                    {
                        queriable.IfExists(IGNDbObjectTypeEnum.Database, databaseName, "");
                    }
                }
                fromQuery = " FROM";
            }
            queriable.AddOperation(fromQuery, queriable.SanitizeName(table), "");
            if (checkExists)
            {
                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        queriable.IfExists(IGNDbObjectTypeEnum.Column, field, table);
                    }
                }
                queriable.IfExists(IGNDbObjectTypeEnum.Table, table, "");
            }
            return this;
        }

        public IJoinable JoinableFrom(string table, bool checkExists)
        {
            return JoinableFromInto(table, string.Empty, checkExists, string.Empty);
        }

        public IJoinable JoinableFromInto(string table, string targetTable, bool checkExists, string databaseName = null)
        {
            return (IJoinable)this.FromInto(table, targetTable, checkExists, databaseName);
        }

        IGroupedConditional IGroupableTarget.ConditionalFrom(string table, bool checkExists)
        {
            return (IGroupedConditional)this.ConditionalFrom(table, checkExists);
        }

        IGroupedConditional IGroupableTarget.ConditionalFromInto(string table, string targetTable, bool checkExists, string databaseName)
        {
            return (IGroupedConditional)this.ConditionalFromInto(table, targetTable, checkExists, databaseName);
        }

        IGroupedJoinable IGroupableTarget.JoinableFrom(string table, bool checkExists)
        {
            return (IGroupedJoinable)this.JoinableFrom(table, checkExists);
        }

        IGroupedJoinable IGroupableTarget.JoinableFromInto(string table, string targetTable, bool checkExists, string databaseName)
        {
            return (IGroupedJoinable)this.JoinableFromInto(table, targetTable, checkExists, databaseName);
        }
    }
}
