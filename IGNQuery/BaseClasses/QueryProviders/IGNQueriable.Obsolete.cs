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
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {
        [Obsolete("Unparametrized version will be fully removed from api in v1.10")]
        public IGNQueriable Values(IEnumerable<object> valuesRow)
        {
            this.paramValues.Clear();
            this.lastParamIndex = 0;
            var valIndexes = new List<int>();
            int i;
            for (i = 0; i < valuesRow.Count(); i++)
            {
                paramValues.Add(new IGNParameterValue(lastParamIndex + i, valuesRow.ElementAt(i)));
                valIndexes.Add(lastParamIndex + i);
            }
            lastParamIndex += i;
            this.ValuesWithParams(valIndexes);
            return this;
        }

        [Obsolete("Unparametrized version will be fully removed from api in v1.10")]
        public IGNQueriable AddRow(IEnumerable<object> valuesRow)
        {
            var valIndexes = new List<int>();
            int i;
            for (i = 0; i < valuesRow.Count(); i++)
            {
                paramValues.Add(new IGNParameterValue(lastParamIndex + i, valuesRow.ElementAt(i)));
                valIndexes.Add(lastParamIndex + i);
            }
            lastParamIndex += i;
            this.AddRowWithParams(valIndexes);
            return this;
        }

        [Obsolete("Unparametrized version will be fully removed from api in next version")]
        public IGNQueriable Set(string colName, object value)
        {
            var result = $"{value}";
            if (value.GetType() == typeof(string))
            {
                result = "'" + result + "'";
            }
            if (value.GetType() == typeof(bool))
            {
                result = $"{((bool)value ? "'1'" : "'0'")}";
            }
            this.paramValues.Add(new IGNParameterValue(lastParamIndex, result));
            this.SetParametrized(colName, lastParamIndex);
            lastParamIndex++;
            return this;
        }

        [Obsolete("Unparametrized version will be fully removed from api in next version")]
        public IGNQueriable Condition(Func<IGNCondition> conditionalFunc)
        {
            var condition = conditionalFunc?.Invoke();
            this.paramValues.Add(new IGNParameterValue(lastParamIndex, condition.Value));
            this.ConditionWithParams(() => IGNConditionWithParameter.FromConfig(condition.ColumnName, condition.Operand, lastParamIndex));
            lastParamIndex++;
            return this;
        }
    }
}
