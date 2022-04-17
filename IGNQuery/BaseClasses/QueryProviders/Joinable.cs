﻿//#############################################
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

using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class Joinable : ConditionalQuery, IJoinable
    {
        internal Joinable(IGNQueriable queriable) : base(queriable)
        {
        }

        internal static new IJoinable Init(IGNQueriable queriable)
        {
            return new Joinable(queriable);
        }

        public IJoinable InnerJoin(string source, string destination, string srcColumn, string destColumn)
        {
            queriable.AddOperation("INNER JOIN", queriable.SanitizeName(destination), " ");
            string parameter = $"{queriable.SanitizeName(source)}.{queriable.SanitizeName(srcColumn)} = " +
                $"{queriable.SanitizeName(destination)}.{queriable.SanitizeName(destColumn)}";
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }

        public IJoinable LeftJoin(string source, string destination, string srcColumn, string destColumn)
        {
            queriable.AddOperation("LEFT JOIN", queriable.SanitizeName(destination), " ");
            string parameter = $"{queriable.SanitizeName(source)}.{queriable.SanitizeName(srcColumn)} = " +
                $"{queriable.SanitizeName(destination)}.{queriable.SanitizeName(destColumn)}";
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }

        public IJoinable RightJoin(string source, string destination, string srcColumn, string destColumn)
        {
            queriable.AddOperation("RIGHT JOIN", queriable.SanitizeName(destination), " ");
            string parameter = $"{queriable.SanitizeName(source)}.{queriable.SanitizeName(srcColumn)} = " +
                $"{queriable.SanitizeName(destination)}.{queriable.SanitizeName(destColumn)}";
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }
    }
}
