using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace LMS.Extensions
{
    /// <summary>
    /// Extension methods for System.String.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method for String that removes diacritics (for example, accents and umlauts) from a string.
        /// </summary>
        /// <param name="selfString">The String object to be extended.</param>
        /// <returns>Returns a string without any diacritics.</returns>
        public static string RemoveDiacritics(this string selfString)
        {
            if (String.IsNullOrEmpty(selfString))
                return String.Empty;

            string inputFormD = selfString.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < inputFormD.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(inputFormD[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(inputFormD[i]);
                }
            }

            return (stringBuilder.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}