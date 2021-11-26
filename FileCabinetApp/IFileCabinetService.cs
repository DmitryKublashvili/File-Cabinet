﻿using System;
using System.Collections.ObjectModel;

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
        void EditRecord(ParametresOfRecord parametresOfRecord);

        /// <summary>
        /// Gets FileCabinetService state on current moment.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public IMemento<ReadOnlyCollection<FileCabinetRecord>> MakeSnapshot();

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>ReadOnlyCollection of records that have that birth date.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate);

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>ReadOnlyCollection of records that have that first name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>ReadOnlyCollection of records that have that last name.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Gets array of all records.
        /// </summary>
        /// <returns>ReadOnlyCollection of all records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count.</returns>
        int GetStat();
    }
}