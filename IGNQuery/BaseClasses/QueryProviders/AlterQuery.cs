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
    public class AlterQuery : QueryResult,
        IAlterQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        private string delimiter = " ";
        private bool isAddQuery = false;

        public AlterQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IAlterQuery Add()
        {
            isAddQuery = true;
            string operand = queriable.HasAddColumnOrSqlServer() ? "" : "ADD";
            queriable.AddOperation(operand, "", delimiter);
            delimiter = ", ";
            return this;
        }

        public IAlterQuery Alter()
        {
            queriable.AddOperation("ALTER", "", delimiter);
            delimiter = ", ";
            return this;
        }

        public IAlterExistenceCheckQuery Column(TableField column, int decimals = 0)
        {
            name = column.Name;
            objectType = IGNDbObjectTypeEnum.Column;
            string operand = isAddQuery ? "" : "COLUMN";
            queriable.AddOperation(operand, queriable.FormatFieldOptionals(column, decimals), "");
            return this;
        }

        public IAlterExistenceCheckQuery Drop(string column)
        {
            this.name = column;
            objectType = IGNDbObjectTypeEnum.Column;
            string command = !queriable.HasDropColumnOrSqlServer() ? "DROP COLUMN" : "";
            queriable.AddOperation(command, queriable.SanitizeName(name), delimiter);
            delimiter = ", ";
            return this;
        }

        public IAlterQuery IfExists()
        {
            queriable.IfExists(objectType, name, "");
            return this;
        }

        public IAlterQuery IfNotExists()
        {
            queriable.IfNotExists(objectType, name, "");
            return this;
        }

        public IAlterExistenceCheckQuery Table(string tableName)
        {
            name = tableName;
            objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("ALTER TABLE", queriable.SanitizeName(tableName), "");
            return this;
        }
    }
}
