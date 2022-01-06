using System;
using System.Collections.Generic;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Enumerates the sequence of records from file system service.
    /// </summary>
    public class MemoryIterator : IRecordIterator
    {
        private readonly FileCabinetRecord[] records;

        private int indexOfCurrentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">Records array.</param>
        public MemoryIterator(FileCabinetRecord[] records)
        {
            this.records = records;
            this.indexOfCurrentPosition = -1;
        }

        /// <inheritdoc/>
        public FileCabinetRecord GetNext()
        {
            if (!this.HasMore())
            {
                throw new InvalidOperationException();
            }

            this.indexOfCurrentPosition++;

            return this.records[this.indexOfCurrentPosition];
        }

        /// <inheritdoc/>
        public bool HasMore()
        {
            return this.indexOfCurrentPosition < this.records.Length;
        }
    }
}
