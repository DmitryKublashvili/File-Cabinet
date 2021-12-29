using System;
using System.IO;

namespace FileCabinetGenerator
{
    /// <summary>
    /// User menu.
    /// </summary>
    static class Menu
    {
        public const string DefaultFileType = "csv";
        public const string DefaultFileName = "File-Cabinet-Generator-Storage";
        public const int DefaultRecordsAmount = 500;
        public const int DefaultStartId = 1;

        private static string fileType = DefaultFileType;
        private static string fileName = DefaultFileName;
        private static int recordsAmount = DefaultRecordsAmount;
        private static int startId = DefaultStartId;

        private static RecordsGenerator generator = null;

        /// <summary>
        /// Starts menu.
        /// </summary>
        /// <param name="args">Command line args.</param>
        public static void Start(string[] args)
        {
            CommandLineParametersHandling(args);

            bool isSettingsFinishd = false;

            while (!isSettingsFinishd)
            {
                Console.Clear();

                PrintStartConditions();

                Console.WriteLine("If you want to change:"
                    + Environment.NewLine + "\t" + "fyle type press 'T' and then press Enter"
                    + Environment.NewLine + "\t" + "fyle name press 'O' and then press Enter"
                    + Environment.NewLine + "\t" + "records amount press 'A' and then press Enter"
                    + Environment.NewLine + "\t" + "start ID press 'I' and then press Enter"
                    + Environment.NewLine + "To create file with current conditions press 'D' and press Enter"
                    );

                string userInput = Console.ReadLine().ToUpperInvariant();

                switch (userInput)
                {
                    case "T":
                        SetFileType(); break;
                    case "O":
                        SetFileName(); break;
                    case "A":
                        SetRecordsAmount(); break;
                    case "I":
                        SetStartId(); break;
                    case "D":
                        isSettingsFinishd = true; break;
                    default:
                        break;
                }
            }

            CreateRecords();

            Console.WriteLine("If you want to show records on console press 'Y' and press Enter. Otherwise press Enter.");

            string input = Console.ReadLine().ToUpperInvariant();

            if (input == "Y")
            {
                ShowRecords();
            }

            Console.WriteLine($"If you want to save records in {fileType} press 'Y' and press Enter. Otherwise press Enter.\n");

            input = Console.ReadLine().ToUpperInvariant();

            if (input == "Y")
            {
                if (fileType == "CSV")
                {
                    if (CsvExporter.ExportInCsvFile(fileName, generator.GetRecords(), out string exceptionMessage))
                    {
                        Console.WriteLine($"\nFile {fileName} created.");
                    }
                    else
                    {
                        Console.WriteLine(exceptionMessage);
                    }
                }
                else
                {
                    if (XmlExporter.ExportInXmlFile(fileName, generator.GetRecords(), out string exceptionMessage))
                    {
                        Console.WriteLine($"\nFile {fileName} created.");
                    }
                    else
                    {
                        Console.WriteLine(exceptionMessage);
                    }
                }
            }
        }

        private static void PrintStartConditions()
        {
            Console.WriteLine("Start conditions:" +
                Environment.NewLine + "\t" + "file type: " + fileType +
                Environment.NewLine + "\t" + "file name: " + fileName +
                Environment.NewLine + "\t" + "records amount: " + recordsAmount +
                Environment.NewLine + "\t" + "start Id: " + startId +
                Environment.NewLine);
        }

        private static void SetFileType()
        {
            Console.WriteLine($"Current type is {fileType}."
                + Environment.NewLine + "If you want to change it print \"csv\" or \"xml\" word and press Enter."
                + Environment.NewLine + "Otherwise press Enter.");

            var input = Console.ReadLine().ToUpperInvariant();

            if (input == "CSV" || input == "XML")
            {
                fileType = input.ToLowerInvariant();
                Console.WriteLine($"New file type is {fileType}. Press any key");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Incorrect input. Press any key.");
                Console.ReadKey();
            }

            FileNameValidation(ref fileName);
        }

        private static void SetFileName()
        {
            Console.WriteLine($"Current file name is {fileName}."
                + Environment.NewLine + "If you want to change it print new file name and press Enter."
                + Environment.NewLine + "Otherwise press Enter.");

            var input = Console.ReadLine();

            if (FileNameValidation(ref input))
            {
                fileName = input;
                Console.WriteLine($"New file name is {fileName}. Press any key");
                Console.ReadKey();
            }
        }

