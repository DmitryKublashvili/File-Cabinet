using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom FirstName Validator.
    /// </summary>
    public class CustomFirstNameValidator : IRecordValidator
    {
        private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
        private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";

        /// <summary>
        /// Gets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MinLettersCountInName { get; } = 1;

        /// <summary>
        /// Gets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MaxLettersCountInName { get; } = 20;

        /// <summary>
        /// Validate parametres.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "This method checks severak parameters.")]
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            var id = parametresOfRecord.Id;
            var name = parametresOfRecord.FirstName;

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(id, nameof(name), NotValidEmptyNameMessage);
            }

            if (name.Length < this.MinLettersCountInName || name.Length > this.MaxLettersCountInName)
            {
                throw new ValidationException(id, $"The name must be {this.MinLettersCountInName}-{this.MaxLettersCountInName} characters long", nameof(name));
            }

            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                {
                    throw new ValidationException(id, NotValidSimbolsInNameMessage, nameof(name));
                }
            }
        }
    }
}
