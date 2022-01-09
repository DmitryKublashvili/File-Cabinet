using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command handler.
    /// </summary>
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public DeleteCommandHandler(IFileCabinetService service)
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

            if (request.Command == "delete")
            {
                string[] userInputParameters = request.Parameters.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (userInputParameters[0].ToUpperInvariant() != "WHERE")
                {
                    Console.WriteLine("Incorrect command. 'where' expected.");
                    return;
                }

                string userInputParameterName = userInputParameters[1].Split('=')[0].ToUpperInvariant();

                RecordParameter parameterForDeleting = GetParameterForDeleting(userInputParameterName);

                if (parameterForDeleting == RecordParameter.None)
                {
                    Console.WriteLine($"Incorrect parameter {userInputParameterName}");
                    return;
                }

                string userInputParameterValue = userInputParameters[1].Split('=')[1].Trim('\'');

                int[] idsOfDeletedRecords;

                switch (parameterForDeleting)
                {
                    case RecordParameter.Id:

                        if (!int.TryParse(userInputParameterValue, out int id))
                        {
                            Console.WriteLine("Incorrect Id input");
                            return;
                        }

                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<int>(parameterForDeleting, id);
                        break;
                    case RecordParameter.FirstName:
                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<string>(parameterForDeleting, userInputParameterValue);
                        break;
                    case RecordParameter.LastName:
                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<string>(parameterForDeleting, userInputParameterValue);
                        break;
                    case RecordParameter.DateOfBirth:

                        if (!DateTime.TryParse(userInputParameterValue, new CultureInfo("en"), DateTimeStyles.None, out DateTime dateOfBirth))
                        {
                            Console.WriteLine("Incorrect DateOfBirth input");
                            return;
                        }

                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<DateTime>(parameterForDeleting, dateOfBirth);
                        break;
                    case RecordParameter.Sex:

                        if (userInputParameterValue.ToUpperInvariant() != "M" && userInputParameterValue.ToUpperInvariant() != "F")
                        {
                            Console.WriteLine("Incorrect Sex input");
                            return;
                        }

                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<char>(parameterForDeleting, char.ToUpperInvariant(userInputParameterValue[0]));
                        break;
                    case RecordParameter.Salary:

                        if (!decimal.TryParse(userInputParameterValue, out decimal salary))
                        {
                            Console.WriteLine("Incorrect Salary input");
                            return;
                        }

                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<decimal>(parameterForDeleting, salary);
                        break;
                    case RecordParameter.YearsOfService:

                        if (!short.TryParse(userInputParameterValue, out short yearsOfService))
                        {
                            Console.WriteLine("Incorrect YearsOfService input");
                            return;
                        }

                        idsOfDeletedRecords = this.service.RemoveAllRecordsByParameter<short>(parameterForDeleting, yearsOfService);
                        break;
                    default:
                        idsOfDeletedRecords = Array.Empty<int>();
                        break;
                }

                if (idsOfDeletedRecords is null || idsOfDeletedRecords.Length == 0)
                {
                    Console.WriteLine("No matches were found.");
                    return;
                }

                StringBuilder report = new StringBuilder("Records");

                for (int i = 0; i < idsOfDeletedRecords.Length; i++)
                {
                    report.Append($" #{idsOfDeletedRecords[i]},");
                }

                report.Remove(report.Length - 1, 1);
                report.AppendLine(" are deleted.");

                Console.WriteLine(report);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }

        private static RecordParameter GetParameterForDeleting(string parameterName)
        {
            switch (parameterName)
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
    }
}