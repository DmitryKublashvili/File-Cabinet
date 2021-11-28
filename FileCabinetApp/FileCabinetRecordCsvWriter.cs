using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Records state in CSV file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="textWriter">Some text writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter textWriter)
        {
            this.Writer = textWriter;
        }

        /// <summary>
        /// Gets or sets and sets text writer.
        /// </summary>
        /// <value>
        /// Textwriter for writing text in file.
        /// </value>
        public TextWriter Writer { get; set; }

        /// <summary>
        /// Writes state to file in CSV format.
        /// </summary>
        /// <param name="state">State for recording.</param>
        public void Write(FileCabinetServiceSnapshot state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            int recordsCount = state.GetState().Count;

            var records = state.GetState();

            foreach (var item in records[0].GetType().GetProperties())
            {
                this.Writer.Write(item.Name + ";");
            }

            this.Writer.WriteLine();

            for (int i = 0; i < recordsCount; i++)
            {
                this.Writer.WriteLine(records[i].Id.ToString(CultureInfo.InvariantCulture) + ";"
                    + records[i].FirstName.ToString(CultureInfo.InvariantCulture) + ";"
                    + records[i].LastName.ToString(CultureInfo.InvariantCulture) + ";"
                    + records[i].DateOfBirth.ToShortDateString() + ";"
                    + records[i].Sex.ToString(CultureInfo.InvariantCulture) + ";"
                    + records[i].Salary.ToString(CultureInfo.InvariantCulture) + ";"
                    + records[i].YearsOfService.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}