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
    internal class Alter : DropColumn, IAlter
    {
        internal Alter(IGNQueriable queriable, string table, bool checkExists) : base(queriable)
        {
            this.table = table;
            queriable.AddOperation("ALTER TABLE", queriable.SanitizeName(table), "");
            if (checkExists)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Table, table, string.Empty);
            }
        }

        public IAddColumn AddColumn(TableColumnConfiguration column, bool existsCheck)
        {
            string operand = queriable.HasAddColumnOrSqlServer() ? "" : "ADD";
            if(queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                operand += " COLUMN";
            }
            queriable.AddOperation(operand, queriable.FormatFieldOptionals(column), " ");
            if (existsCheck)
            {
                queriable.IfNotExists(IGNDbObjectTypeEnum.Column, column.ColumnName, table);
            }
            return this;
        }

        public IAlterColumn AlterColumn(TableColumnConfiguration column, bool existsCheck)
        {
            string operand = "ALTER COLUMN";
            if (queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                operand = operand.Replace("ALTER", "MODIFY");
            }
            queriable.AddOperation(operand, queriable.FormatFieldOptionals(column), " ");
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Column, column.ColumnName, table);
            }
            return this;
        }

        public IDropColumn DropColumn(string column, bool existsCheck)
        {
            string command = !queriable.HasDropColumnOrSqlServer() ? "DROP COLUMN" : "";
            queriable.AddOperation(command, queriable.SanitizeName(column), " ");
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Column, column, table);
            }
            return this;
        }
    }
}
