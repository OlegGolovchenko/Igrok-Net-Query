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

using System;

namespace IGNQuery.BaseClasses.Business
{
    public class IGNParameter
    {
        public int ParamNumber { get; private set; }

        public Type FieldType { get; private set; }

        public int TypeLength { get; private set; }

        internal IGNParameter(int paramNumber, Type fieldType, int typeLength)
        {
            ParamNumber = paramNumber;
            FieldType = fieldType;
            TypeLength = typeLength;
        }

        public static IGNParameter FromConfig(int paramNumber, Type fieldType, int typeLength)
        {
            return new IGNParameter(paramNumber, fieldType, typeLength);
        }

        public string AsQuery(GetDbTypeFunc getDbType)
        {
            return $"@p{ParamNumber} {getDbType(FieldType, TypeLength)}";
        }
    }

    public class IGNParameterValue
    {
        public int Position { get; private set; }

        public object Value { get; private set; }

        internal IGNParameterValue(int position, object value)
        {
            Position = position;
            Value = value;
        }

        public static IGNParameterValue FromConfig(int position, object value)
        {
            return new IGNParameterValue(position, value);
        }
    }
}
