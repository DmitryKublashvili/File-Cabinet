using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Validation exception to display a violation of the validation rules.
    /// </summary>
    public class ValidationException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="id">Not valid record id</param>
        /// <param name="message">Exception message.</param>
        /// <param name="paramName">Name of not valid parameter.</param>
        public ValidationException(int id, string message, string paramName)
            : base(message, paramName)
        {
            this.NotValidRecordId = id;
        }

        /// <summary>
        /// Gets not valid record Id.
        /// </summary>
        /// <value>Not valid record integer Id.</value>
        public int NotValidRecordId { get; private set; }
    }
}
