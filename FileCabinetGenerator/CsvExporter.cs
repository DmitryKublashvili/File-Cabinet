using FileCabinetApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Creates CSV file with records.
    /// </summary>
    static class CsvExporter
    {
        /// <summary>
        /// Creates CSV file with records.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <param name="records">Records.</param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <returns></returns>
        public static bool ExportInCsvFile(string fileName, IEnumerable<FileCabinetRecord> records, out string exceptionMessage)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(fileName))
                {
                    foreach (var item in records.First().GetType().GetProperties())
                    {
                        if (item.Name == "DateOfBirthString" || item.Name == "SexString")
                        {
                            continue;
                        }

                        sw.Write(item.Name + ";");
                    }

                    sw.WriteLine();

                    foreach (var item in records)
                    {
                        sw.WriteLine(item.Id.ToString(CultureInfo.InvariantCulture) + ";"
                            + item.FirstName.ToString(CultureInfo.InvariantCulture) + ";"
                            + item.LastName.ToString(CultureInfo.InvariantCulture) + ";"
                            + item.DateOfBirth.ToShortDateString() + ";"
                            + item.Sex.ToString(CultureInfo.InvariantCulture) + ";"
                            + item.Salary.ToString(CultureInfo.InvariantCulture) + ";"
                            + item.YearsOfService.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
            catch (Exception e)
            {
                exceptionMessage = e.Message;
                return false;
            }
            exceptionMessage = string.Empty;

            return true;
        }
    }
}
