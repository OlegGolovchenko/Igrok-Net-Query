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

using System;

namespace IGNQuery
{
    public class TableField
    { 
        public string Name { get; set; }
        public string Type { get; set; }
        public bool CanHaveNull { get; set; }

        public bool Generated { get; set; }

        public string DefValue { get; set; }

        public bool Primary { get; set; }

        public const string TYPE_LONG = "bigint";

        public const string TYPE_BOOLEAN = "bit";

        public static string TypeNvarchar(int length)
        {
            return $"nvarchar({length})";
        }

        public TableField()
        {
            //if(!Activation.IsActive)
            //{
            //    throw new Exception("Product is not activated, please call Activation.Activate([email]) to activate product. This product is totally free. Your info will be used only for licensing purposes. to read more visit https://igrok-net.org");
            //}
        }
    }
}
