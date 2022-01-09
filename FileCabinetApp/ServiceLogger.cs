using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FileCabinetApp.Iterators;

#pragma warning disable CA1062
namespace FileCabinetApp
{
    /// <summary>
    /// Service Meter.
    /// </summary>
    public class ServiceLogger : IFileCabinetService
    {
        private const string FilePath = @"Logs.txt";

        private readonly IFileCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="fileCabinetService">File Cabinet Service.</param>
        public ServiceLogger(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        /// <inheritdoc/>
        public int CreateRecord(ParametresOfRecord parametresOfRecord)
        {
            CollingLogCreate("CreateRecord()", parametresOfRecord);

            int result = this.fileCabinetService.CreateRecord(parametresOfRecord);

            ComplettingLogCreate("CreateRecord()", result.ToString(CultureInfo.InvariantCulture));

            return result;
        }

        /// <inheritdoc/>
        public bool Defragment()
        {
            CollingLogCreate("Defragment()");

            bool result = this.fileCabinetService.Defragment();

            ComplettingLogCreate("Defragment()", result.ToString(CultureInfo.InvariantCulture));

            return result;
        }

        /// <inheritdoc/>
        public bool EditRecord(ParametresOfRecord parametresOfRecord)
        {
            CollingLogCreate("EditRecord(ParametresOfRecord parametresOfRecord)", parametresOfRecord);

            bool result = this.fileCabinetService.EditRecord(parametresOfRecord);

            ComplettingLogCreate("EditRecord(ParametresOfRecord parametresOfRecord)", result.ToString(CultureInfo.InvariantCulture));

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            CollingLogCreate("FindByDateOfBirth(DateTime searchingDate)", searchingDate.ToShortDateString());

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByDateOfBirth(searchingDate);

            ComplettingLogCreate("FindByDateOfBirth(DateTime searchingDate)", $"ReadOnlyCollection<FileCabinetRecord>");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            CollingLogCreate("FindByFirstName(string firstName)", firstName);

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByFirstName(firstName);

            ComplettingLogCreate("FindByFirstName(string firstName)", $"ReadOnlyCollection<FileCabinetRecord>");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            CollingLogCreate("FindByLastName(string lastName)", lastName);

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByLastName(lastName);

            ComplettingLogCreate("FindByLastName(string lastName)", $"ReadOnlyCollection<FileCabinetRecord>");

            return result;
        }

        /// <inheritdoc/>
        public IRecordIterator GetRecords()
        {
            CollingLogCreate("GetRecords()");

            IRecordIterator result = this.fileCabinetService.GetRecords();

            ComplettingLogCreate("GetRecords()", $"ReadOnlyCollection<FileCabinetRecord>");

            return result;
        }

        /// <inheritdoc/>
        public (int recordsCount, int deletedRecordsCount) GetStat()
        {
            CollingLogCreate("GetStat()");

            (int recordsCount, int deletedRecordsCount) result = this.fileCabinetService.GetStat();

            ComplettingLogCreate("GetStat()", $" recordsCount = {result.recordsCount}, deletedRecordsCount = {result.deletedRecordsCount}");

            return result;
        }

        /// <inheritdoc/>
        public bool IsRecordExist(int id)
        {
            CollingLogCreate("IsRecordExist(int id)", id.ToString(CultureInfo.InvariantCulture));

            bool result = this.fileCabinetService.IsRecordExist(id);

            ComplettingLogCreate("IsRecordExist(int id)", result.ToString());

            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            CollingLogCreate("MakeSnapshot()");

            FileCabinetServiceSnapshot result = this.fileCabinetService.MakeSnapshot();

            ComplettingLogCreate("MakeSnapshot()", "FileCabinetServiceSnapshot " + result.GetName());

            return result;
        }

        /// <inheritdoc/>
        public bool RemoveRecordById(int id)
        {
            CollingLogCreate("RemoveRecordById(int id)", id.ToString(CultureInfo.InvariantCulture));

            bool result = this.fileCabinetService.RemoveRecordById(id);

            ComplettingLogCreate("RemoveRecordById(int id)", result.ToString());

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<(int id, string exceptionMessage)> Restore(FileCabinetServiceSnapshot snapShot)
        {
            CollingLogCreate("Restore(FileCabinetServiceSnapshot snapShot)", "FileCabinetServiceSnapshot " + snapShot.GetName());

            IEnumerable<(int id, string exceptionMessage)> result = this.fileCabinetService.Restore(snapShot);

            ComplettingLogCreate("Restore(FileCabinetServiceSnapshot snapShot)", "IEnumerable<(int id, string exceptionMessage)> contains " + result.Count() + " records.");

            return result;
        }

        /// <inheritdoc/>
        public int[] RemoveAllRecordsByParameter<T>(RecordParameter parameterName, T parameterValue)
        {
            CollingLogCreate($"RemoveAllRecordsByParameter(RecordParameter parameterName, T parameterValue)", $"Arguments: {parameterName}, {parameterValue} ");

            int[] result = this.fileCabinetService.RemoveAllRecordsByParameter(parameterName, parameterValue);

            ComplettingLogCreate("RemoveAllRecordsByParameter(RecordParameter parameterName, T parameterValue)", "int[] contains " + result.Length + " values.");

            return result;
        }

        private static void CollingLogCreate(string methodName, ParametresOfRecord parametresOfRecord)
        {
            DateTime collingDateTime = DateTime.Now;

            string arguments = $" FirstName = {parametresOfRecord.FirstName}, LastName = {parametresOfRecord.LastName}, DateOfBirth = {parametresOfRecord.DateOfBirth}";

            string collingLog = $"{collingDateTime} - Colling {methodName}" + " with " + arguments;

            WriteLogInFile(collingLog);
        }

        private static void CollingLogCreate(string methodName, string arguments)
        {
            DateTime collingDateTime = DateTime.Now;

            string collingLog = $"{collingDateTime} - Colling {methodName}" + " with " + arguments;

            WriteLogInFile(collingLog);
        }

        private static void CollingLogCreate(string methodName)
        {
            DateTime collingDateTime = DateTime.Now;

            string collingLog = $"{collingDateTime} - Colling {methodName}";

            WriteLogInFile(collingLog);
        }

        private static void ComplettingLogCreate(string methodName, string result)
        {
            DateTime completingDateTime = DateTime.Now;

            string completingLog = $"{completingDateTime} - {methodName} returned '{result}'";

            WriteLogInFile(completingLog);
        }

        private static void WriteLogInFile(string collingLog)
        {
            File.AppendAllLines(FilePath, new[] { collingLog });
        }
    }
}
