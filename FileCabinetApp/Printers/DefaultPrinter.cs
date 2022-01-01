using System;
using System.Collections.Generic;

namespace FileCabinetApp.Printers
{
    /// <summary>
    /// Provides default method for printing records.
    /// </summary>
    public class DefaultPrinter : IRecordPrinter
    {
        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="records">Records to print.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var item in records)
            {
                if (item is null)
                {
                    continue;
                }

                Console.WriteLine(
                    $"#{item.Id}, {item.FirstName}, {item.LastName}, " +
                    $"{item.DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                    $"Sex - {item.Sex}, Salary {item.Salary.ToString(Program.CultureInfoSettings)}, " +
                    $"{item.YearsOfService} years Of Service");
            }
        }
    }
}
