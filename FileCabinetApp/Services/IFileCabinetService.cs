using System;
using System.Collections.Generic;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Defines functions of file cabinet.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord.</param>
        /// <returns>New created record.</returns>
        int CreateRecord(ParametresOfRecord parametresOfRecord);

        /// <summary>
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        /// <returns>Is edition completed successfully.</returns>
        bool EditRecord(ParametresOfRecord parametresOfRecord);

        /// <summary>
        /// Gets FileCabinetService state on current moment.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>IRecordIterator of records that have that birth date.</returns>
        IRecordIterator FindByDateOfBirth(DateTime searchingDate);

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>IRecordIterator of records that have that first name.</returns>
        IRecordIterator FindByFirstName(string firstName);

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>IRecordIterator of records that have that last name.</returns>
        IRecordIterator FindByLastName(string lastName);

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>IRecordIterator of all records.</returns>
        IRecordIterator GetRecords();

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Exosting records count and deleted records count.</returns>
        (int recordsCount, int deletedRecordsCount) GetStat();

        /// <summary>
        /// Restores state according current state and addition state from snapshot.
        /// </summary>
        /// <param name="snapShot">Snapshot with some addition or new state.</param>
        /// <returns>Information (id, message) about not valid records.</returns>
        IEnumerable<(int id, string exceptionMessage)> Restore(FileCabinetServiceSnapshot snapShot);

        /// <summary>
        /// Removes record by it's ID.
        /// </summary>
        /// <param name="id">ID of record.</param>
        /// <returns>Is removing completed successfully.</returns>
        bool RemoveRecordById(int id);

        /// <summary>
        /// Define is the record exists by it's ID.
        /// </summary>
        /// <param name="id">ID of the record.</param>
        /// <returns>Result bool value.</returns>
        bool IsRecordExist(int id);

        /// <summary>
        /// Defragments storage file by removing marked as deleted records.
        /// </summary>
        /// <returns>Is defragmentation completed successfully.</returns>
        bool Defragment();
    }
}