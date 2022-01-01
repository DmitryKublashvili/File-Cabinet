using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom DateOfBirth Validator.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime minDateOfBirth;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="minDateOfBirth">Min DateOfBirth.</param>
        public DateOfBirthValidator(DateTime minDateOfBirth)
        {
            this.minDateOfBirth = minDateOfBirth;
        }

        /// <summary>
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "It done to comfort code visualisation.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It custom exception.")]
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var dateOfBirth = parametresOfRecord.DateOfBirth;

            if (dateOfBirth < this.minDateOfBirth || dateOfBirth > DateTime.Now)
            {
                throw new ValidationException(parametresOfRecord.Id, $"Date of birth must be no earlier than {this.minDateOfBirth} " +
                    "and no later than the current date", nameof(dateOfBirth));
            }
        }
    }
}
