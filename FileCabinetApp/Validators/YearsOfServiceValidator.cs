using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom YearsOfService Validator.
    /// </summary>
    public class YearsOfServiceValidator : IRecordValidator
    {
        private readonly int minYearsOfService;
        private readonly int maxYearsOfService;

        /// <summary>
        /// Initializes a new instance of the <see cref="YearsOfServiceValidator"/> class.
        /// </summary>
        /// <param name="minYearsOfService">min YearsOfService.</param>
        /// <param name="maxYearsOfService">max YearsOfService.</param>
        public YearsOfServiceValidator(int minYearsOfService, int maxYearsOfService)
        {
            this.minYearsOfService = minYearsOfService;
            this.maxYearsOfService = maxYearsOfService;
        }

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

            if (yearsOfService < this.minYearsOfService || yearsOfService > this.maxYearsOfService)
            {
                throw new ValidationException(parametresOfRecord.Id, $"The years of service parameter must be greater than or equal to {this.minYearsOfService} " +
                    $"and less than or equal to {this.maxYearsOfService}", nameof(yearsOfService));
            }
        }
    }
}
