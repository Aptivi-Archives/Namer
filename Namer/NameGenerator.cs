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

namespace Namer
{
	public static class NameGenerator
    {
		internal static string[] Names = Array.Empty<string>();
		internal static string[] Surnames = Array.Empty<string>();
		private static readonly HttpClient NameClient = new HttpClient();

		/// <summary>
		/// Populates the names and the surnames for the purpose of initialization
		/// </summary>
		public static void PopulateNames()
		{
			try
			{
				if (Names.Length == 0)
				{
					HttpResponseMessage Response = NameClient.GetAsync("https://cdn.jsdelivr.net/gh/Aptivi/NamesList@master/Processed/FirstNames.txt").Result;
					Response.EnsureSuccessStatusCode();
					Stream NamesStream = Response.Content.ReadAsStreamAsync().Result;
					string NamesString = new StreamReader(NamesStream).ReadToEnd();
					Names = NamesString.SplitNewLines();
				}
				if (Surnames.Length == 0)
				{
					HttpResponseMessage Response = NameClient.GetAsync("https://cdn.jsdelivr.net/gh/Aptivi/NamesList@master/Processed/Surnames.txt").Result;
					Response.EnsureSuccessStatusCode();
					Stream SurnamesStream = Response.Content.ReadAsStreamAsync().Result;
					string SurnamesString = new StreamReader(SurnamesStream).ReadToEnd();
					Surnames = SurnamesString.SplitNewLines();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Can't get names and surnames:" + $" {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Generates the names
		/// </summary>
		/// <returns>List of generated names</returns>
		public static List<string> GenerateNames()
		{
			return GenerateNames(10, "", "", "", "");
		}

		/// <summary>
		/// Generates the names
		/// </summary>
		/// <param name="Count">How many names to generate?</param>
		/// <returns>List of generated names</returns>
		public static List<string> GenerateNames(int Count)
		{
			return GenerateNames(Count, "", "", "", "");
		}

		/// <summary>
		/// Generates the names
		/// </summary>
		/// <param name="Count">How many names to generate?</param>
		/// <param name="NamePrefix">What should the name start with?</param>
		/// <param name="NameSuffix">What should the name end with?</param>
		/// <param name="SurnamePrefix">What should the surname start with?</param>
		/// <param name="SurnameSuffix">What should the surname end with?</param>
		/// <returns>List of generated names</returns>
		public static List<string> GenerateNames(int Count, string NamePrefix, string NameSuffix, string SurnamePrefix, string SurnameSuffix)
		{
			Random RandomDriver = new Random();
			List<string> NamesList = new List<string>();

			// Initialize names
			PopulateNames();

			// Check if the prefix and suffix check is required
			bool NamePrefixCheckRequired = !string.IsNullOrEmpty(NamePrefix);
			bool NameSuffixCheckRequired = !string.IsNullOrEmpty(NameSuffix);
			bool SuramePrefixCheckRequired = !string.IsNullOrEmpty(SurnamePrefix);
			bool SurameSuffixCheckRequired = !string.IsNullOrEmpty(SurnameSuffix);

			// Process the names according to suffix and/or prefix check requirement
			string[] ProcessedNames = Names;
			if (NamePrefixCheckRequired && NameSuffixCheckRequired)
				ProcessedNames = Names.Where((str) => str.StartsWith(NamePrefix) && str.EndsWith(NameSuffix)).ToArray();
			else if (NamePrefixCheckRequired)
				ProcessedNames = Names.Where((str) => str.StartsWith(NamePrefix)).ToArray();
			else if (NameSuffixCheckRequired)
				ProcessedNames = Names.Where((str) => str.EndsWith(NameSuffix)).ToArray();

			// Do the same for the surnames
			string[] ProcessedSurnames = Surnames;
			if (NamePrefixCheckRequired && NameSuffixCheckRequired)
				ProcessedSurnames = Surnames.Where((str) => str.StartsWith(SurnamePrefix) && str.EndsWith(SurnameSuffix)).ToArray();
			else if (NamePrefixCheckRequired)
				ProcessedSurnames = Surnames.Where((str) => str.StartsWith(SurnamePrefix)).ToArray();
			else if (NameSuffixCheckRequired)
				ProcessedSurnames = Surnames.Where((str) => str.EndsWith(SurnameSuffix)).ToArray();

			// Check the names and the surnames
			if (ProcessedNames.Length == 0)
				throw new Exception("The names are not found! Please ensure that the name conditions are correct.");
			if (ProcessedSurnames.Length == 0)
				throw new Exception("The surnames are not found! Please ensure that the surname conditions are correct.");

			// Select random names
			for (int NameNum = 1; NameNum <= Count; NameNum++)
			{
				// Get the names
				string GeneratedName = ProcessedNames[RandomDriver.Next(ProcessedNames.Length)];
				string GeneratedSurname = ProcessedSurnames[RandomDriver.Next(ProcessedSurnames.Length)];
				NamesList.Add(GeneratedName + " " + GeneratedSurname);
			}
			return NamesList;
		}

		/// <summary>
		/// Makes a string array with new line as delimiter
		/// </summary>
		/// <param name="Str">Target string</param>
		/// <returns></returns>
		internal static string[] SplitNewLines(this string Str)
		{
			return Str.Replace(Convert.ToChar(13), default).Split(Convert.ToChar(10));
		}
	}
}