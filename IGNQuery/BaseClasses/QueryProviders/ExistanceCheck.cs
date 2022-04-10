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
using System;
using System.Reflection;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class ExistanceCheck<T> : IExistanceCheck<T> where T : IQuery
    {
        private IGNQueriable queriable;
        private IGNDbObjectTypeEnum objectType;
        private string name;

        internal ExistanceCheck(IGNQueriable queriable, string name, IGNDbObjectTypeEnum objectType)
        {
            this.queriable = queriable;
            this.objectType = objectType;
            this.name = name;
        }

        public virtual T IfExists()
        {
            queriable.IfExists(objectType, name, ""); 
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable) }, null);
            return (T)ci.Invoke(new object[] { queriable });
        }

        public virtual T IfNotExists()
        {
            queriable.IfNotExists(objectType, name, "");
            Type t = typeof(T);

            ConstructorInfo ci = t.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable) }, null);
            return (T)ci.Invoke(new object[] { queriable });
        }
    }
}
