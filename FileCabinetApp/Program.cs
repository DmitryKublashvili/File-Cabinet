﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

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
        private const string IncorrectCommandFormatMessage = "Comand has incorrect format.";
        private const string StorageFilePath = "cabinet-records.db";

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static CultureInfo cultureInfo = new ("en");
        private static FileStream fileStream;
        private static bool isRunning = true;
        private static bool isDefaultValidatoinRules = true;
        private static bool isFileSystemStorageUsed;
        private static IRecordValidator validator = new DefaultValidator();
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(validator);

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
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
            new string[] { "export", "exports current state in file", "The 'export' exports current state in file according to the specified parameters." },
            new string[] { "import", "imports records from file", "The 'import' imports records from CSV or XML format file." },
        };

        /// <summary>
        /// Starts the program and implements the menu functions.
        /// </summary>
        /// <param name="args">Params.</param>
        public static void Main(string[] args)
        {
            if (!(args is null) && args.Length > 0)
            {
                CommandLineArgumentsApplying(args);
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

        private static void CommandLineArgumentsApplying(string[] args)
        {
            string parameterOfValidation = string.Join(" ", args).ToUpperInvariant();

            if (parameterOfValidation.Contains("--VALIDATION-RULES=CUSTOM") || parameterOfValidation.Contains("-V CUSTOM"))
            {
                isDefaultValidatoinRules = false;
                validator = new CustomValidator();

                if (parameterOfValidation.Contains("--STORAGE=FILE") || parameterOfValidation.Contains("-S FILE"))
                {
                    fileStream = new FileStream(StorageFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(validator, fileStream);
                    isFileSystemStorageUsed = true;
                    Console.WriteLine("Used storage in file.");
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(validator);
                }
            }
            else
            {
                if (parameterOfValidation.Contains("--STORAGE=FILE") || parameterOfValidation.Contains("-S FILE"))
                {
                    fileStream = new FileStream(StorageFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(validator, fileStream);
                    isFileSystemStorageUsed = true;
                    Console.WriteLine("Used storage in file.");
                }
            }
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
            if (isFileSystemStorageUsed)
            {
                fileStream.Dispose();
                fileStream.Close();
            }

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
            Console.Write($"First name: ");
            string firstName = ReadInput<string>(NamesConverter, NamesValidator);

            Console.Write($"Second name: ");
            string lastName = ReadInput<string>(NamesConverter, NamesValidator);

            Console.Write("Date of birth: ");
            DateTime dateOfBirth = ReadInput<DateTime>(DateOfBirthConverter, DateOfBirthValidator);

            Console.Write("Sex (M or F): ");
            char sex = ReadInput<char>(SexConverter, SexValidator);

            Console.Write("Salary: ");
            decimal salary = ReadInput<decimal>(SalaryConverter, SalaryValidator);

            Console.Write("Years Of Service: ");
            short yearsOfService = ReadInput<short>(YearsOfServiceConverter, YearsOfServiceValidator);

            var id = fileCabinetService.CreateRecord(new ParametresOfRecord(firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

            Console.WriteLine($"Record #{id} is created.");
        }

        private static void Edit(string userEnter)
        {
            if (!int.TryParse(userEnter, out int id))
            {
                Console.WriteLine("Incorrect input.");
                return;
            }

            if (!isFileSystemStorageUsed && !CheckIndexOfRecordInMemory(id))
            {
                Console.WriteLine($"#{userEnter} record is not found.");
                return;
            }

            if (isFileSystemStorageUsed && id > fileCabinetService.GetStat())
            {
                Console.WriteLine($"#{userEnter} record is not found.");
                return;
            }

            Console.Write($"First name: ");
            string firstName = ReadInput<string>(NamesConverter, NamesValidator);

            Console.Write($"Second name: ");
            string lastName = ReadInput<string>(NamesConverter, NamesValidator);

            Console.Write("Date of birth: ");
            DateTime dateOfBirth = ReadInput<DateTime>(DateOfBirthConverter, DateOfBirthValidator);

            Console.Write("Sex (M or F): ");
            char sex = ReadInput<char>(SexConverter, SexValidator);

            Console.Write("Salary: ");
            decimal salary = ReadInput<decimal>(SalaryConverter, SalaryValidator);

            Console.Write("Years Of Service: ");
            short yearsOfService = ReadInput<short>(YearsOfServiceConverter, YearsOfServiceValidator);

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

        private static void Export(string value)
        {
            var parameters = value.Split(" ");

            if (parameters.Length != 2)
            {
                Console.WriteLine(IncorrectCommandFormatMessage);
                return;
            }

            if (parameters[0].ToUpperInvariant() != "CSV" && parameters[0].ToUpperInvariant() != "XML")
            {
                Console.WriteLine(IncorrectCommandFormatMessage);
                return;
            }

            if (parameters[0].ToUpperInvariant() != parameters[1].Split(".")[^1].ToUpperInvariant())
            {
                Console.WriteLine("The export command and file extension must have the same format");
                return;
            }

            if (File.Exists(parameters[1]))
            {
                Console.WriteLine("File is exist - rewrite {0}[Y / n]", parameters[1]);
                string answere = Console.ReadLine().ToUpperInvariant();

                while (answere != "Y")
                {
                    if (answere == "N")
                    {
                        return;
                    }

                    Console.WriteLine("Incorrect answere. Write 'Y' or 'N' and press Enter, please.");

                    answere = Console.ReadLine().ToUpperInvariant();
                }
            }

            using (StreamWriter sw = new StreamWriter(parameters[1]))
            {
                var snapShot = (fileCabinetService as FileCabinetMemoryService).MakeSnapshot();

                if (parameters[0].ToUpperInvariant() == "CSV")
                {
                    snapShot.SaveToCSV(sw);
                }
                else
                {
                    snapShot.SaveToXML(sw);
                }
            }

            Console.WriteLine("All records are exported to file {0}.", parameters[1]);
        }

        private static bool CheckIndexOfRecordInMemory(int id)
        {
            ReadOnlyCollection<FileCabinetRecord> listOfRecords = fileCabinetService.GetRecords();

            for (int i = 0; i < listOfRecords.Count; i++)
            {
                if (id == listOfRecords[i].Id)
                {
                    return true;
                }
            }

            return false;
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        private static Tuple<bool, string> YearsOfServiceValidator(short arg)
        {
            bool isValid = arg >= validator.MinYearsOfService && arg <= validator.MaxYearsOfService;

            string message = $"The years of service parameter must be greater than or equal to {validator.MinYearsOfService} " +
                    $"and less than or equal to {validator.MaxYearsOfService}";

            return new Tuple<bool, string>(isValid, message);
        }

        private static Tuple<bool, string, short> YearsOfServiceConverter(string arg)
        {
            bool isConverted = short.TryParse(arg, out short checkedYearsOfService);

            string message = $"Incorrect value, only integers in {validator.MinYearsOfService}-{validator.MaxYearsOfService} interval are available. Please enter again";

            return new Tuple<bool, string, short>(isConverted, message, checkedYearsOfService);
        }

        private static Tuple<bool, string, char> SexConverter(string arg)
        {
            bool isConverted = !string.IsNullOrWhiteSpace(arg) && arg.Length == 1;

            return new Tuple<bool, string, char>(isConverted, IncorrectSexMessage, isConverted ? arg[0] : '_');
        }

        private static Tuple<bool, string> SexValidator(char arg)
        {
            char sexToUpper = char.ToUpperInvariant(arg);

            bool isValid = sexToUpper == 'M' || sexToUpper == 'F';

            return new Tuple<bool, string>(isValid, IncorrectSexMessage);
        }

        private static Tuple<bool, string, string> NamesConverter(string arg)
        {
            return new Tuple<bool, string, string>(true, "name", arg);
        }

        private static Tuple<bool, string> NamesValidator(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg) || arg.Length == 0)
            {
                return new Tuple<bool, string>(false, NotValidEmptyNameMessage);
            }

            if (arg.Length < validator.MinLettersCountInName || arg.Length > validator.MaxLettersCountInName)
            {
                return new Tuple<bool, string>(false, $"The name must be {validator.MinLettersCountInName} - {validator.MaxLettersCountInName} characters long");
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

        private static Tuple<bool, string> SalaryValidator(decimal arg)
        {
            bool isValid = arg >= validator.MinSalary && arg <= validator.MaxSalary;

            string message = $"The salary must be greater than or equal to {validator.MinSalary} " +
                             $"and less than or equal to {validator.MaxSalary}";

            return new Tuple<bool, string>(isValid, message);
        }

        private static Tuple<bool, string, decimal> SalaryConverter(string arg)
        {
            bool isConverted = decimal.TryParse(arg, NumberStyles.AllowDecimalPoint, cultureInfo, out decimal checkedSalary);

            return new Tuple<bool, string, decimal>(isConverted, NotParsedSalaryMessage, checkedSalary);
        }

        private static Tuple<bool, string, DateTime> DateOfBirthConverter(string arg)
        {
            bool isConverted = DateTime.TryParse(arg, cultureInfo, DateTimeStyles.AdjustToUniversal, out DateTime checkedDateOfBirth);

            return new Tuple<bool, string, DateTime>(isConverted, IncorrectDateOfBirthFormatMessage, checkedDateOfBirth);
        }

        private static Tuple<bool, string> DateOfBirthValidator(DateTime arg)
        {
            bool isValid = arg >= validator.MinDateOfBirth && arg <= DateTime.Now;

            string message = $"Date of birth must be no earlier than {validator.MinDateOfBirth.ToShortDateString()} " +
                      "and no later than the current date";

            return new Tuple<bool, string>(isValid, message);
        }

        private static void Import(string value)
        {
            if (value is null)
            {
                Console.WriteLine(IncorrectCommandFormatMessage);
                return;
            }

            var parameters = value.Split(" ");

            if (parameters.Length < 2)
            {
                Console.WriteLine(IncorrectCommandFormatMessage);
                return;
            }

            if (parameters[0].ToUpperInvariant() != "CSV" && parameters[0].ToUpperInvariant() != "XML")
            {
                Console.WriteLine(IncorrectCommandFormatMessage);
                return;
            }

            if (parameters[0].ToUpperInvariant() != parameters[^1].Split(".")[^1].ToUpperInvariant())
            {
                Console.WriteLine("The export command and file extension must have the same format");
                return;
            }

            string filePath = string.Concat(parameters[1..]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Import error: file {value} does not exist");
                return;
            }

            if (parameters[0].ToUpperInvariant() == "CSV")
            {
                ImportCSV(filePath);
            }
            else
            {
                ImportXML(filePath);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Temp supression.")]
        private static void ImportXML(string filePath)
        {
            Console.WriteLine($"Import XML started from {filePath}");

            var snapShot = new FileCabinetServiceSnapshot();

            snapShot.LoadFromXML(new StreamReader(filePath));

            fileCabinetService.Restore(snapShot);
        }

        private static void ImportCSV(string filePath)
        {
            Console.WriteLine($"Import CSV started from {filePath}");

            var snapShot = new FileCabinetServiceSnapshot();

            snapShot.LoadFromCSV(new StreamReader(filePath));

            int amountImportedRecords = fileCabinetService.Restore(snapShot);

            Console.WriteLine(amountImportedRecords + " records were imported from " + filePath);
        }
    }
}