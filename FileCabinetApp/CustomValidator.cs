using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator.
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
        public int MaxLettersCountInName { get; } = 20;

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

            var id = parametresOfRecord.Id;
            var firstName = parametresOfRecord.FirstName;
            var lastName = parametresOfRecord.LastName;
            var dateOfBirth = parametresOfRecord.DateOfBirth;
            var sex = parametresOfRecord.Sex;
            var salary = parametresOfRecord.Salary;
            var yearsOfService = parametresOfRecord.YearsOfService;

            this.NamesValidation(id, firstName, nameof(firstName));
            this.NamesValidation(id, lastName, nameof(lastName));
            this.DateOfBirthValidation(id, dateOfBirth, nameof(dateOfBirth));
            SexValidation(id, sex, nameof(sex));
            this.SalaryValidation(id, salary, nameof(salary));
            this.YearsOfServiceValidation(id, yearsOfService, nameof(yearsOfService));
        }

        private static void SexValidation(int id, char sex, string nameOfParameter)
        {
            if (char.ToUpperInvariant(sex) != 'M' && char.ToUpperInvariant(sex) != 'F')
            {
                throw new ValidationException(id, IncorrectSexMessage, nameOfParameter);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "This method checks severak parameters.")]
        private void NamesValidation(int id, string name, string nameOfParameter)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(id, nameOfParameter, NotValidEmptyNameMessage);
            }

            if (name.Length < this.MinLettersCountInName || name.Length > this.MaxLettersCountInName)
            {
                throw new ValidationException(id, $"The name must be {this.MinLettersCountInName}-{this.MaxLettersCountInName} characters long", nameOfParameter);
            }

            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                {
                    throw new ValidationException(id, NotValidSimbolsInNameMessage, nameOfParameter);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        private void DateOfBirthValidation(int id, DateTime dateOfBirth, string nameOfParameter)
        {
            if (dateOfBirth < this.MinDateOfBirth || dateOfBirth > DateTime.Now)
            {
                throw new ValidationException(id, $"Date of birth must be no earlier than {this.MinDateOfBirth} " +
                    "and no later than the current date", nameOfParameter);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        private void SalaryValidation(int id, decimal salary, string nameOfParameter)
        {
            if (salary < this.MinSalary || salary > this.MaxSalary)
            {
                throw new ValidationException(id, $"The salary must be greater than or equal to {this.MinSalary} " +
                    $"and less than or equal to {this.MaxSalary}", nameOfParameter);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        private void YearsOfServiceValidation(int id, short yearsOfService, string nameOfParameter)
        {
            if (yearsOfService < this.MinYearsOfService || yearsOfService > this.MaxYearsOfService)
            {
                throw new ValidationException(id, $"The years of service parameter must be greater than or equal to {this.MinYearsOfService} " +
                    $"and less than or equal to {this.MaxYearsOfService}", nameOfParameter);
            }
        }
    }
}
