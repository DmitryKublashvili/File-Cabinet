using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Help command handler.
    /// </summary>
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistic", "The 'stat' command prints statistic." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "insert", "inserts new record with the specified parameters", "The 'inserts' command inserts new record with the specified parameters." },
            new string[] { "update", "updates records by specified parameters and values", "The 'update' command allows to update records by specified parameters and values." },
            new string[] { "list", "shows records information", "The 'list' command shows records information." },
            new string[] { "find", "finds records by the specified parameter", "The 'find' command shows a list of records in which the specified parameter was found." },
            new string[] { "export", "exports current state in file", "The 'export' command exports current state in file according to the specified parameters." },
            new string[] { "import", "imports records from file", "The 'import' command imports records from CSV or XML format file." },
            new string[] { "delete", "deletes records by specified parameter value from storage", "The 'delete' command deletes records by specified parameter value from storage." },
            new string[] { "purge", "performs defragmentation of the storage-file (available only for FileCabinetFilesystemService)", "The 'purge' command performs defragmentation of the storage-file." },
        };

        private readonly int mistakeSensitivity = 1;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Used commands already in lower case.")]
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "help")
            {
                if (!string.IsNullOrEmpty(request.Parameters))
                {
                    var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], request.Parameters, StringComparison.InvariantCultureIgnoreCase));
                    if (index >= 0)
                    {
                        Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                    }
                    else
                    {
                        Console.WriteLine($"There is no explanation for '{request.Parameters}' command.");
                    }
                }
                else
                {
                    Console.WriteLine("Available commands:");

                    foreach (var helpMessage in helpMessages)
                    {
                        Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                    }
                }

                Console.WriteLine();
            }
            else if (IsUnknounCommand(request.Command.ToLowerInvariant()))
            {
                Console.WriteLine(this.GetHelpMessage(request.Command.ToLowerInvariant()));
            }
            else
            {
                this.nextHandler?.Handle(request);
            }
        }

        private static bool IsUnknounCommand(string command)
        {
            for (int i = 0; i < helpMessages.Length; i++)
            {
                if (command == helpMessages[i][0])
                {
                    return false;
                }
            }

            return true;
        }

        private string GetHelpMessage(string userInput)
        {
            // filtering by length
            List<string> nearestCommands = helpMessages
                .Select(x => x[0])
                .Where(command => Math.Abs(command.Length - userInput.Length) <= this.mistakeSensitivity)
                .ToList();

            // filtering by letters
            for (int i = 0; i < nearestCommands.Count; i++)
            {
                int avalibleMistakes = this.mistakeSensitivity + 1;

                for (int j = 0; j < nearestCommands[i].Length; j++)
                {
                    if (!userInput.Contains(nearestCommands[i][j]))
                    {
                        avalibleMistakes--;

                        if (avalibleMistakes == 0)
                        {
                            nearestCommands.Remove(nearestCommands[i]);
                            i--;
                            break;
                        }
                    }
                }
            }

            string mostSimilarCommands = string.Join(Environment.NewLine + "\t", nearestCommands);

            string helpMessage = $"The '{userInput}' is not a File_Cabinet command. See 'help'." +
                Environment.NewLine + $"The most similar commands are:" +
                Environment.NewLine + $"\t{mostSimilarCommands}";

            return helpMessage;
        }
    }
}