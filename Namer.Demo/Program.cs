/*
 * MIT License
 *
 * Copyright (c) 2022 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;

namespace Namer.Demo
{
    internal class Program
    {
        static void Main()
        {
            // Generate 10 names
            Console.WriteLine("Generate 10 names\n");
            List<string> names = NameGenerator.GenerateNames();
            Console.WriteLine("- {0}\n\n", string.Join(", ", names));

            // Generate 20 names
            Console.WriteLine("Generate 20 names\n");
            List<string> names20 = NameGenerator.GenerateNames(20);
            Console.WriteLine("- {0}\n\n", string.Join(", ", names20));

            // Generate 5 names with custom name prefix
            Console.WriteLine("Generate 5 names with custom name prefix\n");
            List<string> names5nameprefix = NameGenerator.GenerateNames(5, "J", "m", "", "");
            Console.WriteLine("- {0}\n\n", string.Join(", ", names5nameprefix));

            // Generate 5 names with custom surname prefix
            Console.WriteLine("Generate 5 names with custom surname prefix\n");
            List<string> names5surnameprefix = NameGenerator.GenerateNames(5, "", "", "B", "g");
            Console.WriteLine("- {0}\n\n", string.Join(", ", names5surnameprefix));

            // Generate 5 names with custom name and surname prefix
            Console.WriteLine("Generate 5 names with custom name and surname prefix\n");
            List<string> namescomplete = NameGenerator.GenerateNames(5, "Ev", "n", "Na", "lo");
            Console.WriteLine("- {0}\n\n", string.Join(", ", namescomplete));
        }
    }
}