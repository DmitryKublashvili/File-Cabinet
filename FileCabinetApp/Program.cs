using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Iterators;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.ValidationSettings;
using Microsoft.Extensions.Configuration;

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
        private const string StorageFilePath = "cabinet-records.db";
        private static bool isDefaultValidatoinRules = true;
        private static bool isFileSystemStorageUsed;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1450:Private fields only used as local variables in methods should become local variables", Justification = "It used by sending delegate outsiade.")]
        private static bool isRunning;
        private static IFileCabinetService fileCabinetService;

        /// <summary>
        /// Gets current culture settings.
        /// </summary>
        /// <value>
        /// Current culture settings.
        /// </value>
        public static CultureInfo CultureInfoSettings { get; private set; }

        /// <summary>
        /// Gets validator.
        /// </summary>
        /// <value>
        /// IRecordValidator implemented instance.
        /// </value>
        public static IRecordValidator Validator { get; private set; }

        /// <summary>
        /// Starts the program and implements the menu functions.
        /// </summary>
        /// <param name="args">Params.</param>
        public static void Main(string[] args)
        {
            SetValidationRules();

            isRunning = true;
            CultureInfoSettings = new ("en");

            CommandLineArgumentsApplying(args);

            ICommandHandler commandHandler = CreateCommandHendlers();

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
                    Console.WriteLine(HintMessage);
                    continue;
                }

                string parameters = inputs.Length > 1 ? inputs[1] : string.Empty;

                AppCommandRequest request = new AppCommandRequest(command, parameters);

                commandHandler.Handle(request);
            }
            while (isRunning);
        }

        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="iterator">Records to print.</param>
        public static void Print(IRecordIterator iterator)
        {
            if (iterator is null)
            {
                throw new ArgumentNullException(nameof(iterator));
            }

            while (iterator.HasMore())
            {
                var record = iterator.GetNext();

                Console.WriteLine(
                    $"#{record.Id}, {record.FirstName}, {record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                    $"Sex - {record.Sex}, Salary {record.Salary.ToString(Program.CultureInfoSettings)}, " +
                    $"{record.YearsOfService} years Of Service");
            }
        }

        /// <summary>
        /// Prints records from IEnumerable.
        /// </summary>
        /// <param name="records">Records to print.</param>
        public static void PrintIEnumerable(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var record in records)
            {
                Console.WriteLine(
                    $"#{record.Id}, {record.FirstName}, {record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                    $"Sex - {record.Sex}, Salary {record.Salary.ToString(Program.CultureInfoSettings)}, " +
                    $"{record.YearsOfService} years Of Service");
            }
        }

        private static ICommandHandler CreateCommandHendlers()
        {
            HelpCommandHandler helpCommandHandler = new HelpCommandHandler();
            ExitCommandHandler exitCommandHandler = new ExitCommandHandler(Exit);
            StatCommandHandler statCommandHandler = new StatCommandHandler(fileCabinetService);
            CreateCommandHandler createCommandHandler = new CreateCommandHandler(fileCabinetService);
            InsertCommandHandler insertCommandHandler = new InsertCommandHandler(fileCabinetService);
            ListCommandHandler listCommandHandler = new ListCommandHandler(fileCabinetService, Print);
            DeleteCommandHandler deleteCommandHandler = new DeleteCommandHandler(fileCabinetService);
            FindCommandHandler findCommandHandler = new FindCommandHandler(fileCabinetService, PrintIEnumerable);
            ExportCommandHandler exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            ImportCommandHandler importCommandHandler = new ImportCommandHandler(fileCabinetService);
            PurgeCommandHandler purgeCommandHandler = new PurgeCommandHandler(fileCabinetService, isFileSystemStorageUsed);
            UpdateCommandHandler updateCommandHandler = new UpdateCommandHandler(fileCabinetService);

            helpCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(insertCommandHandler);
            insertCommandHandler.SetNext(listCommandHandler);
            listCommandHandler.SetNext(findCommandHandler);
            findCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(updateCommandHandler);
            updateCommandHandler.SetNext(deleteCommandHandler);
            deleteCommandHandler.SetNext(purgeCommandHandler);

            return helpCommandHandler;
        }

        private static void CommandLineArgumentsApplying(string[] args)
        {
            string parameterOfValidation = string.Join(" ", args).ToUpperInvariant();

            if (parameterOfValidation.Contains("--VALIDATION-RULES=CUSTOM") || parameterOfValidation.Contains("-V CUSTOM"))
            {
                ValidationMethods.IsDefaultValidatoinRules = false;
                isDefaultValidatoinRules = false;
                Validator = new ValidatorBuilder().CreateCustom();

                if (parameterOfValidation.Contains("--STORAGE=FILE") || parameterOfValidation.Contains("-S FILE"))
                {
                    var fileStream = new FileStream(StorageFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(Validator, fileStream);
                    isFileSystemStorageUsed = true;
                    Console.WriteLine("Used storage in file.");
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(Validator);
                    isFileSystemStorageUsed = false;
                    Console.WriteLine("Used storage in memory.");
                }
            }
            else
            {
                ValidationMethods.IsDefaultValidatoinRules = true;
                isDefaultValidatoinRules = true;
                Validator = new ValidatorBuilder().CreateDefault();

                if (parameterOfValidation.Contains("--STORAGE=FILE") || parameterOfValidation.Contains("-S FILE"))
                {
                    var fileStream = new FileStream(StorageFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(Validator, fileStream);
                    isFileSystemStorageUsed = true;
                    Console.WriteLine("Used storage in file.");
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(Validator);
                    isFileSystemStorageUsed = false;
                    Console.WriteLine("Used storage in memory.");
                }
            }

            if (parameterOfValidation.Contains("USE-STOPWATCH"))
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (parameterOfValidation.Contains("USE-LOGGER"))
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
            }
        }

        private static void Exit()
        {
            isRunning = false;
        }

        private static void SetValidationRules()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("validation-rules.json")
                .Build();

            CommonAppValidationSettings validationAppSettings = builder.Get<CommonAppValidationSettings>();

            // default
            DefaultValidationSettings.MaxLettersCountInName = validationAppSettings.Default.FirstName.MaxLettersCountInName;
            DefaultValidationSettings.MinLettersCountInName = validationAppSettings.Default.FirstName.MinLettersCountInName;
            DefaultValidationSettings.MinDateOfBirth = validationAppSettings.Default.DateOfBirth.MinDateOfBirth;
            DefaultValidationSettings.MinSalary = validationAppSettings.Default.Salary.MinSalary;
            DefaultValidationSettings.MaxSalary = validationAppSettings.Default.Salary.MaxSalary;
            DefaultValidationSettings.MinYearsOfService = validationAppSettings.Default.YearsOfService.MinYearsOfService;
            DefaultValidationSettings.MaxYearsOfService = validationAppSettings.Default.YearsOfService.MaxYearsOfService;

            // custom
            CustomValidationSettings.MaxLettersCountInName = validationAppSettings.Custom.FirstName.MaxLettersCountInName;
            CustomValidationSettings.MinLettersCountInName = validationAppSettings.Custom.FirstName.MinLettersCountInName;
            CustomValidationSettings.MinDateOfBirth = validationAppSettings.Custom.DateOfBirth.MinDateOfBirth;
            CustomValidationSettings.MinSalary = validationAppSettings.Custom.Salary.MinSalary;
            CustomValidationSettings.MaxSalary = validationAppSettings.Custom.Salary.MaxSalary;
            CustomValidationSettings.MinYearsOfService = validationAppSettings.Custom.YearsOfService.MinYearsOfService;
            CustomValidationSettings.MaxYearsOfService = validationAppSettings.Custom.YearsOfService.MaxYearsOfService;
        }
    }
}
