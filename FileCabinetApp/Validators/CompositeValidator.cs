using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Provide the functionality to validation.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Validators list.</param>
        public CompositeValidator(List<IRecordValidator> validators)
        {
            this.validators = validators;
        }

        /// <inheritdoc/>
        public void ValidateParameters(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            foreach (var item in this.validators)
            {
                item.ValidateParameters(parametresOfRecord);
            }
        }
    }
}