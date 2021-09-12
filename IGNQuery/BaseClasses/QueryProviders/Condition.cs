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

using System;
using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using IGNQuery.BaseClasses.Business;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class Condition : ICondition
    {
        private readonly IGNQueriable queriable;
        private string fieldName;

        public Condition(IGNQueriable queriable)
        {
            this.queriable = queriable;
        }

        public ICondition And()
        {
            this.queriable.And();
            return this;
        }

        public ICondition BoolEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Eq, paramNb));
            return this;
        }

        public ICondition BoolNotEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Ne, paramNb));
            return this;
        }

        public ICondition Field(string name)
        {
            this.fieldName = name;
            return this;
        }

        public IQueryResult Go()
        {
            return new QueryResult(this.queriable);
        }

        public ICondition LongEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Eq, paramNb));
            return this;
        }

        public ICondition LongNotEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Ne, paramNb));
            return this;
        }

        public ICondition Or()
        {
            this.queriable.Or();
            return this;
        }

        public ICondition StringEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Eq, paramNb));
            return this;
        }

        public ICondition StringNotEqualToParam(int paramNb)
        {
            this.queriable.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(this.fieldName, IGNSqlCondition.Ne, paramNb));
            return this;
        }
    }
}
