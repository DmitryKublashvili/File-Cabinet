using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Default Salary Validator.
    /// </summary>
    public class DefaultSalaryValidator : IRecordValidator
    {
        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MinSalary { get; } = 2_000;

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MaxSalary { get; } = 100_000;

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

            if (salary < this.MinSalary || salary > this.MaxSalary)
            {
                throw new ValidationException(parametresOfRecord.Id, $"The salary must be greater than or equal to {this.MinSalary} " +
                    $"and less than or equal to {this.MaxSalary}", nameof(salary));
            }
        }
    }
}