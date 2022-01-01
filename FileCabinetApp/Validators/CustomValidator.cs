using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Gets or sets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MinLettersCountInName { get; set; } = 1;

        /// <summary>
        /// Gets or sets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MaxLettersCountInName { get; set;  } = 20;

        /// <summary>
        /// Gets or sets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public DateTime MinDateOfBirth { get; set; } = new (1970, 1, 1);

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
        public int MaxSalary { get; } = 70_000;

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
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            new CompositeValidator(new List<IRecordValidator>()
            {
                new FirstNameValidator(this.MinLettersCountInName, this.MaxLettersCountInName),
                new LastNameValidator(this.MinLettersCountInName, this.MaxLettersCountInName),
                new DateOfBirthValidator(this.MinDateOfBirth),
                new SexValidator(),
                new SalaryValidator(this.MinSalary, this.MaxSalary),
                new YearsOfServiceValidator(this.MinYearsOfService, this.MaxYearsOfService),
            }).ValidateParameters(parametresOfRecord);
        }
    }
}
