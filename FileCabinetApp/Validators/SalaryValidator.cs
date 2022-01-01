using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom Salary Validator.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private readonly int minSalary;
        private readonly int maxSalary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="minSalary">min Salary.</param>
        /// <param name="maxSalary">max Salary.</param>
        public SalaryValidator(int minSalary, int maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It is custom exception.")]
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var salary = parametresOfRecord.Salary;

            if (salary < this.minSalary || salary > this.maxSalary)
            {
                throw new ValidationException(parametresOfRecord.Id, $"The salary must be greater than or equal to {this.minSalary} " +
                    $"and less than or equal to {this.maxSalary}", nameof(salary));
            }
        }
    }
}