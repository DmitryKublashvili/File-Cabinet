using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Define the functionality of validators type.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Gets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MinLettersCountInName { get; }

        /// <summary>
        /// Gets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public int MaxLettersCountInName { get; }

        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MinSalary { get; }

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public int MaxSalary { get; }

        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MinYearsOfService { get; }

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public int MaxYearsOfService { get; }

        /// <summary>
        /// Gets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public DateTime MinDateOfBirth { get; }

        /// <summary>
        /// Validate parameters of record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord instance.</param>
        void ValidateParameters(ParametresOfRecord parametresOfRecord);
    }
}