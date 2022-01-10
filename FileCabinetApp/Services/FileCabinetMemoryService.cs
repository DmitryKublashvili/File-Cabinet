using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements functions of file cabinet.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        private readonly Dictionary<int, List<FileCabinetRecord>> idDictionary = new Dictionary<int, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly Dictionary<char, List<FileCabinetRecord>> sexDictionary = new Dictionary<char, List<FileCabinetRecord>>();
        private readonly Dictionary<decimal, List<FileCabinetRecord>> salaryDictionary = new Dictionary<decimal, List<FileCabinetRecord>>();
        private readonly Dictionary<short, List<FileCabinetRecord>> yearsOfServiceDictionary = new Dictionary<short, List<FileCabinetRecord>>();

        private readonly IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        /// <summary>
        /// Gets FileCabinetService state on current moment.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            FileCabinetRecord[] separateListOfRecords = new FileCabinetRecord[this.list.Count];

            this.list.CopyTo(separateListOfRecords);

            var tempList = new List<FileCabinetRecord>(separateListOfRecords);

            return new FileCabinetServiceSnapshot(new ReadOnlyCollection<FileCabinetRecord>(tempList));
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord.</param>
        /// <returns>New created record.</returns>
        public int CreateRecord(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            this.recordValidator.ValidateParameters(parametresOfRecord);

            var firstName = parametresOfRecord.FirstName;
            var lastName = parametresOfRecord.LastName;
            var dateOfBirth = parametresOfRecord.DateOfBirth;
            var sex = parametresOfRecord.Sex;
            var salary = parametresOfRecord.Salary;
            var yearsOfService = parametresOfRecord.YearsOfService;

            int id;

            if (parametresOfRecord.Id == -1)
            {
                for (int i = 1; ; i++)
                {
                    if (!this.IsRecordExist(i))
                    {
                        id = i;
                        break;
                    }
                }
            }
            else
            {
                if (this.IsRecordExist(parametresOfRecord.Id))
                {
                    Console.WriteLine($"Record {parametresOfRecord.Id} already exist..");
                    return -1;
                }

                id = parametresOfRecord.Id;
            }

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Sex = sex,
                Salary = salary,
                YearsOfService = yearsOfService,
            };

            this.list.Add(record);

            this.AddRecordToAllDictionaries(record);

            return record.Id;
        }

        /// <summary>
        /// Gets ReadOnlyCollection of all records.
        /// </summary>
        /// <returns>IRecordIterator of all records.</returns>
        public IRecordIterator GetRecords()
        {
            return new MemoryIterator(this.list.ToArray());
        }

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count and formal parameter for interface implementation.</returns>
        public (int recordsCount, int deletedRecordsCount) GetStat()
        {
            return (this.list.Count, 0);
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>IEnumerable of records that have that first name.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Temp. solution in developing process.")]
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                foreach (var record in this.firstNameDictionary[firstName])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>IEnumerable of records that have that last name.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Temp. solution in developing process.")]
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                foreach (var record in this.lastNameDictionary[lastName])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>IEnumerable of records that have that birth date.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            if (this.dateOfBirthDictionary.ContainsKey(searchingDate))
            {
                foreach (var record in this.dateOfBirthDictionary[searchingDate])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Removes all records where specified parameter equals argument value.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <typeparam name="T">Type of parameter.</typeparam>
        /// <returns>Array of Ids deleted records.</returns>
        public int[] RemoveAllRecordsByParameter<T>(RecordParameter parameterName, T parameterValue)
        {
            FileCabinetRecord[] recordsToDelete = this.list.Where(record => ((T)record
            .GetType()
            .GetProperty(parameterName.ToString())
            .GetValue(record)).Equals(parameterValue)).ToArray();

            foreach (var record in recordsToDelete)
            {
                this.list.Remove(record);
            }

            this.RemoveRecordsFromDictionaries(recordsToDelete);

            return recordsToDelete.Select(record => record.Id).ToArray();
        }

        /// <summary>
        /// Apdates all records where specified parameters equals argument values.
        /// </summary>
        /// <typeparam name="T">Types of parameters.</typeparam>
        /// <param name="dataForSearch">Parameters names and their values to search records.</param>
        /// <param name="dataForUpdate">Parameters names and their values to apdate records.</param>
        /// <returns>Array of Ids deleted records.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Temp. solution in developing process.")]
        public int[] UpdateRecordsByParameters<T>((RecordParameter parameter, T value)[] dataForSearch, (RecordParameter parameter, T value)[] dataForUpdate)
        {
            List<FileCabinetRecord> recordsToUpdate = this.list;

            // find records
            for (int i = 0; i < dataForSearch.Length; i++)
            {
                recordsToUpdate = recordsToUpdate.Where(record => ((T)record
                .GetType()
                .GetProperty(dataForSearch[i].parameter.ToString())
                .GetValue(record)).Equals(dataForSearch[i].value)).ToList();
            }

            this.RemoveRecordsFromDictionaries(recordsToUpdate.ToArray());

            // update records
            for (int i = 0; i < dataForUpdate.Length; i++)
            {
                for (int j = 0; j < recordsToUpdate.Count; j++)
                {
                    recordsToUpdate[j].GetType().GetProperty(dataForUpdate[i].parameter.ToString()).SetValue(recordsToUpdate[j], dataForUpdate[i].value);
                }
            }

            foreach (var record in recordsToUpdate)
            {
                this.recordValidator.ValidateParameters(new ParametresOfRecord(record));
                this.AddRecordToAllDictionaries(record);
            }

            return recordsToUpdate.Select(record => record.Id).ToArray();
        }

        /// <summary>
        /// Define is the record exists by it's ID.
        /// </summary>
        /// <param name="id">ID of the record.</param>
        /// <returns>Result bool value.</returns>
        public bool IsRecordExist(int id) => this.list.Exists(x => x.Id == id);

        /// <summary>
        /// Defragments storage file by removing marked as deleted records.
        /// </summary>
        /// <returns>Is defragmentation completed successfully.</returns>
        public bool Defragment()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Restores state according current state and addition state from snapshot.
        /// </summary>
        /// <param name="snapShot">Snapshot with some addition or new state.</param>
        /// <returns>Information (id, message) about not valid records.</returns>
        public IEnumerable<(int id, string exceptionMessage)> Restore(FileCabinetServiceSnapshot snapShot)
        {
            if (snapShot is null)
            {
                throw new ArgumentException("Snapshot was null", nameof(snapShot));
            }

            ReadOnlyCollection<FileCabinetRecord> newRecords = snapShot.GetState();

            List<int> existingRecordsIds = this.list.Select(r => r.Id).ToList();

            List<(int id, string exceptionMessage)> validationViolations = new List<(int id, string exceptionMessage)>();

            for (int i = 0; i < newRecords.Count; i++)
            {
                try
                {
                    this.recordValidator.ValidateParameters(new ParametresOfRecord(newRecords[i]));
                }
                catch (ValidationException e)
                {
                    validationViolations.Add((e.NotValidRecordId, e.Message));
                    continue;
                }

                int indexOfRecordWithSameId = existingRecordsIds.IndexOf(newRecords[i].Id);

                if (indexOfRecordWithSameId >= 0)
                {
                    this.RemoveRecordsFromDictionaries(new FileCabinetRecord[1] { newRecords[i] });

                    this.list[indexOfRecordWithSameId] = newRecords[i];
                }
                else
                {
                    this.list.Add(newRecords[i]);
                    this.AddRecordToAllDictionaries(newRecords[i]);
                }
            }

            return validationViolations;
        }

        private static void RemoveEmptyValuesFromConcreteDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary)
        {
            T[] keys = dictionary.Where(x => x.Value.Count == 0).Select(x => x.Key).ToArray();

            foreach (var key in keys)
            {
                dictionary.Remove(key);
            }
        }

        private static void AddRecordToConcreteDictionary<T>(Dictionary<T, List<FileCabinetRecord>> dictionary, T key, FileCabinetRecord record)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(record);
            }
            else
            {
                dictionary.Add(key: key, new List<FileCabinetRecord>() { record });
            }
        }

        private void RemoveRecordsFromDictionaries(FileCabinetRecord[] records)
        {
            foreach (var record in records)
            {
                this.idDictionary[record.Id].Remove(record);
                this.firstNameDictionary[record.FirstName].Remove(record);
                this.lastNameDictionary[record.LastName].Remove(record);
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                this.sexDictionary[record.Sex].Remove(record);
                this.salaryDictionary[record.Salary].Remove(record);
                this.yearsOfServiceDictionary[record.YearsOfService].Remove(record);
            }

            this.RemoveEmptyValuesFromAllDictionaries();
        }

        private void RemoveEmptyValuesFromAllDictionaries()
        {
            RemoveEmptyValuesFromConcreteDictionary(this.idDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.firstNameDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.lastNameDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.dateOfBirthDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.sexDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.salaryDictionary);
            RemoveEmptyValuesFromConcreteDictionary(this.yearsOfServiceDictionary);
        }

        private void AddRecordToAllDictionaries(FileCabinetRecord record)
        {
            AddRecordToConcreteDictionary(this.idDictionary, record.Id, record);
            AddRecordToConcreteDictionary(this.firstNameDictionary, record.FirstName, record);
            AddRecordToConcreteDictionary(this.lastNameDictionary, record.LastName, record);
            AddRecordToConcreteDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);
            AddRecordToConcreteDictionary(this.sexDictionary, record.Sex, record);
            AddRecordToConcreteDictionary(this.salaryDictionary, record.Salary, record);
            AddRecordToConcreteDictionary(this.yearsOfServiceDictionary, record.YearsOfService, record);
        }
    }
}