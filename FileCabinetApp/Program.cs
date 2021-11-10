using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dmitry Kublashvili";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static CultureInfo cultureInfo = new ("en");

        private static bool isRunning = true;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistic", "The 'stat' command prints statistic." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "list", "shows records information", "The 'list' command shows records information." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
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
            Console.WriteLine("Exiting an application...");
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

            decimal accountBalance = GetCheckedAccountBalance();

            var id = fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, sex, accountBalance);

            Console.Write($"Record #{id} is created.");
        }

        private static string GetCheckedName(string paramName)
        {
            string name;
            bool isValid = false;

            do
            {
                Console.Write($"{paramName}: ");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name) || name.Length == 0)
                {
                    Console.WriteLine("The name must not be null or contain only spaces");
                    continue;
                }

                if (name.Length is < 2 or > 60)
                {
                    Console.WriteLine("The first name must be 2-60 characters long");
                    continue;
                }

                bool isOnlyLettersInside = true;

                for (int i = 0; i < name.Length; i++)
                {
                    if (!char.IsLetter(name[i]))
                    {
                        Console.WriteLine("The name must consists of only letters");
                        isOnlyLettersInside = false;
                        break;
                    }
                }

                isValid = isOnlyLettersInside;
            }
            while (!isValid);

            return name;
        }

        private static decimal GetCheckedAccountBalance()
        {
            decimal accountBalance;
            bool isValid;

            do
            {
                Console.Write("Account balance: ");

                while (!decimal.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, cultureInfo, out accountBalance))
                {
                    Console.WriteLine("Incorrect summ, please enter again");
                }

                isValid = accountBalance >= -50_000 && accountBalance <= 1_000_000_000;

                if (!isValid)
                {
                    Console.WriteLine(
                        "The account balance must be greater than or equal to -50,000 " +
                        "and less than or equal to 1,000,000,000");
                }
            }
            while (!isValid);

            return accountBalance;
        }

        private static char GetCheckedSex()
        {
            bool isValid = false;
            string userEnter;

            do
            {
                Console.Write("Sex (M or F): ");
                userEnter = Console.ReadLine().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(userEnter) || userEnter.Length != 1)
                {
                    Console.WriteLine("Incorrect. Please, enter one letter of M or F");
                    continue;
                }

                isValid = userEnter[0] == 'M' || userEnter[0] == 'F';

                if (!isValid)
                {
                    Console.WriteLine("Incorrect. Please, enter one letter of M or F");
                }
            }
            while (!isValid);

            return userEnter[0];
        }

        private static DateTime GetCheckedDateOfBirth()
        {
            DateTime minDate = new (1950, 1, 1);
            DateTime dateOfBirth;
            bool isValid;

            do
            {
                Console.Write("Date of birth: ");

                while (!DateTime.TryParse(Console.ReadLine(), cultureInfo, DateTimeStyles.AdjustToUniversal, out dateOfBirth))
                {
                    Console.WriteLine("Incorrect format of date, enter the date in format mm/dd/yyyy please.");
                    Console.Write("Date of birth: ");
                }

                isValid = dateOfBirth >= minDate && dateOfBirth <= DateTime.Now;

                if (!isValid)
                {
                    Console.WriteLine(
                          "Date of birth must be no earlier than January 1, 1950 " +
                          "and no later than the current date");
                }
            }
            while (!isValid);

            return dateOfBirth;
        }

        private static void List(string parameters)
        {
            var list = fileCabinetService.GetRecords();

            if (list is null || list.Length == 0)
            {
                Console.WriteLine("There are no records yet");
                return;
            }

            for (int i = 0; i < list.Length; i++)
            {
                Console.WriteLine(
                    $"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, " +
                    $"{list[i].DateOfBirth.ToString("yyyy-MMM-d", cultureInfo)}, Age {list[i].FullAge}, " +
                    $"Sex - {list[i].Sex}, Balance {list[i].AccountBalance.ToString(cultureInfo)}");
            }
        }
    }
}