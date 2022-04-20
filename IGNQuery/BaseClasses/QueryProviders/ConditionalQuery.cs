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

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class ConditionalQuery : QueryResult, IConditionalQuery
    {
        internal ConditionalQuery(IGNQueriable queriable):base(queriable)
        {
        }

        internal static new IConditionalQuery Init(IGNQueriable queriable)
        {
            return new ConditionalQuery(queriable);
        }

        public IConditionalQuery Where(IGNConditionWithParameter condition)
        {
            condition.SetSanitizedName(queriable.SanitizeName(condition.ColumnName));
            queriable.AddOperation("WHERE", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery And(IGNConditionWithParameter condition)
        {
            queriable.AddOperation("ADD", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery Or(IGNConditionWithParameter condition)
        {
            queriable.AddOperation("OR", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery Not(IGNConditionWithParameter condition)
        {
            queriable.AddOperation("NOT", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery Having(IGNConditionWithParameter condition)
        {
            condition.SetSanitizedName(queriable.SanitizeName(condition.ColumnName));
            queriable.AddOperation("HAVING", $"{condition}", " ");
            return this;
        }

        public IGroupOrderQuery GroupBy(string column)
        {
            queriable.AddOperation("GROUP BY", column, " ");
            return new GroupOrderQuery(queriable);
        }

        public IGroupOrderQuery OrderBy(string column)
        {
            queriable.AddOperation("ORDER BY", column, " ");
            return new GroupOrderQuery(queriable);
        }
    }
}
