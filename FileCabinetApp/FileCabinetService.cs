using System;
using System.Collections.Generic;

public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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

        return record.Id;
    }

    public FileCabinetRecord[] GetRecords()
    {
        return this.list.ToArray();
    }

    public int GetStat()
    {
        return this.list.Count;
    }

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
                this.list[i].FirstName = firstName;
                this.list[i].LastName = lastName;
                this.list[i].DateOfBirth = dateOfBirth;
                this.list[i].Sex = sex;
                this.list[i].Salary = salary;
                return;
            }
        }

        throw new ArgumentException($"{id} record is not found.");
    }

    public FileCabinetRecord[] FindByFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException("first name was null or empty", nameof(firstName));
        }

        firstName = firstName.ToUpperInvariant();

        List<FileCabinetRecord> findingRecords = new List<FileCabinetRecord>();

        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list[i].FirstName.ToUpperInvariant().Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
            {
                findingRecords.Add(this.list[i]);
            }
        }

        return findingRecords.ToArray();
    }

    public FileCabinetRecord[] FindByLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            throw new ArgumentException("first name was null or empty", nameof(lastName));
        }

        lastName = lastName.ToUpperInvariant();

        List<FileCabinetRecord> findingRecords = new List<FileCabinetRecord>();

        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list[i].LastName.ToUpperInvariant().Equals(lastName, StringComparison.InvariantCultureIgnoreCase))
            {
                findingRecords.Add(this.list[i]);
            }
        }

        return findingRecords.ToArray();
    }

    private static void NamesValidation(string name, string nameOfParameter)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameOfParameter, "The name must not be null or contain only spaces");
        }

        if (name.Length is < 2 or > 60)
        {
            throw new ArgumentException("The first name must be 2-60 characters long", nameOfParameter);
        }

        for (int i = 0; i < name.Length; i++)
        {
            if (!char.IsLetter(name[i]))
            {
                throw new ArgumentException("The name must consists of only letters", nameOfParameter);
            }
        }
    }

    private static void DateOfBirthValidation(DateTime dateOfBirth, string nameOfParameter)
    {
        DateTime minDate = new (1950, 1, 1);

        if (dateOfBirth < minDate || dateOfBirth > DateTime.Now)
        {
            throw new ArgumentException(
                "Date of birth must be no earlier than January 1, 1950 " +
                "and no later than the current date", nameOfParameter);
        }
    }

    private static void SexValidation(char sex, string nameOfParameter)
    {
        if (char.ToUpperInvariant(sex) != 'M' && char.ToUpperInvariant(sex) != 'F')
        {
            throw new ArgumentException("Only the letters 'M' or 'F' are valid", nameOfParameter);
        }
    }

    private static void SalaryValidation(decimal salary, string nameOfParameter)
    {
        if (salary < 2_000 || salary > 100_000)
        {
            throw new ArgumentException(
                "The salary must be greater than or equal to 2 000 " +
                "and less than or equal to 100 000", nameOfParameter);
        }
    }

    private static void YearsOfServiceValidation(short yearsOfService, string nameOfParameter)
    {
        if (yearsOfService < 0 || yearsOfService > 50)
        {
            throw new ArgumentException(
                "The years of service parameter must be greater than or equal to 0 " +
                "and less than or equal to 50", nameOfParameter);
        }
    }
}