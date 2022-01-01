using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom Sex Validator.
    /// </summary>
    public class SexValidator : IRecordValidator
    {
        private const string IncorrectSexMessage = "Only the letters 'M' or 'F' are valid";

        /// <summary>
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "It is custom exception.")]
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var sex = parametresOfRecord.Sex;

            if (char.ToUpperInvariant(sex) != 'M' && char.ToUpperInvariant(sex) != 'F')
            {
                throw new ValidationException(parametresOfRecord.Id, IncorrectSexMessage, nameof(sex));
            }
        }
    }
}
