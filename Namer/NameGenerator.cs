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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Namer
{
    /// <summary>
    /// Name generator class
    /// </summary>
	public static class NameGenerator
    {
		internal static string[] Names = Array.Empty<string>();
		internal static string[] Surnames = Array.Empty<string>();
		private static readonly HttpClient NameClient = new();
        private static readonly string nameAddressPart = "https://cdn.jsdelivr.net/gh/Aptivi/NamesList@latest/Processed/";
        private static readonly string unifiedNameListFileName = "FirstNames.txt";
        private static readonly string femaleNameListFileName = "FirstNames_Female.txt";
        private static readonly string maleNameListFileName = "FirstNames_Male.txt";
        private static readonly string surnameListFileName = "Surnames.txt";

        /// <summary>
        /// Populates the names and the surnames for the purpose of initialization
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        public static void PopulateNames(NameGenderType genderType = NameGenderType.Unified) =>
            PopulateNamesAsync(genderType).Wait();

        /// <summary>
        /// [Async] Populates the names and the surnames for the purpose of initialization
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        public static async Task PopulateNamesAsync(NameGenderType genderType = NameGenderType.Unified)
        {
            try
            {
                string surnameAddress = $"{nameAddressPart}{surnameListFileName}";
                string namesFileName =
                    genderType == NameGenderType.Female ? femaleNameListFileName :
                    genderType == NameGenderType.Male ? maleNameListFileName :
                    unifiedNameListFileName;
                string nameAddress = $"{nameAddressPart}{namesFileName}";

                if (Names.Length == 0)
                    Names = await PopulateInternalAsync(nameAddress);
                if (Surnames.Length == 0)
                    Surnames = await PopulateInternalAsync(surnameAddress);
            }
            catch (Exception ex)
            {
                throw new Exception("Can't get names and surnames:" + $" {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generates the names
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateNames(NameGenderType genderType = NameGenderType.Unified) =>
            GenerateNames(10, "", "", "", "", genderType);

        /// <summary>
        /// Generates the names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateNames(int Count, NameGenderType genderType = NameGenderType.Unified) =>
            GenerateNames(Count, "", "", "", "", genderType);

        /// <summary>
        /// Generates the names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="NamePrefix">What should the name start with?</param>
        /// <param name="NameSuffix">What should the name end with?</param>
        /// <param name="SurnamePrefix">What should the surname start with?</param>
        /// <param name="SurnameSuffix">What should the surname end with?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateNames(int Count, string NamePrefix, string NameSuffix, string SurnamePrefix, string SurnameSuffix, NameGenderType genderType = NameGenderType.Unified)
		{
			// Initialize names
			PopulateNames(genderType);
			return GenerateNameArray(Count, NamePrefix, NameSuffix, SurnamePrefix, SurnameSuffix);
        }

        /// <summary>
        /// [Async] Generates the names
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateNamesAsync(NameGenderType genderType = NameGenderType.Unified) =>
            await GenerateNamesAsync(10, "", "", "", "", genderType);

        /// <summary>
        /// [Async] Generates the names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateNamesAsync(int Count, NameGenderType genderType = NameGenderType.Unified) =>
            await GenerateNamesAsync(Count, "", "", "", "", genderType);

        /// <summary>
        /// [Async] Generates the names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="NamePrefix">What should the name start with?</param>
        /// <param name="NameSuffix">What should the name end with?</param>
        /// <param name="SurnamePrefix">What should the surname start with?</param>
        /// <param name="SurnameSuffix">What should the surname end with?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateNamesAsync(int Count, string NamePrefix, string NameSuffix, string SurnamePrefix, string SurnameSuffix, NameGenderType genderType = NameGenderType.Unified)
        {
            // Initialize names
            await PopulateNamesAsync(genderType);
            return GenerateNameArray(Count, NamePrefix, NameSuffix, SurnamePrefix, SurnameSuffix);
        }

        /// <summary>
        /// Generates the first names
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateFirstNames(NameGenderType genderType = NameGenderType.Unified) =>
            GenerateFirstNames(10, "", "", genderType);

        /// <summary>
        /// Generates the first names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateFirstNames(int Count, NameGenderType genderType = NameGenderType.Unified) =>
            GenerateFirstNames(Count, "", "", genderType);

        /// <summary>
        /// Generates the first names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="NamePrefix">What should the name start with?</param>
        /// <param name="NameSuffix">What should the name end with?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateFirstNames(int Count, string NamePrefix, string NameSuffix, NameGenderType genderType = NameGenderType.Unified)
		{
			// Initialize names
			PopulateNames(genderType);
			return GenerateFirstNameArray(Count, NamePrefix, NameSuffix);
        }

        /// <summary>
        /// [Async] Generates the first names
        /// </summary>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateFirstNamesAsync(NameGenderType genderType = NameGenderType.Unified) =>
            await GenerateFirstNamesAsync(10, "", "", genderType);

        /// <summary>
        /// [Async] Generates the first names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateFirstNamesAsync(int Count, NameGenderType genderType = NameGenderType.Unified) =>
            await GenerateFirstNamesAsync(Count, "", "", genderType);

        /// <summary>
        /// [Async] Generates the first names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="NamePrefix">What should the name start with?</param>
        /// <param name="NameSuffix">What should the name end with?</param>
        /// <param name="genderType">Gender type to use when generating names</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateFirstNamesAsync(int Count, string NamePrefix, string NameSuffix, NameGenderType genderType = NameGenderType.Unified)
        {
            // Initialize names
            await PopulateNamesAsync(genderType);
            return GenerateFirstNameArray(Count, NamePrefix, NameSuffix);
        }

        /// <summary>
        /// Generates the last names
        /// </summary>
        /// <returns>List of generated names</returns>
        public static string[] GenerateLastNames() =>
            GenerateLastNames(10, "", "");

        /// <summary>
        /// Generates the last names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateLastNames(int Count) =>
            GenerateLastNames(Count, "", "");

        /// <summary>
        /// Generates the last names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="SurnamePrefix">What should the surname start with?</param>
        /// <param name="SurnameSuffix">What should the surname end with?</param>
        /// <returns>List of generated names</returns>
        public static string[] GenerateLastNames(int Count, string SurnamePrefix, string SurnameSuffix)
        {
            // Initialize names
            PopulateNames();
            return GenerateLastNameArray(Count, SurnamePrefix, SurnameSuffix);
        }

        /// <summary>
        /// [Async] Generates the last names
        /// </summary>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateLastNamesAsync() =>
            await GenerateLastNamesAsync(10, "", "");

        /// <summary>
        /// [Async] Generates the last names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateLastNamesAsync(int Count) =>
            await GenerateLastNamesAsync(Count, "", "");

        /// <summary>
        /// [Async] Generates the last names
        /// </summary>
        /// <param name="Count">How many names to generate?</param>
        /// <param name="SurnamePrefix">What should the surname start with?</param>
        /// <param name="SurnameSuffix">What should the surname end with?</param>
        /// <returns>List of generated names</returns>
        public static async Task<string[]> GenerateLastNamesAsync(int Count, string SurnamePrefix, string SurnameSuffix)
        {
            // Initialize names
            await PopulateNamesAsync();
            return GenerateLastNameArray(Count, SurnamePrefix, SurnameSuffix);
        }

        internal static string[] GenerateFirstNameArray(int Count, string NamePrefix, string NameSuffix)
        {
            var random = new Random();
			List<string> namesList = new();

            // Check if the prefix and suffix check is required
            bool NamePrefixCheckRequired = !string.IsNullOrEmpty(NamePrefix);
            bool NameSuffixCheckRequired = !string.IsNullOrEmpty(NameSuffix);

            // Process the names according to suffix and/or prefix check requirement
            string[] ProcessedNames = Names;
            if (NamePrefixCheckRequired && NameSuffixCheckRequired)
                ProcessedNames = Names.Where((str) => str.StartsWith(NamePrefix) && str.EndsWith(NameSuffix)).ToArray();
            else if (NamePrefixCheckRequired)
                ProcessedNames = Names.Where((str) => str.StartsWith(NamePrefix)).ToArray();
            else if (NameSuffixCheckRequired)
                ProcessedNames = Names.Where((str) => str.EndsWith(NameSuffix)).ToArray();

            // Check the names
            if (ProcessedNames.Length == 0)
                throw new Exception("The names are not found! Please ensure that the name conditions are correct.");

            // Select random names
            for (int NameNum = 1; NameNum <= Count; NameNum++)
            {
                // Get the names
                string GeneratedName = ProcessedNames[random.Next(ProcessedNames.Length)];
                namesList.Add(GeneratedName);
            }
            return namesList.ToArray();
        }

        internal static string[] GenerateLastNameArray(int Count, string SurnamePrefix, string SurnameSuffix)
        {
            var random = new Random();
			List<string> surnamesList = new();

            // Check if the prefix and suffix check is required
            bool SurnamePrefixCheckRequired = !string.IsNullOrEmpty(SurnamePrefix);
            bool SurnameSuffixCheckRequired = !string.IsNullOrEmpty(SurnameSuffix);

            // Process the surnames according to suffix and/or prefix check requirement
            string[] ProcessedSurnames = Surnames;
            if (SurnamePrefixCheckRequired && SurnameSuffixCheckRequired)
                ProcessedSurnames = Surnames.Where((str) => str.StartsWith(SurnamePrefix) && str.EndsWith(SurnameSuffix)).ToArray();
            else if (SurnamePrefixCheckRequired)
                ProcessedSurnames = Surnames.Where((str) => str.StartsWith(SurnamePrefix)).ToArray();
            else if (SurnameSuffixCheckRequired)
                ProcessedSurnames = Surnames.Where((str) => str.EndsWith(SurnameSuffix)).ToArray();

            // Check the surnames
            if (ProcessedSurnames.Length == 0)
                throw new Exception("The surnames are not found! Please ensure that the surname conditions are correct.");

            // Select random surnames
            for (int NameNum = 1; NameNum <= Count; NameNum++)
            {
                // Get the surnames
                string GeneratedSurname = ProcessedSurnames[random.Next(ProcessedSurnames.Length)];
                surnamesList.Add(GeneratedSurname);
            }
            return surnamesList.ToArray();
        }

        internal static string[] GenerateNameArray(int Count, string NamePrefix, string NameSuffix, string SurnamePrefix, string SurnameSuffix)
        {
			List<string> namesList = new();

            // Get random names and surnames
            string[] names = GenerateFirstNameArray(Count, NamePrefix, NameSuffix);
            string[] surnames = GenerateLastNameArray(Count, SurnamePrefix, SurnameSuffix);

            // Select random names
            for (int NameNum = 1; NameNum <= Count; NameNum++)
            {
                // Get the names
                string GeneratedName = names[NameNum - 1];
                string GeneratedSurname = surnames[NameNum - 1];
                namesList.Add(GeneratedName + " " + GeneratedSurname);
            }
            return namesList.ToArray();
        }

        internal static async Task<string[]> PopulateInternalAsync(string nameLink)
        {
            HttpResponseMessage Response = await NameClient.GetAsync(nameLink);
            Response.EnsureSuccessStatusCode();
            Stream SurnamesStream = await Response.Content.ReadAsStreamAsync();
            string SurnamesString = new StreamReader(SurnamesStream).ReadToEnd();
            return SurnamesString.SplitNewLines();
        }

        /// <summary>
        /// Makes a string array with new line as delimiter
        /// </summary>
        /// <param name="Str">Target string</param>
        /// <returns></returns>
        internal static string[] SplitNewLines(this string Str) =>
            Str.Replace(Convert.ToChar(13), default).Split(Convert.ToChar(10));
    }
}
