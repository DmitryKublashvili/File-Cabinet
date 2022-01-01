using System;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Import command handler.
    /// </summary>
    public class ImportCommandHandler : CommandHandlerBase
    {
        private const string IncorrectCommandFormatMessage = "Comand has incorrect format.";

        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public ImportCommandHandler(IFileCabinetService service)
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

            if (request.Command == "import")
            {
                if (request.Parameters is null)
                {
                    Console.WriteLine(IncorrectCommandFormatMessage);
                    return;
                }

                var parameters = request.Parameters.Split(" ");

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
                    Console.WriteLine($"Import error: file {request.Parameters} does not exist");
                    return;
                }

                if (parameters[0].ToUpperInvariant() == "CSV")
                {
                    this.ImportCSV(filePath);
                }
                else
                {
                    this.ImportXML(filePath);
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private void ImportXML(string filePath)
        {
            Console.WriteLine($"Import XML started from {filePath}");

            var snapShot = new FileCabinetServiceSnapshot();
            snapShot.LoadFromXML(new StreamReader(filePath));

            this.CommonImportCompletion(snapShot, filePath);
        }

        private void ImportCSV(string filePath)
        {
            Console.WriteLine($"Import CSV started from {filePath}");

            var snapShot = new FileCabinetServiceSnapshot();
            snapShot.LoadFromCSV(new StreamReader(filePath));

            this.CommonImportCompletion(snapShot, filePath);
        }

        private void CommonImportCompletion(FileCabinetServiceSnapshot snapShot, string filePath)
        {
            var validationViolations = this.service.Restore(snapShot);

            int countOfViolations = 0;

            foreach (var item in validationViolations)
            {
                Console.WriteLine($"Violation validations rules. Id: {item.id}, Message: {item.exceptionMessage}. This record will be skiped.");
                countOfViolations++;
            }

            Console.WriteLine(snapShot.GetState().Count - countOfViolations + " records were imported from " + filePath);
        }
    }
}