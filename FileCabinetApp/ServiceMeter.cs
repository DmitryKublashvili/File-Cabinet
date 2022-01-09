using System;
using System.Collections.Generic;
using System.Diagnostics;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Service Meter.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;

        private readonly Stopwatch stopwatch = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="fileCabinetService">File Cabinet Service.</param>
        public ServiceMeter(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        /// <inheritdoc/>
        public int CreateRecord(ParametresOfRecord parametresOfRecord)
        {
            this.StartWatch();

            int result = this.fileCabinetService.CreateRecord(parametresOfRecord);

            this.WatchStop("Create");

            return result;
        }

        /// <inheritdoc/>
        public bool Defragment()
        {
            this.StartWatch();

            bool result = this.fileCabinetService.Defragment();

            this.WatchStop("Defragment");

            return result;
        }

        /// <inheritdoc/>
        public bool EditRecord(ParametresOfRecord parametresOfRecord)
        {
            this.StartWatch();

            bool result = this.fileCabinetService.EditRecord(parametresOfRecord);

            this.WatchStop("EditRecord");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            this.StartWatch();

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByDateOfBirth(searchingDate);

            this.WatchStop("FindByDateOfBirth");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.StartWatch();

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByFirstName(firstName);

            this.WatchStop("FindByFirstName");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.StartWatch();

            IEnumerable<FileCabinetRecord> result = this.fileCabinetService.FindByLastName(lastName);

            this.WatchStop("FindByLastName");

            return result;
        }

        /// <inheritdoc/>
        public IRecordIterator GetRecords()
        {
            this.StartWatch();

            IRecordIterator result = this.fileCabinetService.GetRecords();

            this.WatchStop("GetRecords");

            return result;
        }

        /// <inheritdoc/>
        public (int recordsCount, int deletedRecordsCount) GetStat()
        {
            this.StartWatch();

            (int recordsCount, int deletedRecordsCount) result = this.fileCabinetService.GetStat();

            this.WatchStop("GetStat");

            return result;
        }

        /// <inheritdoc/>
        public bool IsRecordExist(int id)
        {
            this.StartWatch();

            bool result = this.fileCabinetService.IsRecordExist(id);

            this.WatchStop("IsRecordExist");

            return result;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.StartWatch();

            FileCabinetServiceSnapshot result = this.fileCabinetService.MakeSnapshot();

            this.WatchStop("MakeSnapshot");

            return result;
        }

        /// <inheritdoc/>
        public int[] RemoveAllRecordsByParameter<T>(RecordParameter parameterName, T parameterValue)
        {
            this.StartWatch();

            var result = this.fileCabinetService.RemoveAllRecordsByParameter(parameterName, parameterValue);

            this.WatchStop("RemoveAllRecordsByParameter");

            return result;
        }

        /// <inheritdoc/>
        public bool RemoveRecordById(int id)
        {
            this.StartWatch();

            bool result = this.fileCabinetService.RemoveRecordById(id);

            this.WatchStop("RemoveRecordById");

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<(int id, string exceptionMessage)> Restore(FileCabinetServiceSnapshot snapShot)
        {
            this.StartWatch();

            IEnumerable<(int id, string exceptionMessage)> result = this.fileCabinetService.Restore(snapShot);

            this.WatchStop("Restore");

            return result;
        }

        /// <inheritdoc/>
        public int[] UpdateRecordsByParameters<T>((RecordParameter parameter, T value)[] dataForSearch, (RecordParameter parameter, T value)[] dataForUpdate)
        {
            this.StartWatch();

            int[] result = this.fileCabinetService.UpdateRecordsByParameters(dataForSearch, dataForUpdate);

            this.WatchStop("UpdateRecordsByParameters");

            return result;
        }

        private void StartWatch()
        {
            this.stopwatch.Start();
        }

        private void WatchStop(string methodName)
        {
            this.stopwatch.Stop();

            Console.WriteLine(methodName + $" method execution duration is {this.stopwatch.ElapsedTicks} ticks");

            this.stopwatch.Reset();
        }
    }
}
