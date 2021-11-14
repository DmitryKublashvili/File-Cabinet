using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Parametres validator.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
        private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";
        private const string IncorrectSexMessage = "Only the letters 'M' or 'F' are valid";

        /// <summary>
        /// Gets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MinLettersCountInName { get; } = 1;

        /// <summary>
        /// Gets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MaxLettersCountInName { get; } = 30;

        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MinSalary { get; } = 5_000;

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MaxSalary { get; } = 50_000;

        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MinYearsOfService { get; } = 1;

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MaxYearsOfService { get; } = 30;

        /// <summary>
        /// Gets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public DateTime MinDateOfBirth { get; } = new (1970, 1, 1);

        /// <summary>
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var firstName = parametresOfRecord.FirstName;
            var lastName = parametresOfRecord.LastName;
            var dateOfBirth = parametresOfRecord.DateOfBirth;
            var sex = parametresOfRecord.Sex;
            var salary = parametresOfRecord.Salary;
            var yearsOfService = parametresOfRecord.YearsOfService;

            this.NamesValidation(firstName, nameof(firstName));
            this.NamesValidation(lastName, nameof(lastName));
            this.DateOfBirthValidation(dateOfBirth, nameof(dateOfBirth));
            SexValidation(sex, nameof(sex));
            this.SalaryValidation(salary, nameof(salary));
            this.YearsOfServiceValidation(yearsOfService, nameof(yearsOfService));
        }

        private static void SexValidation(char sex, string nameOfParameter)
        {
            if (char.ToUpperInvariant(sex) != 'M' && char.ToUpperInvariant(sex) != 'F')
            {
                throw new ArgumentException(IncorrectSexMessage, nameOfParameter);
            }
        }

        private void NamesValidation(string name, string nameOfParameter)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameOfParameter, NotValidEmptyNameMessage);
            }

            if (name.Length < this.MinLettersCountInName || name.Length > this.MaxLettersCountInName)
            {
                throw new ArgumentException($"The name must be {this.MinLettersCountInName}-{this.MaxLettersCountInName} characters long", nameOfParameter);
            }

            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                {
                    throw new ArgumentException(NotValidSimbolsInNameMessage, nameOfParameter);
                }
            }
        }

        private void DateOfBirthValidation(DateTime dateOfBirth, string nameOfParameter)
        {
            if (dateOfBirth < this.MinDateOfBirth || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException(
                    $"Date of birth must be no earlier than {this.MinDateOfBirth} " +
                    "and no later than the current date", nameOfParameter);
            }
        }

        private void SalaryValidation(decimal salary, string nameOfParameter)
        {
            if (salary < this.MinSalary || salary > this.MaxSalary)
            {
                throw new ArgumentException(
                    $"The salary must be greater than or equal to {this.MinSalary} " +
                    $"and less than or equal to {this.MaxSalary}", nameOfParameter);
            }
        }

        private void YearsOfServiceValidation(short yearsOfService, string nameOfParameter)
        {
            if (yearsOfService < this.MinYearsOfService || yearsOfService > this.MaxYearsOfService)
            {
                throw new ArgumentException(
                    $"The years of service parameter must be greater than or equal to {this.MinYearsOfService} " +
                    $"and less than or equal to {this.MaxYearsOfService}", nameOfParameter);
            }
        }
    }
}
