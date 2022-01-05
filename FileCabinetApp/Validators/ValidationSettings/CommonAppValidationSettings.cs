using System;

namespace FileCabinetApp.Validators.ValidationSettings
{
    /// <summary>
    /// Common Validation Settings.
    /// </summary>
    public class CommonAppValidationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonAppValidationSettings"/> class.
        /// </summary>
        public CommonAppValidationSettings()
        {
            this.Default = new Default();
            this.Custom = new Custom();
        }

        public Default Default { get; set; }

        public Custom Custom { get; set; }
    }

    public class NameValidationRules
    {
        /// <summary>
        /// Gets or sets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MinLettersCountInName { get; set; } = 2;

        /// <summary>
        /// Gets or sets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MaxLettersCountInName { get; set; } = 60;
    }

    public class DateOfBirthValidationRules
    {
        /// <summary>
        /// Gets or sets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public DateTime MinDateOfBirth { get; set; } = new(1950, 1, 1);
    }

    public class SalaryValidationRules
    {
        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public decimal MinSalary { get; set; } = 2_000;

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public decimal MaxSalary { get; set; } = 100_000;
    }

    public class YearsOfServiceValidationRules
    {
        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MinYearsOfService { get; set; } = 2;

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MaxYearsOfService { get; set; } = 42;
    }

    public class Default
    {
        public Default()
        {
            FirstName = new NameValidationRules();
            LastName = new NameValidationRules();
            DateOfBirth = new DateOfBirthValidationRules();
            Salary = new SalaryValidationRules();
            YearsOfService = new YearsOfServiceValidationRules();
        }

        public NameValidationRules FirstName { get; set; }

        public NameValidationRules LastName { get; set; }

        public DateOfBirthValidationRules DateOfBirth { get; set; }

        public SalaryValidationRules Salary { get; set; }

        public YearsOfServiceValidationRules YearsOfService { get; set; }
    }

    public class Custom
    {
        public Custom()
        {
            FirstName = new NameValidationRules();
            LastName = new NameValidationRules();
            DateOfBirth = new DateOfBirthValidationRules();
            Salary = new SalaryValidationRules();
            YearsOfService = new YearsOfServiceValidationRules();
        }

        public NameValidationRules FirstName { get; set; }

        public NameValidationRules LastName { get; set; }

        public DateOfBirthValidationRules DateOfBirth { get; set; }

        public SalaryValidationRules Salary { get; set; }

        public YearsOfServiceValidationRules YearsOfService { get; set; }
    }
}
