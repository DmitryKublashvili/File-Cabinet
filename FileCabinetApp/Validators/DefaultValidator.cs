using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Parametres validator.
    /// </summary>
    public class DefaultValidator : IRecordValidator
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

            new DefaultFirstNameValidator().ValidateParameters(parametresOfRecord);
            new DefaultLastNameValidator().ValidateParameters(parametresOfRecord);
            new DefaultDateOfBirthValidator().ValidateParameters(parametresOfRecord);
            new DefaultSexValidator().ValidateParameters(parametresOfRecord);
            new DefaultSalaryValidator().ValidateParameters(parametresOfRecord);
            new DefaultYearsOfServiceValidator().ValidateParameters(parametresOfRecord);
        }
    }
}
