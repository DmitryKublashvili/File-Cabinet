using System;
using System.Collections.Generic;

/// <summary>
/// Implements functions of file cabinet.
/// </summary>
public class FileCabinetService
{
    private const string FindingEmptyNameMessage = "Finding name was null or empty";
    private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
    private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";
    private const string IncorrectSexMessage = "Only the letters 'M' or 'F' are valid";

    private const int MinLettersCountInName = 2;
    private const int MaxLettersCountInName = 60;
    private const int MinSalary = 2_000;
    private const int MaxSalary = 100_000;
    private const int MinYearsOfService = 0;
    private const int MaxYearsOfService = 50;

    private static DateTime minDateOfBirth = new (1950, 1, 1);

    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
    private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

    /// <summary>
    /// Creates a record.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="sex">Sex of person.</param>
    /// <param name="salary">Salary.</param>
    /// <param name="yearsOfService">Count years of service.</param>
    /// <returns>New created record.</returns>
    public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char sex, decimal salary, short yearsOfService)
    {
        NamesValidation(firstName, nameof(firstName));
        NamesValidation(lastName, nameof(lastName));
        DateOfBirthValidation(dateOfBirth, nameof(dateOfBirth));
        SexValidation(sex, nameof(sex));
        SalaryValidation(salary, nameof(salary));
        YearsOfServiceValidation(yearsOfService, nameof(yearsOfService));

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
    /// Gets array of all records.
    /// </summary>
    /// <returns>Array of all records.</returns>
    public FileCabinetRecord[] GetRecords()
    {
        return this.list.ToArray();
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
    /// <param name="id">ID of edited record.</param>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="sex">Sex of person.</param>
    /// <param name="salary">Salary.</param>
    /// <param name="yearsOfService">Count years of service.</param>
    public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, decimal salary, short yearsOfService)
    {
        NamesValidation(firstName, nameof(firstName));
        NamesValidation(lastName, nameof(lastName));
        DateOfBirthValidation(dateOfBirth, nameof(dateOfBirth));
        SexValidation(sex, nameof(sex));
        SalaryValidation(salary, nameof(salary));
        YearsOfServiceValidation(yearsOfService, nameof(yearsOfService));

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
    /// Gets an array of records that have that first name.
    /// </summary>
    /// <param name="firstName">Search first name.</param>
    /// <returns>Array of records that have that first name.</returns>
    public FileCabinetRecord[] FindByFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException(FindingEmptyNameMessage, nameof(firstName));
        }

        firstName = firstName.ToUpperInvariant();

        if (this.firstNameDictionary.ContainsKey(firstName))
        {
            return this.firstNameDictionary[firstName].ToArray();
        }
        else
        {
            return Array.Empty<FileCabinetRecord>();
        }
    }

    /// <summary>
    /// Gets an array of records that have that last name.
    /// </summary>
    /// <param name="lastName">Search last name.</param>
    /// <returns>Array of records that have that last name.</returns>
    public FileCabinetRecord[] FindByLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentException(FindingEmptyNameMessage, nameof(lastName));
        }

        lastName = lastName.ToUpperInvariant();

        if (this.lastNameDictionary.ContainsKey(lastName))
        {
            return this.lastNameDictionary[lastName].ToArray();
        }
        else
        {
            return Array.Empty<FileCabinetRecord>();
        }
    }

    /// <summary>
    /// Gets an array of records that have that date of birth.
    /// </summary>
    /// <param name="date">Search birth date.</param>
    /// <returns>Array of records that have that birth date.</returns>
    public FileCabinetRecord[] FindByDateOfBirth(DateTime date)
    {
        if (this.dateOfBirthDictionary.ContainsKey(date))
        {
            return this.dateOfBirthDictionary[date].ToArray();
        }
        else
        {
            return Array.Empty<FileCabinetRecord>();
        }
    }

    private static void NamesValidation(string name, string nameOfParameter)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameOfParameter, NotValidEmptyNameMessage);
        }

        if (name.Length is < MinLettersCountInName or > MaxLettersCountInName)
        {
            throw new ArgumentException($"The name must be {MinLettersCountInName}-{MaxLettersCountInName} characters long", nameOfParameter);
        }

        for (int i = 0; i < name.Length; i++)
        {
            if (!char.IsLetter(name[i]))
            {
                throw new ArgumentException(NotValidSimbolsInNameMessage, nameOfParameter);
            }
        }
    }

    private static void DateOfBirthValidation(DateTime dateOfBirth, string nameOfParameter)
    {
        if (dateOfBirth < minDateOfBirth || dateOfBirth > DateTime.Now)
        {
            throw new ArgumentException(
                $"Date of birth must be no earlier than {minDateOfBirth} " +
                "and no later than the current date", nameOfParameter);
        }
    }

    private static void SexValidation(char sex, string nameOfParameter)
    {
        if (char.ToUpperInvariant(sex) != 'M' && char.ToUpperInvariant(sex) != 'F')
        {
            throw new ArgumentException(IncorrectSexMessage, nameOfParameter);
        }
    }

    private static void SalaryValidation(decimal salary, string nameOfParameter)
    {
        if (salary is < MinSalary or > MaxSalary)
        {
            throw new ArgumentException(
                $"The salary must be greater than or equal to {MinSalary} " +
                $"and less than or equal to {MaxSalary}", nameOfParameter);
        }
    }

    private static void YearsOfServiceValidation(short yearsOfService, string nameOfParameter)
    {
        if (yearsOfService < MinYearsOfService || yearsOfService > MaxYearsOfService)
        {
            throw new ArgumentException(
                $"The years of service parameter must be greater than or equal to {MinYearsOfService} " +
                $"and less than or equal to {MaxYearsOfService}", nameOfParameter);
        }
    }
}