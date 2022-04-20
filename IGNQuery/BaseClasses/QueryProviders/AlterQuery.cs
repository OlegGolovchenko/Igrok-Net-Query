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

namespace IGNQuery.BaseClasses.QueryProviders
{

    internal class AlterQuery : QueryResult,
        IAlterQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        internal string delimiter = " ";

        internal AlterQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IAddColumnQuery Add()
        {
            string operand = queriable.HasAddColumnOrSqlServer() ? "" : "ADD";
            queriable.AddOperation(operand, "", delimiter);
            delimiter = ", ";
            return new AddColumnQuery(queriable, delimiter);
        }

        public IAlterColumnQuery Alter()
        {
            queriable.AddOperation("ALTER", "", delimiter);
            delimiter = ", ";
            return new AlterColumnQuery(queriable, delimiter);
        }

        public IAlterExistsCheckQuery Drop(string column)
        {
            this.name = column;
            objectType = IGNDbObjectTypeEnum.Column;
            string command = !queriable.HasDropColumnOrSqlServer() ? "DROP COLUMN" : "";
            queriable.AddOperation(command, queriable.SanitizeName(name), delimiter);
            delimiter = ", ";
            return new AlterExistsCheckQuery(queriable, column, objectType, delimiter);
        }

        public IAlterExistsCheckQuery Table(string tableName)
        {
            name = tableName;
            objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("ALTER TABLE", queriable.SanitizeName(tableName), "");
            return new AlterExistsCheckQuery(queriable, tableName, objectType, delimiter);
        }
    }
}
