using System;
using System.Collections.Generic;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Enumerates the sequence of records from file system service.
    /// </summary>
    public class FileSystemIterator : IRecordIterator
    {
        private readonly FileCabinetFilesystemService service;

        private readonly List<long> positions;

        private int indexOfCurrentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemIterator"/> class.
        /// </summary>
        /// <param name="service">FileCabinetFilesystemService.</param>
        /// <param name="positions">Positions of records in file.</param>
        public FileSystemIterator(FileCabinetFilesystemService service, List<long> positions)
        {
            this.service = service;
            this.positions = positions;
            this.indexOfCurrentPosition = -1;
        }

        /// <summary>
        /// Gets next position of record in file.
        /// </summary>
        /// <value>
        /// Next position of record in file.
        /// </value>
        public long NextPosition { get => this.positions[this.indexOfCurrentPosition + 1]; }

        /// <inheritdoc/>
        public FileCabinetRecord GetNext()
        {
            if (!this.HasMore())
            {
                throw new InvalidOperationException();
            }

            this.indexOfCurrentPosition++;

            return this.service.GetRecordFromFile(this.positions[this.indexOfCurrentPosition]);
        }

        /// <inheritdoc/>
        public bool HasMore()
        {
            return this.indexOfCurrentPosition < this.positions.Count - 1;
        }
    }
}
