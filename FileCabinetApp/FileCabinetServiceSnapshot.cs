using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Creates FileCabinetService state on current moment.
    /// </summary>
    public class FileCabinetServiceSnapshot : IMemento<ReadOnlyCollection<FileCabinetRecord>>
    {
        private readonly DateTime date;
        private readonly string name;
        private readonly ReadOnlyCollection<FileCabinetRecord> state;

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

            this.date = DateTime.Now;

            this.name = this.date + " last records Id is: " + state[^1]?.Id ?? "No records.";

            this.state = state;
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
        /// Saves state to file.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        public void SaveToCSV(TextWriter textWriter)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(textWriter);

            csvWriter.Write(this);
        }
    }
}