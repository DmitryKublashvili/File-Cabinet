using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Reads state from CSV file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader">Some text reader.</param>
        public FileCabinetRecordCsvReader(TextReader reader)
        {
            this.Reader = reader;
        }

        /// <summary>
        /// Gets or sets and sets text reader.
        /// </summary>
        /// <value>
        /// Textreader for reading text from file.
        /// </value>
        public TextReader Reader { get; set; }

        /// <summary>
        /// Reads records from file in CSV format.
        /// </summary>
        /// <returns>List of loaded records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            var textRecords = this.Reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            this.Reader.Close();

            for (int i = 1; i < textRecords.Length; i++)
            {
                FileCabinetRecord record = new FileCabinetRecord();

                var recordParametersFromFile = textRecords[i].Split(';');

                record.Id = int.Parse(recordParametersFromFile[0], CultureInfo.InvariantCulture);
                record.FirstName = recordParametersFromFile[1];
                record.LastName = recordParametersFromFile[2];
                var dateTimeStrings = recordParametersFromFile[3].Split('.');
                record.DateOfBirth = DateTime.Parse(dateTimeStrings[1] + "/" + dateTimeStrings[0] + "/" + dateTimeStrings[2], CultureInfo.InvariantCulture);
                record.Sex = recordParametersFromFile[4] == "M" ? 'M' : 'F';
                record.Salary = decimal.Parse(recordParametersFromFile[5], CultureInfo.InvariantCulture);
                record.YearsOfService = short.Parse(recordParametersFromFile[6], CultureInfo.InvariantCulture);

                records.Add(record);
            }

            return records;
        }
    }
}