using System;

namespace LMS.Extensions
{
    /// <summary>
    /// Extension methods for System.DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Extension method for DateTime to get the first day of the week.
        /// </summary>
        /// <param name="selfDateTime">The DateTime object to be extended.</param>
        /// <param name="startOfWeek">A DayOfWeek enumerable setting the first day of week to Sunday or Monday.</param>
        /// <returns>Returns the first day of the week relative to the selfDateTime value.</returns>
        public static DateTime StartOfWeek(this DateTime selfDateTime, DayOfWeek startOfWeek)
        {
            int diff = selfDateTime.DayOfWeek - startOfWeek;
            if (diff < (int)DayOfWeek.Sunday) diff = 6;

            return selfDateTime.AddDays(-diff).Date;
        }

        /// <summary>
        /// Extension method for DateTime to get the first day of the next week.
        /// </summary>
        /// <param name="selfDateTime">The DateTime object to be extended.</param>
        /// <param name="startOfWeek">A DayOfWeek enumerable setting the first day of week to Sunday or Monday.</param>
        /// <returns>Returns the first day of the next week relative to the selfDateTime value.</returns>
        public static DateTime StartOfNextWeek(this DateTime selfDateTime, DayOfWeek startOfWeek)
        {
            int diff = DayOfWeek.Saturday - selfDateTime.DayOfWeek + (int)startOfWeek;
            if (diff > (int)DayOfWeek.Saturday) diff = 0;

            return selfDateTime.AddDays(diff + 1).Date;
        }

        /// <summary>
        /// Extension method for DateTime to add working days (skipping Saturday and Sunday).
        /// </summary>
        /// <param name="selfDateTime">The DateTime object to be extended.</param>
        /// <param name="numberOfDays">The number of days to be added.</param>
        /// <returns>Returns a DateTime object with numberOfDays added to the selfDateTime value.</returns>
        public static DateTime AddWorkDays(this DateTime selfDateTime, int numberOfDays)
        {
            if (numberOfDays < 0) numberOfDays = 0;

            for (int i = 0; i < numberOfDays; i++)
            {
                do
                {
                    selfDateTime = selfDateTime.AddDays(1);
                }
                while (selfDateTime.DayOfWeek == DayOfWeek.Saturday ||
                        selfDateTime.DayOfWeek == DayOfWeek.Sunday);

            }

            return selfDateTime;
        }
    }
}