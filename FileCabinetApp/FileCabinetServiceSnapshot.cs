using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Creates FileCabinetService state on current moment.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private DateTime date;
        private string name;
        private ReadOnlyCollection<FileCabinetRecord> state;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.date = DateTime.Now;
            this.name = "Empty snapshot";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="state">Takes Read Only Collection of FileCabinetRecords.</param>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            this.state = state;

            this.DateAndNameInitialisation();
        }

        /// <summary>
        /// Gets state on the moment of fixation.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Gets date and time of state fixation.
        /// </summary>
        /// <returns>Date and time of state fixation.</returns>
        public DateTime GetDate()
        {
            return this.date;
        }

        /// <summary>
        /// Gets name of state fixation.
        /// </summary>
        /// <returns>Name of state fixation.</returns>
        public string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Saves state to CSV file.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        public void SaveToCSV(TextWriter textWriter)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(textWriter);

            csvWriter.Write(this);
        }

        /// <summary>
        /// Loads state from CSV file.
        /// </summary>
        /// <param name="textReader">Text writer.</param>
        public void LoadFromCSV(TextReader textReader)
        {
            FileCabinetRecordCsvReader reader = new FileCabinetRecordCsvReader(textReader);

            this.state = new ReadOnlyCollection<FileCabinetRecord>(reader.ReadAll());

            this.DateAndNameInitialisation();
        }

        /// <summary>
        /// Loads state from XML file.
        /// </summary>
        /// <param name="textReader">Text writer.</param>
        public void LoadFromXML(TextReader textReader)
        {
            textReader.Close();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves state to XML file.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        public void SaveToXML(TextWriter textWriter)
        {
            FileCabinetRecordXmlWriter xmlWriter = new FileCabinetRecordXmlWriter(textWriter);

            xmlWriter.Write(this);
        }

        private void DateAndNameInitialisation()
        {
            this.date = DateTime.Now;

            string additionToSnapshotName = this.state.Count > 0 ? this.state[^1].Id.ToString(CultureInfo.InvariantCulture) : "No records.";

            this.name = this.date + " last records Id is: " + additionToSnapshotName;
        }
    }
}