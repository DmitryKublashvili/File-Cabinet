using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Program class. Starts the program and implements the file cabinet menu functions.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Dmitry Kublashvili";
        private const string DefaultValidationRulesMessage = "Using default validation rules.";
        private const string CustomValidationRulesMessage = "Using custom validation rules.";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string ExitMessage = "Exiting an application...";
        private const string ThereAreNoRecordsMessage = "There are no records yet.";
        private const string NoMatchesMessage = "No matches were found.";
        private const string NotValidEmptyNameMessage = "The name must not be null or contain only spaces";
        private const string NotValidSimbolsInNameMessage = "The name must consists of only letters";
        private const string NotParsedSalaryMessage = "Incorrect summ, please enter again";
        private const string IncorrectSexMessage = "Incorrect. Please, enter one letter of M or F";
        private const string IncorrectDateOfBirthFormatMessage = "Incorrect format of date, enter the date in format mm/dd/yyyy please.";

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static CultureInfo cultureInfo = new ("en");
        private static bool isRunning = true;
        private static bool isDefaultValidatoinRules = true;
        private static IRecordValidator validator = new DefaultValidator();
        private static IFileCabinetService fileCabinetService = new FileCabinetService(validator);

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistic", "The 'stat' command prints statistic." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits selected (by id) record", "The 'edit' command allows to edit selected by Id record." },
            new string[] { "list", "shows records information", "The 'list' command shows records information." },
            new string[] { "find", "finds records by the specified parameter", "The 'find' command shows a list of records in which the specified parameter was found." },
        };

        /// <summary>
        /// Starts the program and implements the menu functions.
        /// </summary>
        /// <param name="args">Params.</param>
        public static void Main(string[] args)
        {
            if (!(args is null) && args.Length > 0)
            {
                string parameterOfValidation = string.Join(" ", args).ToUpperInvariant();
                Console.WriteLine(parameterOfValidation);

                if (parameterOfValidation.Contains("CUSTOM"))
                {
                    isDefaultValidatoinRules = false;
                    validator = new CustomValidator();
                    fileCabinetService = new FileCabinetService(validator);
                }
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(isDefaultValidatoinRules ? DefaultValidationRulesMessage : CustomValidationRulesMessage);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                string[] inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine(ExitMessage);
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string firstName = GetCheckedName("First name");

            string lastName = GetCheckedName("Last name");

            DateTime dateOfBirth = GetCheckedDateOfBirth();

            char sex = GetCheckedSex();

            decimal salary = GetCheckedSalary();

            short yearsOfService = GetCheckedYearsOfService();

            var id = fileCabinetService.CreateRecord(new ParametresOfRecord(firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

            Console.WriteLine($"Record #{id} is created.");
        }

        private static void Edit(string userEnter)
        {
            int indexOfRecord = GetIndexOfRecord(userEnter);

            if (indexOfRecord == -1)
            {
                Console.WriteLine($"#{userEnter} record is not found.");
                return;
            }

            string firstName = GetCheckedName("First name");

            string lastName = GetCheckedName("Last name");

            DateTime dateOfBirth = GetCheckedDateOfBirth();

            char sex = GetCheckedSex();

            decimal salary = GetCheckedSalary();

            short yearsOfService = GetCheckedYearsOfService();

            var id = fileCabinetService.GetRecords()[indexOfRecord].Id;

            fileCabinetService.EditRecord(new ParametresOfRecord(id, firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void List(string parameters)
        {
            var list = fileCabinetService.GetRecords();

            if (list is null || list.Count == 0)
            {
                Console.WriteLine(ThereAreNoRecordsMessage);
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(
                    $"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, " +
                    $"{list[i].DateOfBirth.ToString("yyyy-MMM-d", cultureInfo)}, " +
                    $"Sex - {list[i].Sex}, Salary {list[i].Salary.ToString(cultureInfo)}, " +
                    $"{list[i].YearsOfService} years Of Service");
            }
        }

        private static void Find(string parametres)
        {
            string[] userCommandParameters = parametres.Split(new char[] { ' ', '"' }, 2, StringSplitOptions.RemoveEmptyEntries);

            List<string> commandParametres = new List<string>()
            {
                "FIRSTNAME",
                "LASTNAME",
                "DATEOFBIRTH",
            };

            string firstParameter = userCommandParameters[0].ToUpperInvariant();

            if (!commandParametres.Contains(firstParameter))
            {
                Console.WriteLine($"Unknown parameter '{userCommandParameters[0]}'.");
                return;
            }

            string secondParameter = userCommandParameters[1].Trim('"', ' ').ToUpperInvariant();
            ReadOnlyCollection<FileCabinetRecord> foundRecords;

            if (firstParameter == "FIRSTNAME")
            {
                foundRecords = fileCabinetService.FindByFirstName(secondParameter);
            }
            else if (firstParameter == "LASTNAME")
            {
                foundRecords = fileCabinetService.FindByLastName(secondParameter);
            }
            else
            {
                if (DateTime.TryParse(secondParameter, cultureInfo, DateTimeStyles.AdjustToUniversal, out DateTime date))
                {
                    foundRecords = fileCabinetService.FindByDateOfBirth(date);
                }
                else
                {
                    Console.WriteLine($"Date of birth '{secondParameter}' was incorrect format.");
                    return;
                }
            }

            if (foundRecords.Count == 0)
            {
                Console.WriteLine(NoMatchesMessage);
                return;
            }

            for (int i = 0; i < foundRecords.Count; i++)
            {
                Console.WriteLine(
                    $"#{foundRecords[i].Id}, {foundRecords[i].FirstName}, {foundRecords[i].LastName}, " +
                    $"{foundRecords[i].DateOfBirth.ToString("yyyy-MMM-d", cultureInfo)}, " +
                    $"Sex - {foundRecords[i].Sex}, Salary {foundRecords[i].Salary.ToString(cultureInfo)}, " +
                    $"{foundRecords[i].YearsOfService} years Of Service, ");
            }
        }

        private static string GetCheckedName(string typeOfName)
        {
            string checkedName;
            bool isValid = false;
            var minLettersCountInName = validator.MinLettersCountInName;
            var maxLettersCountInName = validator.MaxLettersCountInName;

            do
            {
                Console.Write($"{typeOfName}: ");
                checkedName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(checkedName) || checkedName.Length == 0)
                {
                    Console.WriteLine(NotValidEmptyNameMessage);
                    continue;
                }

                if (checkedName.Length < minLettersCountInName || checkedName.Length > maxLettersCountInName)
                {
                    Console.WriteLine($"The name must be {minLettersCountInName}-{maxLettersCountInName} characters long");
                    continue;
                }

                bool isOnlyLettersInside = true;

                for (int i = 0; i < checkedName.Length; i++)
                {
                    if (!char.IsLetter(checkedName[i]))
                    {
                        Console.WriteLine(NotValidSimbolsInNameMessage);
                        isOnlyLettersInside = false;
                        break;
                    }
                }

                isValid = isOnlyLettersInside;
            }
            while (!isValid);

            return checkedName;
        }

        private static decimal GetCheckedSalary()
        {
            decimal checkedSalary;
            bool isValid;
            var minSalary = validator.MinSalary;
            var maxSalary = validator.MaxSalary;

            do
            {
                Console.Write("Salary: ");

                while (!decimal.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, cultureInfo, out checkedSalary))
                {
                    Console.WriteLine(NotParsedSalaryMessage);
                }

                isValid = checkedSalary >= minSalary && checkedSalary <= maxSalary;

                if (!isValid)
                {
                    Console.WriteLine(
                        $"The salary must be greater than or equal to {minSalary} " +
                        $"and less than or equal to {maxSalary}");
                }
            }
            while (!isValid);

            return checkedSalary;
        }

        private static char GetCheckedSex()
        {
            bool isValid = false;
            string checkedSex;

            do
            {
                Console.Write("Sex (M or F): ");
                checkedSex = Console.ReadLine().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(checkedSex) || checkedSex.Length != 1)
                {
                    Console.WriteLine(IncorrectSexMessage);
                    continue;
                }

                isValid = checkedSex[0] == 'M' || checkedSex[0] == 'F';

                if (!isValid)
                {
                    Console.WriteLine(IncorrectSexMessage);
                }
            }
            while (!isValid);

            return checkedSex[0];
        }

        private static DateTime GetCheckedDateOfBirth()
        {
            DateTime checkedDateOfBirth;
            bool isValid;
            var minDateOfBirth = validator.MinDateOfBirth;

            do
            {
                Console.Write("Date of birth: ");

                while (!DateTime.TryParse(Console.ReadLine(), cultureInfo, DateTimeStyles.AdjustToUniversal, out checkedDateOfBirth))
                {
                    Console.WriteLine(IncorrectDateOfBirthFormatMessage);
                    Console.Write("Date of birth: ");
                }

                isValid = checkedDateOfBirth >= minDateOfBirth && checkedDateOfBirth <= DateTime.Now;

                if (!isValid)
                {
                    Console.WriteLine(
                          $"Date of birth must be no earlier than {minDateOfBirth.ToShortDateString()} " +
                          "and no later than the current date");
                }
            }
            while (!isValid);

            return checkedDateOfBirth;
        }

        private static short GetCheckedYearsOfService()
        {
            short checkedYearsOfService;
            bool isValid;
            var minYearsOfService = validator.MinYearsOfService;
            var maxYearsOfService = validator.MaxYearsOfService;

            do
            {
                Console.Write("Years Of Service: ");

                while (!short.TryParse(Console.ReadLine(), out checkedYearsOfService))
                {
                    Console.WriteLine("Incorrect value, only integers in 0-50 interval are available. Please enter again");
                }

                isValid = checkedYearsOfService >= minYearsOfService && checkedYearsOfService <= maxYearsOfService;

                if (!isValid)
                {
                    Console.WriteLine(
                        $"The years of service parameter must be greater than or equal to {minYearsOfService} " +
                        $"and less than or equal to {maxYearsOfService}");
                }
            }
            while (!isValid);

            return checkedYearsOfService;
        }

        private static int GetIndexOfRecord(string parameter)
        {
            if (int.TryParse(parameter, out int id))
            {
                ReadOnlyCollection<FileCabinetRecord> listOfRecords = fileCabinetService.GetRecords();

                for (int i = 0; i < listOfRecords.Count; i++)
                {
                    if (id == listOfRecords[i].Id)
                    {
                        return i;
                    }
                }

                return -1;
            }

            return -1;
        }
    }
}