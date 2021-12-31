using System;
using System.Globalization;
using static FileCabinetApp.CommandHandlers.ValidationMethods;

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
            bool isValid = arg >= Program.Validator.MinYearsOfService && arg <= Program.Validator.MaxYearsOfService;

            string message = $"The years of service parameter must be greater than or equal to {Program.Validator.MinYearsOfService} " +
                    $"and less than or equal to {Program.Validator.MaxYearsOfService}";

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

            string message = $"Incorrect value, only integers in {Program.Validator.MinYearsOfService}-{Program.Validator.MaxYearsOfService} interval are available. Please enter again";

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

            if (arg.Length < Program.Validator.MinLettersCountInName || arg.Length > Program.Validator.MaxLettersCountInName)
            {
                return new Tuple<bool, string>(false, $"The name must be {Program.Validator.MinLettersCountInName} - {Program.Validator.MaxLettersCountInName} characters long");
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
            bool isValid = arg >= Program.Validator.MinSalary && arg <= Program.Validator.MaxSalary;

            string message = $"The salary must be greater than or equal to {Program.Validator.MinSalary} " +
                             $"and less than or equal to {Program.Validator.MaxSalary}";

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
            bool isValid = arg >= Program.Validator.MinDateOfBirth && arg <= DateTime.Now;

            string message = $"Date of birth must be no earlier than {Program.Validator.MinDateOfBirth.ToShortDateString()} " +
                      "and no later than the current date";

            return new Tuple<bool, string>(isValid, message);
        }
    }
}