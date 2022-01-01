using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator.
    /// </summary>
    public static class DefaultValidationParams
    {
        /// <summary>
        /// Gets or sets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public static int MinLettersCountInName { get; set; } = 2;

        /// <summary>
        /// Gets or sets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public static int MaxLettersCountInName { get; set;  } = 60;

        /// <summary>
        /// Gets or sets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public static DateTime MinDateOfBirth { get; set; } = new (1950, 1, 1);

        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public static decimal MinSalary { get; } = 2_000;

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public static decimal MaxSalary { get; } = 100_000;

        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public static int MinYearsOfService { get; } = 2;

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public static int MaxYearsOfService { get; } = 42;
    }
}
