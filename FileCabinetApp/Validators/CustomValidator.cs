using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
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

            new CustomFirstNameValidator().ValidateParameters(parametresOfRecord);
            new CustomLastNameValidator().ValidateParameters(parametresOfRecord);
            new CustomDateOfBirthValidator().ValidateParameters(parametresOfRecord);
            new CustomSexValidator().ValidateParameters(parametresOfRecord);
            new CustomSalaryValidator().ValidateParameters(parametresOfRecord);
            new CustomYearsOfServiceValidator().ValidateParameters(parametresOfRecord);
        }
    }
}
