using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers;

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
        /// <param name="records">Records to print.</param>
        public static void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (var item in records)
            {
                if (item is null)
                {
                    continue;
                }

                Console.WriteLine(
                    $"#{item.Id}, {item.FirstName}, {item.LastName}, " +
                    $"{item.DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                    $"Sex - {item.Sex}, Salary {item.Salary.ToString(Program.CultureInfoSettings)}, " +
                    $"{item.YearsOfService} years Of Service");
            }
        }

        private static ICommandHandler CreateCommandHendlers()
        {
            HelpCommandHandler helpCommandHandler = new HelpCommandHandler();
            ExitCommandHandler exitCommandHandler = new ExitCommandHandler(Exit);
            StatCommandHandler statCommandHandler = new StatCommandHandler(fileCabinetService);
            CreateCommandHandler createCommandHandler = new CreateCommandHandler(fileCabinetService);
            ListCommandHandler listCommandHandler = new ListCommandHandler(fileCabinetService, Print);
            EditCommandHandler editCommandHandler = new EditCommandHandler(fileCabinetService);
            FindCommandHandler findCommandHandler = new FindCommandHandler(fileCabinetService, Print);
            ExportCommandHandler exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            ImportCommandHandler importCommandHandler = new ImportCommandHandler(fileCabinetService);
            RemoveCommandHandler removeCommandHandler = new RemoveCommandHandler(fileCabinetService);
            PurgeCommandHandler purgeCommandHandler = new PurgeCommandHandler(fileCabinetService, isFileSystemStorageUsed);

            helpCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(listCommandHandler);
            listCommandHandler.SetNext(editCommandHandler);
            editCommandHandler.SetNext(findCommandHandler);
            findCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(removeCommandHandler);
            removeCommandHandler.SetNext(purgeCommandHandler);

            return helpCommandHandler;
        }

        private static void CommandLineArgumentsApplying(string[] args)
        {
            string parameterOfValidation = string.Join(" ", args).ToUpperInvariant();

            if (parameterOfValidation.Contains("--VALIDATION-RULES=CUSTOM") || parameterOfValidation.Contains("-V CUSTOM"))
            {
                isDefaultValidatoinRules = false;
                Validator = new CustomValidator();

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
                }
            }
            else
            {
                isDefaultValidatoinRules = true;
                Validator = new DefaultValidator();

                if (parameterOfValidation.Contains("--STORAGE=FILE") || parameterOfValidation.Contains("-S FILE"))
                {
                    var fileStream = new FileStream(StorageFilePath, FileMode.OpenOrCreate);
                    fileCabinetService = new FileCabinetFilesystemService(Validator, fileStream);
                    isFileSystemStorageUsed = true;
                    Console.WriteLine("Used storage in file.");
                }
            }
        }

        private static void Exit()
        {
            isRunning = false;
        }
    }
}
