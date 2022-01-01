using System;
using System.Collections.Generic;

namespace FileCabinetApp
{

    /// <summary>
    /// Validator Builder.
    /// </summary>
    public class ValidatorBuilder
    {
        /// <summary>
        /// Gets first name Validator.
        /// </summary>
        /// <value>
        /// First name Validator.
        /// </value>
        public FirstNameValidator FirstNameValidator { get; private set; }

        /// <summary>
        /// Gets last name Validator.
        /// </summary>
        /// <value>
        /// Last name Validator.
        /// </value>
        public LastNameValidator LastNameValidator { get; private set; }

        /// <summary>
        /// Gets date Of Birth Validator.
        /// </summary>
        /// <value>
        /// Date Of Birth Validator.
        /// </value>
        public DateOfBirthValidator DateOfBirthValidator { get; private set; }

        /// <summary>
        /// Gets sex Validator.
        /// </summary>
        /// <value>
        /// Sex Validator.
        /// </value>
        public SexValidator SexValidator { get; private set; }

        /// <summary>
        /// Gets salary Validator.
        /// </summary>
        /// <value>
        /// Salary Validator.
        /// </value>
        public SalaryValidator SalaryValidator { get; private set; }

        /// <summary>
        /// Gets years Of Service Validator.
        /// </summary>
        /// <value>
        /// Years Of Service Validator.
        /// </value>
        public YearsOfServiceValidator YearsOfServiceValidator { get; private set; }

        /// <summary>
        /// Initialise first name validator property.
        /// </summary>
        /// <param name="minLettersCountInName">Min Letters Count In Name.</param>
        /// <param name="maxLettersCountInName">Max Letters Count In Name.</param>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateFirstName(int minLettersCountInName, int maxLettersCountInName)
        {
            this.FirstNameValidator = new FirstNameValidator(minLettersCountInName, maxLettersCountInName);
            return this;
        }

        /// <summary>
        /// Initialise last name validator property.
        /// </summary>
        /// <param name="minLettersCountInName">Min Letters Count In Name.</param>
        /// <param name="maxLettersCountInName">Max Letters Count In Name.</param>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateLastName(int minLettersCountInName, int maxLettersCountInName)
        {
            this.LastNameValidator = new LastNameValidator(minLettersCountInName, maxLettersCountInName);
            return this;
        }

        /// <summary>
        /// Initialise date of birth validator property.
        /// </summary>
        /// <param name="minDateOfBirth">Min DateOfBirth.</param>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime minDateOfBirth)
        {
            this.DateOfBirthValidator = new DateOfBirthValidator(minDateOfBirth);
            return this;
        }

        /// <summary>
        /// Initialise sex validator property.
        /// </summary>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateSex()
        {
            this.SexValidator = new SexValidator();
            return this;
        }

        /// <summary>
        /// Initialise salary validator property.
        /// </summary>
        /// <param name="minSalary">Min Salary.</param>
        /// <param name="maxSalary">Max Salary.</param>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateSalary(decimal minSalary, decimal maxSalary)
        {
            this.SalaryValidator = new SalaryValidator(minSalary, maxSalary);
            return this;
        }

        /// <summary>
        /// Initialise YearsOfService validator property.
        /// </summary>
        /// <param name="minYearsOfService">Min Years Of Service.</param>
        /// <param name="maxYearsOfService">Max Years Of Service.</param>
        /// <returns>This builder.</returns>
        public ValidatorBuilder ValidateYearsOfService(int minYearsOfService, int maxYearsOfService)
        {
            this.YearsOfServiceValidator = new YearsOfServiceValidator(minYearsOfService, maxYearsOfService);
            return this;
        }

        /// <summary>
        /// Creates composite validator.
        /// </summary>
        /// <returns>CompositeValidator instance.</returns>
        public CompositeValidator Create()
        {
            return new CompositeValidator(new List<IRecordValidator>()
            {
                this.FirstNameValidator,
                this.LastNameValidator,
                this.DateOfBirthValidator,
                this.SexValidator,
                this.SalaryValidator,
                this.YearsOfServiceValidator,
            });
        }
    }
}
