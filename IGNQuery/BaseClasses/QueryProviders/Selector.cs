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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class Selector<U, T> : ISelector<U, T>
        where T : IQuery
        where U : IExistanceCheck<T>
    {
        private IGNDbObjectTypeEnum objectType;
        private string sqloperand = "";
        private IGNQueriable queriable;

        public Selector(IGNQueriable queriable, string sqlOperand)
        {
            this.queriable = queriable;
            this.sqloperand = sqlOperand;
        }

        public U Database(string name)
        {
            queriable.AddOperation($"{sqloperand} DATABASE", queriable.SanitizeName(name), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U Index(string name, string tableName, IEnumerable<string> columns, bool unique)
        {
            objectType = unique ? IGNDbObjectTypeEnum.UniqueIndex : IGNDbObjectTypeEnum.Index;
            string operand = unique ? $"{sqloperand} INDEX" : $"{sqloperand} UNIQUE INDEX";
            queriable.AddOperation(operand, queriable.SanitizeName(name), "");
            queriable.AddOperation("ON", tableName, " ");
            queriable.AddOperation($"({string.Join(",", columns.Select(x => queriable.SanitizeName(x)))})", "", "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U Index(string name, string table, bool unique)
        {
            objectType = unique ? IGNDbObjectTypeEnum.UniqueIndex : IGNDbObjectTypeEnum.Index;
            queriable.AddOperation("DROP INDEX", queriable.SanitizeName(name), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U StoredProcedure(string name, IGNQueriable content, [Optional] IEnumerable<IGNParameter> parameters)
        {
            queriable.AddOperation($"{sqloperand} PROCEDURE", queriable.SanitizeName(name), "");
            queriable.AddOperation(queriable.FormatSpSubqueryAndParams(parameters ?? new List<IGNParameter>(), content), "", "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U StoredProcedure(string name)
        {
            queriable.AddOperation("DROP PROCEDURE", queriable.SanitizeName(name), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U Table(string name, IEnumerable<TableColumnConfiguration> fields)
        {
            queriable.AddOperation($"{sqloperand} TABLE", queriable.SanitizeName(name), "");
            queriable.AddOperation($"({string.Join(",", queriable.CompileFieldsInfo(name, fields))})", "", "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U Table(string name)
        {
            queriable.AddOperation("DROP TABLE", queriable.SanitizeName(name), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U View(string name, IGNQueriable content)
        {
            queriable.AddOperation($"{sqloperand} VIEW", queriable.SanitizeName(name), "");
            queriable.AddOperation("AS", "", "\n");
            queriable.AddOperation($"{content}", "", "\n");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }

        public virtual U View(string name)
        {
            queriable.AddOperation("DROP VIEW", queriable.SanitizeName(name), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database });
        }
    }
}
