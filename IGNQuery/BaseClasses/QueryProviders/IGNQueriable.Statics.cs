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
using IGNQuery.Interfaces;
using System;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {

        public static IGNQueriable Begin(string email, IDataDriver dataDriver)
        {
            Activation.Activate(email);
            if (Activation.IsActive)
            {
                return new IGNQueriable(dataDriver);
            }
            else
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
        }

        public static IGNQueriable FromQueryString(string query, string email, IDataDriver dataDriver)
        {
            Activation.Activate(email);
            if (Activation.IsActive)
            {
                return new IGNQueriable(dataDriver)
                {
                    query = query,
                    exists = true,
                    canExecute = true
                };
            }
            else
            {
                throw new Exception("Please activate your copy of ignquery it's free of charge you just need to pass your email in constructor");
            }
        }

        public static void PrefixWith(string prefix, IGNQueriable queriable)
        {
            queriable.prefix = prefix;
        }

        public static void SuffixWith(string suffix, IGNQueriable queriable)
        {
            queriable.suffix = suffix;
        }

        public static void SetAfterObjectString(string queryPart, IGNQueriable queriable)
        {
            queriable.afterObjType = queryPart;
        }

        public static void SetExists(bool exists, IGNQueriable queriable)
        {
            queriable.exists = exists;
        }

        public static void SetCanExecute(ExistsEnum existsFunc, IGNQueriable queriable)
        {
            switch (existsFunc)
            {
                case ExistsEnum.Exists:
                    queriable.canExecute = queriable.exists;
                    break;
                case ExistsEnum.NotExists:
                    queriable.canExecute = !queriable.exists;
                    break;
                default:
                    queriable.canExecute = true;
                    break;
            }
        }
    }
}
