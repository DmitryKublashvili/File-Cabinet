using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command handler.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "update")
            {
                string[] splitedUserInput = request.Parameters.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (splitedUserInput[0].ToUpperInvariant() != "SET")
                {
                    Console.WriteLine("Incorrect command, 'set' word expected.");
                    return;
                }

                if (!splitedUserInput[1].Contains("where"))
                {
                    Console.WriteLine("Incorrect command, 'where' word expected.");
                    return;
                }

                string[] splitedDataForSearchAndUpdate = splitedUserInput[1].Split("where");

                List<(RecordParameter parameter, object value)> dataForUpdate = new List<(RecordParameter parameter, object value)>();
                List<(RecordParameter parameter, object value)> dataForSearching = new List<(RecordParameter parameter, object value)>();

                try
                {
                    // getting parameters and values to update
                    FillDataParametersAndValues(dataForUpdate, splitedDataForSearchAndUpdate[0]);

                    // getting parameters and values to search
                    splitedDataForSearchAndUpdate[1] = splitedDataForSearchAndUpdate[1].Replace("and", string.Empty);

                    FillDataParametersAndValues(dataForSearching, splitedDataForSearchAndUpdate[1]);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                var idsOfUpdatingRecords = this.service.UpdateRecordsByParameters(dataForSearching.ToArray(), dataForUpdate.ToArray());

                if (idsOfUpdatingRecords is null || idsOfUpdatingRecords.Length == 0)
                {
                    Console.WriteLine("No matches were found.");
                    return;
                }

                Console.WriteLine(GetReport(idsOfUpdatingRecords));
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static void FillDataParametersAndValues(List<(RecordParameter parameter, object value)> dataForUpdate, string userInputParameters)
        {
            var splittedParametersAndValues = userInputParameters.Split(new[] { ',', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splittedParametersAndValues.Length; i += 2)
            {
                var parameter = GetParameter(splittedParametersAndValues[i]);

                dataForUpdate.Add((parameter, GetValueByParameter(parameter, splittedParametersAndValues[i + 1].Trim('\''))));
            }
        }

        private static string GetReport(int[] idsOfUpdatingRecords)
        {
            StringBuilder report = new StringBuilder("Records");

            for (int i = 0; i < idsOfUpdatingRecords.Length; i++)
            {
                report.Append($" #{idsOfUpdatingRecords[i]},");
            }

            report.Remove(report.Length - 1, 1);
            report.AppendLine(" were updated.");

            return report.ToString();
        }

        private static RecordParameter GetParameter(string parameterName)
        {
            switch (parameterName.ToUpperInvariant())
            {
                case "ID": return RecordParameter.Id;
                case "FIRSTNAME": return RecordParameter.FirstName;
                case "LASTNAME": return RecordParameter.LastName;
                case "DATEOFBIRTH": return RecordParameter.DateOfBirth;
                case "SEX": return RecordParameter.Sex;
                case "SALARY": return RecordParameter.Salary;
                case "YEARSOFSERVICE": return RecordParameter.YearsOfService;
                default: return RecordParameter.None;
            }
        }

        private static object GetValueByParameter(RecordParameter parameter, string userInputParameterValue)
        {
            switch (parameter)
            {
                case RecordParameter.Id:

                    if (!int.TryParse(userInputParameterValue, out int id))
                    {
                        throw new ArgumentException($"Incorrect Id '{userInputParameterValue}' input");
                    }

                    return id;

                case RecordParameter.FirstName: return userInputParameterValue;
                case RecordParameter.LastName: return userInputParameterValue;

                case RecordParameter.DateOfBirth:

                    if (!DateTime.TryParse(userInputParameterValue, new CultureInfo("en"), DateTimeStyles.None, out DateTime dateOfBirth))
                    {
                        throw new ArgumentException($"Incorrect date of birth '{userInputParameterValue}' input");
                    }

                    return dateOfBirth;

                case RecordParameter.Sex:

                    if (userInputParameterValue.ToUpperInvariant() != "M" && userInputParameterValue.ToUpperInvariant() != "F")
                    {
                        throw new ArgumentException($"Incorrect sex '{userInputParameterValue}' input");
                    }

                    return char.ToUpperInvariant(userInputParameterValue[0]);

                case RecordParameter.Salary:

                    if (!decimal.TryParse(userInputParameterValue, out decimal salary))
                    {
                        throw new ArgumentException($"Incorrect salary '{userInputParameterValue}' input");
                    }

                    return salary;

                case RecordParameter.YearsOfService:

                    if (!short.TryParse(userInputParameterValue, out short yearsOfService))
                    {
                        throw new ArgumentException($"Incorrect yearsOfService '{userInputParameterValue}' input");
                    }

                    return yearsOfService;

                default: return null;
            }
        }
    }
}