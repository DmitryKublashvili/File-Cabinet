using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements functions of file cabinet.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        private readonly IRecordValidator recordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        public FileCabinetService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
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

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Sex = sex,
                Salary = salary,
                YearsOfService = yearsOfService,
            };

            this.list.Add(record);

            // adding in firstNameDictionary
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(key: firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }

            // adding in lastNameDictionary
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                this.lastNameDictionary[lastName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(key: lastName.ToUpperInvariant(), new List<FileCabinetRecord>() { record });
            }

            // adding in DateOfBirthDictionary
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(key: dateOfBirth, new List<FileCabinetRecord>() { record });
            }

            return record.Id;
        }

        /// <summary>
        /// Gets ReadOnlyCollection of all records.
        /// </summary>
        /// <returns>ReadOnlyCollection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void EditRecord(ParametresOfRecord parametresOfRecord)
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

                    // adding changes in firstNameDictionary
                    this.firstNameDictionary[previousFirstname].Remove(this.list[i]);

                    if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
                    {
                        this.firstNameDictionary[firstName.ToUpperInvariant()].Add(this.list[i]);
                    }
                    else
                    {
                        this.firstNameDictionary.Add(key: firstName.ToUpperInvariant(), new List<FileCabinetRecord>() { this.list[i] });
                    }

                    // adding changes in lastNameDictionary
                    this.lastNameDictionary[previousLastname].Remove(this.list[i]);

                    if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
                    {
                        this.lastNameDictionary[lastName.ToUpperInvariant()].Add(this.list[i]);
                    }
                    else
                    {
                        this.lastNameDictionary.Add(key: lastName.ToUpperInvariant(), new List<FileCabinetRecord>() { this.list[i] });
                    }

                    // adding changes in lastNameDictionary
                    this.dateOfBirthDictionary[previousDateOfBirth].Remove(this.list[i]);

                    if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
                    {
                        this.dateOfBirthDictionary[dateOfBirth].Add(this.list[i]);
                    }
                    else
                    {
                        this.dateOfBirthDictionary.Add(key: dateOfBirth, new List<FileCabinetRecord>() { this.list[i] });
                    }

                    return;
                }
            }

            throw new ArgumentException($"{id} record is not found.");
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>ReadOnlyCollection of records that have that first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("Wanted name was null or empty", nameof(firstName));
            }

            firstName = firstName.ToUpperInvariant();

            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>ReadOnlyCollection of records that have that last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Wanted name was null or empty", nameof(lastName));
            }

            lastName = lastName.ToUpperInvariant();

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="date">Search birth date.</param>
        /// <returns>ReadOnlyCollection of records that have that birth date.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime date)
        {
            if (this.dateOfBirthDictionary.ContainsKey(date))
            {
                return new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[date]);
            }
            else
            {
                return new ReadOnlyCollection<FileCabinetRecord>(Array.Empty<FileCabinetRecord>());
            }
        }
    }
}