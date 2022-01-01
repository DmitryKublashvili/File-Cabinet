using System;
using System.Globalization;

#pragma warning disable CA1062

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Provides methods of validating record parameters.
    /// </summary>
    public static class ValidationMethods
    {
        private const string IncorrectSexMessage = "Incorrect. Please, enter one letter of M or F";
        private const string IncorrectDateOfBirthFormatMessage = "Incorrect format of date, enter the date in format mm/dd/yyyy please.";
        private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
        private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";
        private const string NotParsedSalaryMessage = "Incorrect summ, please enter again";

        /// <summary>
        /// Gets or sets a value indicating whether default validatoin Rules set.
        /// </summary>
        /// <value>
        /// A value indicating whether default validatoin Rules set.
        /// </value>
        public static bool IsDefaultValidatoinRules { get; set; } = true;

        /// <summary>
        /// Gets min letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public static int MinLettersCountInName
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultFirstNameValidator().MinLettersCountInName : new CustomFirstNameValidator().MinLettersCountInName;
            }
        }

        /// <summary>
        /// Gets max letters count in name.
        /// </summary>
        /// <value> Count of leters.
        /// </value>
        public static int MaxLettersCountInName
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultFirstNameValidator().MaxLettersCountInName : new CustomFirstNameValidator().MaxLettersCountInName;
            }
        }

        /// <summary>
        /// Gets min amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public static int MinSalary
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultSalaryValidator().MinSalary : new CustomSalaryValidator().MinSalary;
            }
        }

        /// <summary>
        /// Gets max amount of salary.
        /// </summary>
        /// <value> Amount of salary.
        /// </value>
        public static int MaxSalary
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultSalaryValidator().MaxSalary : new CustomSalaryValidator().MaxSalary;
            }
        }

        /// <summary>
        /// Gets min valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public static int MinYearsOfService
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultYearsOfServiceValidator().MinYearsOfService : new CustomYearsOfServiceValidator().MinYearsOfService;
            }
        }

        /// <summary>
        /// Gets max valid years of service.
        /// </summary>
        /// <value> Count of years.
        /// </value>
        public static int MaxYearsOfService
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultYearsOfServiceValidator().MaxYearsOfService : new CustomYearsOfServiceValidator().MaxYearsOfService;
            }
        }

        /// <summary>
        /// Gets min date of birth.
        /// </summary>
        /// <value> Date of birth.
        /// </value>
        public static DateTime MinDateOfBirth
        {
            get
            {
                return IsDefaultValidatoinRules ? new DefaultDateOfBirthValidator().MinDateOfBirth : new CustomDateOfBirthValidator().MinDateOfBirth;
            }
        }

        /// <summary>
        /// Reads and validates input parameters of record.
        /// </summary>
        /// <typeparam name="T">Some type.</typeparam>
        /// <param name="converter">converter.</param>
        /// <param name="validator">validator.</param>
        /// <returns>Value of T.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Years Of Service Validation.
        /// </summary>
        /// <param name="arg">String type argument.</param>
        /// <returns>Tuple of result of validation.</returns>
        public static Tuple<bool, string> YearsOfServiceValidator(short arg)
        {
            bool isValid = arg >= MinYearsOfService && arg <= MaxYearsOfService;

            string message = $"The years of service parameter must be greater than or equal to {MinYearsOfService} " +
                    $"and less than or equal to {MaxYearsOfService}";

            return new Tuple<bool, string>(isValid, message);
        }

        /// <summary>
        /// Years Of Service convertation.
        /// </summary>
        /// <param name="arg">String type argument to convertation.</param>
        /// <returns>Tuple of result of convertation.</returns>
        public static Tuple<bool, string, short> YearsOfServiceConverter(string arg)
        {
            bool isConverted = short.TryParse(arg, out short checkedYearsOfService);

            string message = $"Incorrect value, only integers in {MinYearsOfService}-{MaxYearsOfService} interval are available. Please enter again";

            return new Tuple<bool, string, short>(isConverted, message, checkedYearsOfService);
        }

        /// <summary>
        /// Sex convertation.
        /// </summary>
        /// <param name="arg">String type argument to convertation.</param>
        /// <returns>Tuple of result of convertation.</returns>
        public static Tuple<bool, string, char> SexConverter(string arg)
        {
            bool isConverted = !string.IsNullOrWhiteSpace(arg) && arg.Length == 1;

            return new Tuple<bool, string, char>(isConverted, IncorrectSexMessage, isConverted ? arg[0] : '_');
        }

        /// <summary>
        /// Sex validation.
        /// </summary>
        /// <param name="arg">String type argument to validation.</param>
        /// <returns>Tuple of result of validation.</returns>
        public static Tuple<bool, string> SexValidator(char arg)
        {
            char sexToUpper = char.ToUpperInvariant(arg);

            bool isValid = sexToUpper == 'M' || sexToUpper == 'F';

            return new Tuple<bool, string>(isValid, IncorrectSexMessage);
        }

        /// <summary>
        /// Names convertation.
        /// </summary>
        /// <param name="arg">String type argument to convertation.</param>
        /// <returns>Tuple of result of convertation.</returns>
        public static Tuple<bool, string, string> NamesConverter(string arg)
        {
            return new Tuple<bool, string, string>(true, "name", arg);
        }

        /// <summary>
        /// Names validation.
        /// </summary>
        /// <param name="arg">String type argument to validation.</param>
        /// <returns>Tuple of result of validation.</returns>
        public static Tuple<bool, string> NamesValidator(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg) || arg.Length == 0)
            {
                return new Tuple<bool, string>(false, NotValidEmptyNameMessage);
            }

            if (arg.Length < MinLettersCountInName || arg.Length > MaxLettersCountInName)
            {
                return new Tuple<bool, string>(false, $"The name must be {MinLettersCountInName} - {MaxLettersCountInName} characters long");
            }

            for (int i = 0; i < arg.Length; i++)
            {
                if (!char.IsLetter(arg[i]))
                {
                    return new Tuple<bool, string>(false, NotValidSimbolsInNameMessage);
                }
            }

            return new Tuple<bool, string>(true, "name");
        }

        /// <summary>
        /// Salary validation.
        /// </summary>
        /// <param name="arg">String type argument to validation.</param>
        /// <returns>Tuple of result of validation.</returns>
        public static Tuple<bool, string> SalaryValidator(decimal arg)
        {
            bool isValid = arg >= MinSalary && arg <= MaxSalary;

            string message = $"The salary must be greater than or equal to {MinSalary} " +
                             $"and less than or equal to {MaxSalary}";

            return new Tuple<bool, string>(isValid, message);
        }

        /// <summary>
        /// Salary convertation.
        /// </summary>
        /// <param name="arg">String type argument to convertation.</param>
        /// <returns>Tuple of result of convertation.</returns>
        public static Tuple<bool, string, decimal> SalaryConverter(string arg)
        {
            bool isConverted = decimal.TryParse(arg, NumberStyles.AllowDecimalPoint, Program.CultureInfoSettings, out decimal checkedSalary);

            return new Tuple<bool, string, decimal>(isConverted, NotParsedSalaryMessage, checkedSalary);
        }

        /// <summary>
        /// Date of birth convertation.
        /// </summary>
        /// <param name="arg">String type argument to convertation.</param>
        /// <returns>Tuple of result of convertation.</returns>
        public static Tuple<bool, string, DateTime> DateOfBirthConverter(string arg)
        {
            bool isConverted = DateTime.TryParse(arg, Program.CultureInfoSettings, DateTimeStyles.AdjustToUniversal, out DateTime checkedDateOfBirth);

            return new Tuple<bool, string, DateTime>(isConverted, IncorrectDateOfBirthFormatMessage, checkedDateOfBirth);
        }

        /// <summary>
        /// Date of birth validation.
        /// </summary>
        /// <param name="arg">String type argument to validation.</param>
        /// <returns>Tuple of result of validation.</returns>
        public static Tuple<bool, string> DateOfBirthValidator(DateTime arg)
        {
            bool isValid = arg >= MinDateOfBirth && arg <= DateTime.Now;

            string message = $"Date of birth must be no earlier than {MinDateOfBirth.ToShortDateString()} " +
                      "and no later than the current date";

            return new Tuple<bool, string>(isValid, message);
        }
    }
}