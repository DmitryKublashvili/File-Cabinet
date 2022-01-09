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
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        /// <returns>Is edition completed successfully.</returns>
        public bool EditRecord(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            this.recordValidator.ValidateParameters(parametresOfRecord);

            var id = parametresOfRecord.Id;
            var firstName = parametresOfRecord.FirstName;
            var lastName = parametresOfRecord.LastName;
            var dateOfBirth = parametresOfRecord.DateOfBirth;
            var sex = parametresOfRecord.Sex;
            var salary = parametresOfRecord.Salary;
            var yearsOfService = parametresOfRecord.YearsOfService;

            for (int i = 0; i < this.list.Count; i++)
            {
                if (id == this.list[i].Id)
                {
                    string previousFirstname = this.list[i].FirstName.ToUpperInvariant();
                    string previousLastname = this.list[i].LastName.ToUpperInvariant();
                    DateTime previousDateOfBirth = this.list[i].DateOfBirth;

                    this.list[i].FirstName = firstName;
                    this.list[i].LastName = lastName;
                    this.list[i].DateOfBirth = dateOfBirth;
                    this.list[i].Sex = sex;
                    this.list[i].Salary = salary;
                    this.list[i].YearsOfService = yearsOfService;

                    this.ReplaceRecordInDictionaries(previousFirstname, previousLastname, previousDateOfBirth, this.list[i]);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>IEnumerable of records that have that first name.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            firstName = firstName.ToUpperInvariant();

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
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            lastName = lastName.ToUpperInvariant();

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
        /// <returns>Bool execution result.</returns>
        public int[] RemoveAllRecordsByParameter<T>(RecordParameter parameterName, T parameterValue)
        {
            FileCabinetRecord[] recordsToDelete = this.list.Where(record => ((T)record
            .GetType()
            .GetProperty(parameterName.ToString())
            .GetValue(record)).Equals(parameterValue)).ToArray();

            foreach (var record in recordsToDelete)
            {
                this.list.Remove(record);
                this.idDictionary[record.Id].Remove(record);
                this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
                this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                this.sexDictionary[record.Sex].Remove(record);
                this.salaryDictionary[record.Salary].Remove(record);
                this.yearsOfServiceDictionary[record.YearsOfService].Remove(record);
            }

            this.RemoveEmptyValuesFromAllDictionaries();

            return recordsToDelete.Select(record => record.Id).ToArray();
        }

        /// <summary>
        /// Removes record by it's ID.
        /// </summary>
        /// <param name="id">ID of record.</param>
        /// <returns>Is removing completed successfully.</returns>
        public bool RemoveRecordById(int id)
        {
            int indexORemovingRecord = this.list.FindIndex(x => x.Id == id);

            if (indexORemovingRecord == -1)
            {
                return false;
            }

            var removingRecord = this.list[indexORemovingRecord];
            this.list.RemoveAt(indexORemovingRecord);
            this.RemoveRecordFromDictionaries(removingRecord);

            return true;
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
                    string previousFirstname = this.list[indexOfRecordWithSameId].FirstName.ToUpperInvariant();
                    string previousLastname = this.list[indexOfRecordWithSameId].LastName.ToUpperInvariant();
                    DateTime previousDateOfBirth = this.list[indexOfRecordWithSameId].DateOfBirth;

                    this.list[indexOfRecordWithSameId] = newRecords[i];

                    this.ReplaceRecordInDictionaries(previousFirstname, previousLastname, previousDateOfBirth, this.list[indexOfRecordWithSameId]);
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
            IEnumerable<T> keys = dictionary.Where(x => x.Value.Count == 0).Select(x => x.Key);

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

        private Dictionary<T, List<FileCabinetRecord>> GetDictionaryByParameter<T>(RecordParameter parameterName)
        {
            switch (parameterName)
            {
                case RecordParameter.Id: return this.idDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.FirstName: return this.firstNameDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.LastName: return this.lastNameDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.DateOfBirth: return this.dateOfBirthDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.Sex: return this.sexDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.Salary: return this.salaryDictionary as Dictionary<T, List<FileCabinetRecord>>;
                case RecordParameter.YearsOfService: return this.yearsOfServiceDictionary as Dictionary<T, List<FileCabinetRecord>>;
                default: return new Dictionary<T, List<FileCabinetRecord>>();
            }
        }

        private void RemoveRecordFromDictionaries(FileCabinetRecord record)
        {
            if (this.idDictionary.ContainsKey(record.Id))
            {
                this.idDictionary[record.Id].Remove(record);
            }

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpperInvariant()))
            {
                this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
            }

            if (this.sexDictionary.ContainsKey(record.Sex))
            {
                this.sexDictionary[record.Sex].Remove(record);
            }

            if (this.salaryDictionary.ContainsKey(record.Salary))
            {
                this.salaryDictionary[record.Salary].Remove(record);
            }

            if (this.yearsOfServiceDictionary.ContainsKey(record.YearsOfService))
            {
                this.yearsOfServiceDictionary[record.YearsOfService].Remove(record);
            }
        }

        private void AddRecordToAllDictionaries(FileCabinetRecord record)
        {
            AddRecordToConcreteDictionary(this.idDictionary, record.Id, record);
            AddRecordToConcreteDictionary(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
            AddRecordToConcreteDictionary(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
            AddRecordToConcreteDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);
            AddRecordToConcreteDictionary(this.sexDictionary, record.Sex, record);
            AddRecordToConcreteDictionary(this.salaryDictionary, record.Salary, record);
            AddRecordToConcreteDictionary(this.yearsOfServiceDictionary, record.YearsOfService, record);
        }

        private void ReplaceRecordInDictionaries(string previousFirstname, string previousLastname, DateTime previousDateOfBirth, FileCabinetRecord editedRecord)
        {
            // adding changes in firstNameDictionary
            this.firstNameDictionary[previousFirstname].Remove(editedRecord);

            if (this.firstNameDictionary.ContainsKey(editedRecord.FirstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[editedRecord.FirstName.ToUpperInvariant()].Add(editedRecord);
            }
            else
            {
                this.firstNameDictionary.Add(key: editedRecord.FirstName.ToUpperInvariant(), new List<FileCabinetRecord>() { editedRecord });
            }

            // adding changes in lastNameDictionary
            this.lastNameDictionary[previousLastname].Remove(editedRecord);

            if (this.lastNameDictionary.ContainsKey(editedRecord.LastName.ToUpperInvariant()))
            {
                this.lastNameDictionary[editedRecord.LastName.ToUpperInvariant()].Add(editedRecord);
            }
            else
            {
                this.lastNameDictionary.Add(key: editedRecord.LastName.ToUpperInvariant(), new List<FileCabinetRecord>() { editedRecord });
            }

            // adding changes in lastNameDictionary
            this.dateOfBirthDictionary[previousDateOfBirth].Remove(editedRecord);

            if (this.dateOfBirthDictionary.ContainsKey(editedRecord.DateOfBirth))
            {
                this.dateOfBirthDictionary[editedRecord.DateOfBirth].Add(editedRecord);
            }
            else
            {
                this.dateOfBirthDictionary.Add(key: editedRecord.DateOfBirth, new List<FileCabinetRecord>() { editedRecord });
            }
        }
    }
}