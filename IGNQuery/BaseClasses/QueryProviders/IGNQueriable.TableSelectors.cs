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

        public IGNQueriable Table(string name)
        {
            if (string.IsNullOrWhiteSpace(this.format))
            {
                throw new Exception("Please call table after Create Update or Alter ddl query");
            }
            this.objectType = IGNDbObjectTypeEnum.Table;
            this.objectName = name;
            return this;
        }

        public IGNQueriable Table(string name, IGNQueriable alterQuery)
        {
            if (string.IsNullOrWhiteSpace(this.format))
            {
                throw new Exception("Please call table after Create Update or Alter ddl query");
            }
            this.objectType = IGNDbObjectTypeEnum.Table;
            this.objectName = name;
            this.querySpecificPart = alterQuery.ToString();
            this.subquery = alterQuery;
            return this;
        }

        public IGNQueriable Table(string name,
            Func<IEnumerable<TableColumnConfiguration>> fields)
        {
            if (string.IsNullOrWhiteSpace(this.format))
            {
                throw new Exception("Please call table after Create Update or Alter ddl query");
            }
            this.objectType = IGNDbObjectTypeEnum.Table;
            this.objectName = name;
            var fieldsInfo = fields?.Invoke();
            this.querySpecificPart = $"({string.Join(",", CompileFieldsInfo(name, fieldsInfo))})";
            return this;
        }
    }
}
