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
using System.Collections.Generic;

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

        public IQueryResult AddDefault(string column, string name, int index, bool existsCheck)
        {
            string operand = "ALTER COLUMN";
            if (queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                operand = "ALTER";
            }
            queriable.AddOperation(operand, queriable.SanitizeName(column), " ");
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Column, column, table);
            }
            if (queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                string defOperand = "SET DEFAULT";
                queriable.AddOperation(defOperand, $"@p{index}", " ");
            }
            else
            {
                queriable.AddOperation("ADD CONSTRAINT", $"DF_{name}", " ");
                queriable.AddOperation("DEFAULT", $"@p{index}", " ");
            }
            return this;
        }

        public IQueryResult AddForeignKey(string name, string sourceColumn, string targetTable, string column, bool existsCheck)
        {
            string fkName = $"FK_{name}";
            queriable.AddOperation("ADD CONSTRAINT", fkName, " ");
            queriable.IfNotExists(IGNDbObjectTypeEnum.PrimaryKey, fkName, "");
            queriable.AddOperation("FOREIGN KEY", $"({queriable.SanitizeName(sourceColumn)}) REFERENCES {queriable.SanitizeName(targetTable)}({queriable.SanitizeName(column)})", " ");
            return this;
        }

        public IAlter AddPrimaryKey(string name, IList<string> columns, bool existsCheck)
        {
            string pkName = $"PK_{name}";
            queriable.AddOperation("ADD CONSTRAINT", pkName, " ");
            queriable.IfNotExists(IGNDbObjectTypeEnum.PrimaryKey, pkName, "");
            queriable.AddOperation("PRIMARY KEY", $"({string.Join(",", columns)})", " ");
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

        public IQueryResult DropDefault(string column, bool existsCheck)
        {
            string operand = "ALTER COLUMN";
            if (queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                operand = "ALTER";
            }
            queriable.AddOperation(operand, queriable.SanitizeName(column), " ");
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Column, column, table);
            }
            queriable.AddOperation("DROP DEFAULT", string.Empty, " ");
            return this;
        }

        public IQueryResult DropForeignKey(string name, bool checkExists)
        {
            string fkName = $"FK_{name}";
            string commmand = "DROP CONSTRAINT";
            if (this.queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                commmand = "DROP FOREIGN KEY";
                queriable.AddOperation(commmand, fkName, " ");
            }
            else
            {
                queriable.AddOperation(commmand, fkName, " ");
            }
            queriable.IfExists(IGNDbObjectTypeEnum.PrimaryKey, fkName, "");
            return this;
        }

        public IAlter DropPrimaryKey(string name, bool existsCheck)
        {
            string pkName = $"PK_{name}";
            string commmand = "DROP CONSTRAINT";
            if(this.queriable.dataDriver.Dialect == DialectEnum.MySQL)
            {
                commmand = "DROP PRIMARY KEY";
                queriable.AddOperation(commmand, "", " ");
            }
            else
            {
                queriable.AddOperation(commmand, pkName, " ");
            }
            queriable.IfExists(IGNDbObjectTypeEnum.PrimaryKey, pkName, "");
            return this;
        }
    }
}
