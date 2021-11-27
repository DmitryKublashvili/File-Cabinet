using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements functions of file cabinet.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord.</param>
        /// <returns>New created record.</returns>
        public int CreateRecord(ParametresOfRecord parametresOfRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void EditRecord(ParametresOfRecord parametresOfRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>ReadOnlyCollection of records that have that birth date.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>ReadOnlyCollection of records that have that first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>ReadOnlyCollection of records that have that last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ReadOnlyCollection of all records.
        /// </summary>
        /// <returns>ReadOnlyCollection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets FileCabinetService state on current moment.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public IMemento<ReadOnlyCollection<FileCabinetRecord>> MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
