using System;
using System.Collections.Generic;

public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

    public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char sex, decimal accountBalance)
    {
        NamesValidation(firstName, nameof(firstName));
        NamesValidation(lastName, nameof(lastName));
        DateOfBirthValidation(dateOfBirth, nameof(dateOfBirth));
        SexValidation(sex, nameof(sex));
        AccountBalanceValidation(accountBalance, nameof(accountBalance));

        var record = new FileCabinetRecord
        {
            Id = this.list.Count + 1,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Sex = sex,
            AccountBalance = accountBalance,
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
    }

    private static void DateOfBirthValidation(DateTime dateOfBirth, string nameOfParameter)
    {
        DateTime checkingDate = new (1950, 1, 1);

        if (dateOfBirth < checkingDate || dateOfBirth > DateTime.Now)
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

    private static void AccountBalanceValidation(decimal accountBalance, string nameOfParameter)
    {
        if (accountBalance < -50_000 || accountBalance > 1_000_000_000)
        {
            throw new ArgumentException(
                "The account balance must be greater than or equal to -50,000 " +
                "and less than or equal to 1,000,000,000", nameOfParameter);
        }
    }
}