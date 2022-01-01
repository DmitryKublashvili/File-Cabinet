using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Default YearsOfService Validator.
    /// </summary>
    public class DefaultYearsOfServiceValidator : IRecordValidator
    {
        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MinYearsOfService { get; } = 5;

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MaxYearsOfService { get; } = 50;

        /// <summary>
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "This method checks severak parameters.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It is custom exception")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var yearsOfService = parametresOfRecord.YearsOfService;

            if (yearsOfService < this.MinYearsOfService || yearsOfService > this.MaxYearsOfService)
            {
                throw new ValidationException(parametresOfRecord.Id, $"The years of service parameter must be greater than or equal to {this.MinYearsOfService} " +
                    $"and less than or equal to {this.MaxYearsOfService}", nameof(yearsOfService));
            }
        }
    }
}
