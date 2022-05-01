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
    internal class OrderableCondition : Condition, IOrderableCondition
    {
        internal OrderableCondition(IGNQueriable queriable) : base(queriable)
        {
        }

        public IOrderable OrderBy(string column, bool descending)
        {
            queriable.AddOperation("ORDER BY", column, " ");
            if (descending)
            {
                queriable.AddOperation("", "DESC", "");
            }
            else
            {
                queriable.AddOperation("", "ASC", "");
            }
            return this;
        }

        IOrderableCondition IOrderableCondition.And(IGNConditionWithParameter condition)
        {
            base.And(condition);
            return this;
        }

        IOrderableCondition IOrderableCondition.Not(IGNConditionWithParameter condition)
        {
            base.Not(condition);
            return this;
        }

        IOrderableCondition IOrderableCondition.Or(IGNConditionWithParameter condition)
        {
            base.Or(condition);
            return this;
        }
    }
}