        private static void SetRecordsAmount()
        {
            Console.WriteLine($"Current records amount is {recordsAmount}."
                + Environment.NewLine + "If you want to change it print new amount and press Enter."
                + Environment.NewLine + "Otherwise press Enter.");

            var input = Console.ReadLine();

            if (int.TryParse(input, out int num) && num > 0)
            {
                recordsAmount = num;
                Console.WriteLine($"New records amount parameter is {recordsAmount}. Press any key");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Incorrect input. Press any key.");
                Console.ReadKey();
            }
        }

        private static void SetStartId()
        {
            Console.WriteLine($"Current start ID is {startId}."
                + Environment.NewLine + "If you want to change it print new ID and press Enter."
                + Environment.NewLine + "Otherwise press Enter.");

            var input = Console.ReadLine();

            if (int.TryParse(input, out int num) && num > 0)
            {
                startId = num;
                Console.WriteLine($"New start ID parameter is {startId}");
            }
            else
            {
                Console.WriteLine("Incorrect input. Press any key.");
                Console.ReadKey();
            }
        }

        private static void CreateRecords()
        {
            Console.WriteLine("Records creation started...");

            generator = new RecordsGenerator(
                fileType: fileType, fileName: fileName, recordsAmount: recordsAmount, startId: startId);

            generator.CreateRecords();

            Console.WriteLine("Records creation finished.");
        }

        private static void ShowRecords()
        {
            var records = generator.GetRecords();

            Console.WriteLine();

            foreach (var item in records)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
        }

        private static bool FileNameValidation(ref string fileName)
        {
            try
            {
                File.WriteAllText(fileName, "");
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine(" '" + fileName + "'" + $" is incorrect name for file in this operation system, " +
                    $"the default file name '{DefaultFileName}' will using. Press any key.");
                Console.ReadKey();
                return false;
            }

            File.Delete(fileName + ".txt");

            // file extension checking
            string[] fileNameComponents = fileName.Split('.');

            if (fileNameComponents[^1].ToUpperInvariant() != fileType.ToUpperInvariant())
            {
                fileNameComponents[^1] = fileType;

                Console.WriteLine($"\nThe file extension was set according to the file type {fileType}. Press any key.");
                Console.ReadKey();
            }

            fileName = string.Join('.', fileNameComponents);

            return true;
        }

        private static void CommandLineParametersHandling(string[] args)
        {
            if (args.Length > 1)
            {
                string stringCommandLineParameters = string.Join(" ", args);
                args = stringCommandLineParameters.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].ToUpperInvariant();
                }

                // type of file definition
                var indexOfTypeCommand = Array.IndexOf(args, "--OUTPUT-TYPE") != -1 ? Array.IndexOf(args, "--OUTPUT-TYPE") : Array.IndexOf(args, "-T");

                if (indexOfTypeCommand != -1 && indexOfTypeCommand < args.Length - 1 && (args[indexOfTypeCommand + 1] == "CSV" || args[indexOfTypeCommand + 1] == "XML"))
                {
                    fileType = args[indexOfTypeCommand + 1];
                }

                // file name definition
                var indexOfFileNameCommand = Array.IndexOf(args, "--OUTPUT") != -1 ? Array.IndexOf(args, "--OUTPUT") : Array.IndexOf(args, "-O");

                if (indexOfFileNameCommand != -1 && indexOfFileNameCommand < args.Length - 1 && FileNameValidation(ref args[indexOfFileNameCommand + 1]))
                {
                    fileName = args[indexOfFileNameCommand + 1];
                }

                // records amount definition
                var indexOfRecordsAmountCommand = Array.IndexOf(args, "--RECORDS-AMOUNT") != -1 ? Array.IndexOf(args, "--RECORDS-AMOUNT") : Array.IndexOf(args, "-A");

                if (indexOfRecordsAmountCommand != -1 && indexOfRecordsAmountCommand < args.Length - 1 && int.TryParse(args[indexOfRecordsAmountCommand + 1], out int amount))
                {
                    recordsAmount = amount;
                }

                // start id definition
                var indexOfStartIdCommand = Array.IndexOf(args, "--START-ID") != -1 ? Array.IndexOf(args, "--START-ID") : Array.IndexOf(args, "-I");

                if (indexOfStartIdCommand != -1 && indexOfStartIdCommand < args.Length - 1 && int.TryParse(args[indexOfStartIdCommand + 1], out int id))
                {
                    startId = id;
                }
            }
        }
    }
}
