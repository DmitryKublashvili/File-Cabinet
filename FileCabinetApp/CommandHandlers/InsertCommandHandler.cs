using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command handler.
    /// </summary>
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public InsertCommandHandler(IFileCabinetService service)
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

            if (request.Command == "insert")
            {
                int indexOfFirstOpenParenth;
                int indexOfFirstCloseParenth;
                int indexOfSecondOpenParenth;
                int indexOfSecondCloseParenth;

                int id = -1;
                string firstName;
                string lastName;
                DateTime dateOfbirth;
                char sex;
                decimal salary;
                short yearsOfService;

                try
                {
                    indexOfFirstOpenParenth = request.Parameters.IndexOf('(');
                    indexOfFirstCloseParenth = request.Parameters.IndexOf(')');
                    indexOfSecondOpenParenth = request.Parameters.LastIndexOf('(');
                    indexOfSecondCloseParenth = request.Parameters.LastIndexOf(')');

                    List<string> parameterNames = request.Parameters.Substring(indexOfFirstOpenParenth + 1, indexOfFirstCloseParenth - indexOfFirstOpenParenth - 1).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> values = request.Parameters.Substring(indexOfSecondOpenParenth + 1, indexOfSecondCloseParenth - indexOfSecondOpenParenth - 1).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    parameterNames = parameterNames.Select(x => x.ToUpperInvariant()).ToList();
                    values = values.Select(x => x.Trim('\'')).ToList();

                    firstName = values[parameterNames.IndexOf("FIRSTNAME")];
                    values.Remove(firstName);
                    parameterNames.Remove("FIRSTNAME");

                    lastName = values[parameterNames.IndexOf("LASTNAME")];
                    values.Remove(lastName);
                    parameterNames.Remove("LASTNAME");

                    dateOfbirth = DateTime.Parse(values[parameterNames.IndexOf("DATEOFBIRTH")], new CultureInfo("en"));
                    values.RemoveAt(parameterNames.IndexOf("DATEOFBIRTH"));
                    parameterNames.Remove("DATEOFBIRTH");

                    sex = values[parameterNames.IndexOf("SEX")][0];
                    values.RemoveAt(parameterNames.IndexOf("SEX"));
                    parameterNames.Remove("SEX");

                    salary = decimal.Parse(values[parameterNames.IndexOf("SALARY")], new CultureInfo("en"));
                    values.RemoveAt(parameterNames.IndexOf("SALARY"));
                    parameterNames.Remove("SALARY");

                    yearsOfService = short.Parse(values[parameterNames.IndexOf("YEARSOFSERVICE")], new CultureInfo("en"));
                    values.RemoveAt(parameterNames.IndexOf("YEARSOFSERVICE"));
                    parameterNames.Remove("YEARSOFSERVICE");

                    if (parameterNames.Contains("ID"))
                    {
                        id = int.Parse(values[parameterNames.IndexOf("ID")], new CultureInfo("en"));
                        values.RemoveAt(parameterNames.IndexOf("ID"));
                        parameterNames.Remove("ID");
                    }

                    if (parameterNames.Count != 0 || values.Count != 0)
                    {
                        throw new ArgumentException(nameof(request.Parameters));
                    }

                    ParametresOfRecord parametresOfRecord;

                    if (id != -1)
                    {
                        if (this.service.IsRecordExist(id))
                        {
                            Console.WriteLine($"Record with ID {id} already exist.");
                            return;
                        }

                        parametresOfRecord = new ParametresOfRecord(id, firstName, lastName, dateOfbirth, sex, salary, yearsOfService);
                    }
                    else
                    {
                        parametresOfRecord = new ParametresOfRecord(firstName, lastName, dateOfbirth, sex, salary, yearsOfService);
                    }

                    id = this.service.CreateRecord(parametresOfRecord);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Incorrect parameters.\n" + e.Message);
                    return;
                }

                Console.WriteLine($"Record #{id} is created.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}