using System;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Export command handler.
    /// </summary>
    public class ExportCommandHandler : CommandHandlerBase
    {
        private const string IncorrectCommandFormatMessage = "Comand has incorrect format.";

        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public ExportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "export")
            {
                var parameters = request.Parameters.Split(" ");

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
                    var snapShot = this.service.MakeSnapshot();

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
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}