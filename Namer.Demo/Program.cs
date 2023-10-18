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

namespace Namer.Demo
{
    internal class Program
    {
        static void Main()
        {
            foreach (string genderName in Enum.GetNames(typeof(NameGenderType)))
            {
                // Get gender
                var genderType = (NameGenderType)Enum.Parse(typeof(NameGenderType), genderName);
                Console.WriteLine($"{genderName}\n");

                // Generate 10 names
                Console.WriteLine("Generate 10 names\n");
                var names = NameGenerator.GenerateNames(genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", names));

                // Generate 20 names
                Console.WriteLine("Generate 5 names\n");
                var names5 = NameGenerator.GenerateNames(5, genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", names5));

                // Generate 5 names with custom name prefix
                Console.WriteLine("Generate 5 names with custom name prefix\n");
                var names5nameprefix = NameGenerator.GenerateNames(5, "J", "n", "", "", genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", names5nameprefix));

                // Generate 5 names with custom surname prefix
                Console.WriteLine("Generate 5 names with custom surname prefix\n");
                var names5surnameprefix = NameGenerator.GenerateNames(5, "", "", "B", "g", genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", names5surnameprefix));

                // Generate 5 names with custom name and surname prefix
                Console.WriteLine("Generate 5 names with custom name and surname prefix\n");
                var namescomplete = NameGenerator.GenerateNames(5, "Ev", "n", "Na", "lo", genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", namescomplete));

                // Find first names
                Console.WriteLine("Find first names\n");
                var findnames = NameGenerator.FindFirstNames("Mic", "", "", genderType);
                Console.WriteLine("- {0}\n", string.Join(", ", findnames));
            }
        }
    }
}
