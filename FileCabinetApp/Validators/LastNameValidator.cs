using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom LastName Validator.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
        private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";
        private readonly int minLettersCountInName;
        private readonly int maxLettersCountInName;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLettersCountInName">Min Letters Count In Name.</param>
        /// <param name="maxLettersCountInName">Max Letters Count In Name.</param>
        public LastNameValidator(int minLettersCountInName, int maxLettersCountInName)
        {
            this.minLettersCountInName = minLettersCountInName;
            this.maxLettersCountInName = maxLettersCountInName;
        }

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
            var name = parametresOfRecord.LastName;

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(id, nameof(name), NotValidEmptyNameMessage);
            }

            if (name.Length < this.minLettersCountInName || name.Length > this.maxLettersCountInName)
            {
                throw new ValidationException(id, $"The name must be {this.minLettersCountInName}-{this.maxLettersCountInName} characters long", nameof(name));
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
