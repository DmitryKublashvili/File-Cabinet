using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements functions of file cabinet.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private static CultureInfo cultureInfo = new ("en");
        private readonly IRecordValidator recordValidator;
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        /// <param name="fileStream">Instance of fileStream.</param>
        public FileCabinetFilesystemService(IRecordValidator recordValidator, FileStream fileStream)
        {
            this.recordValidator = recordValidator;
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates a record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord.</param>
        /// <returns>New created record.</returns>
        public int CreateRecord(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            this.recordValidator.ValidateParameters(parametresOfRecord);

            long startPosition;

            int id;

            startPosition = this.fileStream.Length;

            // get Id
            id = ((int)startPosition / 278) + 1;

            this.WriteRecordInFile(parametresOfRecord, startPosition);

            return id;
        }

        /// <summary>
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void EditRecord(ParametresOfRecord parametresOfRecord)
        {
            if (parametresOfRecord is null)
            {
                throw new ArgumentNullException(nameof(parametresOfRecord));
            }

            if (!this.IsRecordExist(parametresOfRecord.Id))
            {
                return;
            }

            this.recordValidator.ValidateParameters(parametresOfRecord);

            long startPosition = -1;

            int recordsCount = (int)this.fileStream.Length / 278;

            byte[] bytesFromId = new byte[4];

            // find and set position
            for (int i = 0; i < recordsCount; i++)
            {
                this.fileStream.Seek((i * 278) + 2, SeekOrigin.Begin);
                this.fileStream.Read(bytesFromId);

                if (parametresOfRecord.Id == BitConverter.ToInt32(bytesFromId, 0))
                {
                    startPosition = i * 278;
                    break;
                }

                if (i == recordsCount - 1)
                {
                    Console.WriteLine($"Record {parametresOfRecord.Id} not found.");
                    return;
                }
            }

            this.WriteRecordInFile(parametresOfRecord, startPosition);
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>ReadOnlyCollection of records that have that birth date.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            (int, int, int) searchingDateParams = (searchingDate.Year, searchingDate.Month, searchingDate.Day);

            for (int i = 0; i < this.fileStream.Length; i += 278)
            {
                if (!this.IsDeleted(i))
                {
                    continue;
                }

                byte[] bytesDate = new byte[12];
                this.fileStream.Seek(i + 246, SeekOrigin.Begin);
                this.fileStream.Read(bytesDate);

                (int, int, int) dateForCheck = (BitConverter.ToInt32(bytesDate.AsSpan()[.. 4]), BitConverter.ToInt32(bytesDate.AsSpan()[4 .. 8]), BitConverter.ToInt32(bytesDate.AsSpan()[8 ..]));

                if (searchingDateParams == dateForCheck)
                {
                    records.Add(this.GetRecordFromFile(i));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>ReadOnlyCollection of records that have that first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("Wanted name was null or empty", nameof(firstName));
            }

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0; i < this.fileStream.Length; i += 278)
            {
                if (!this.IsDeleted(i))
                {
                    continue;
                }

                byte[] bytesFirstName = new byte[120];
                this.fileStream.Seek(i + 6, SeekOrigin.Begin);
                this.fileStream.Read(bytesFirstName);
                byte[] adaptedBytes = bytesFirstName.Where(s => s != '/' && s != 0).ToArray();
                string firstNameInRecordToCheck = Encoding.ASCII.GetString(adaptedBytes);

                if (firstNameInRecordToCheck.ToUpperInvariant() == firstName.ToUpperInvariant())
                {
                    records.Add(this.GetRecordFromFile(i));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>ReadOnlyCollection of records that have that last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Wanted name was null or empty", nameof(lastName));
            }

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0; i < this.fileStream.Length; i += 278)
            {
                if (!this.IsDeleted(i))
                {
                    continue;
                }

                byte[] bytesLastName = new byte[120];
                this.fileStream.Seek(i + 126, SeekOrigin.Begin);
                this.fileStream.Read(bytesLastName);
                byte[] adaptedBytes = bytesLastName.Where(s => s != '/' && s != 0).ToArray();
                string lastNameInRecordToCheck = Encoding.ASCII.GetString(adaptedBytes);

                if (lastNameInRecordToCheck.ToUpperInvariant() == lastName.ToUpperInvariant())
                {
                    records.Add(this.GetRecordFromFile(i));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets ReadOnlyCollection of all records.
        /// </summary>
        /// <returns>ReadOnlyCollection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            for (int i = 0; i < this.fileStream.Length; i += 278)
            {
                if (!this.IsDeleted(i))
                {
                    records.Add(this.GetRecordFromFile(i));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            int existingRecordsCount = 0;
            int removerdRecordsCount = 0;

            for (int i = 0; i < this.fileStream.Length; i += 278)
            {
                if (!this.IsDeleted(i))
                {
                    existingRecordsCount++;
                }
                else
                {
                    removerdRecordsCount++;
                }
            }

            return existingRecordsCount;
        }

        /// <summary>
        /// Gets FileCabinetService state on current moment.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Restores state according current state and addition state from snapshot.
        /// </summary>
        /// <param name="snapShot">Snapshot with some addition or new state.</param>
        /// <returns>Information (id, message) about not valid records.</returns>
        public IEnumerable<(int id, string exceptionMessage)> Restore(FileCabinetServiceSnapshot snapShot)
        {
            if (snapShot is null)
            {
                throw new ArgumentException("Snapshot was null", nameof(snapShot));
            }

            ReadOnlyCollection<FileCabinetRecord> newRecords = snapShot.GetState();

            List<(int id, string exceptionMessage)> validationViolations = new List<(int id, string exceptionMessage)>();

            for (int i = 0; i < newRecords.Count; i++)
            {
                try
                {
                    this.recordValidator.ValidateParameters(new ParametresOfRecord(newRecords[i]));
                }
                catch (ValidationException e)
                {
                    validationViolations.Add((e.NotValidRecordId, e.Message));
                    continue;
                }

                if (newRecords[i].Id > this.GetStat())
                {
                    this.CreateRecord(new ParametresOfRecord(newRecords[i]));
                }
                else
                {
                    this.EditRecord(new ParametresOfRecord(newRecords[i]));
                }
            }

            return validationViolations;
        }

        /// <summary>
        /// Removes record by it's ID.
        /// </summary>
        /// <param name="id">ID of record.</param>
        public void RemoveRecordById(int id)
        {
            byte[] bytes = new byte[1] { 5 };

            long startPosition = (id - 1) * 278;
            this.fileStream.Seek(startPosition, SeekOrigin.Begin);
            this.fileStream.Write(bytes);
        }

        /// <summary>
        /// Define is the record exists by it's ID.
        /// </summary>
        /// <param name="id">ID of the record.</param>
        /// <returns>Result bool value.</returns>
        public bool IsRecordExist(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id must be more then 0", nameof(id));
            }

            return !this.IsDeleted(id - 1);
        }

        private void WriteRecordInFile(ParametresOfRecord parametresOfRecord, long startPosition)
        {
            if (startPosition < 0)
            {
                throw new ArgumentNullException(nameof(startPosition));
            }

            // short reserved 2
            short reserved = default;
            this.fileStream.Seek(startPosition, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(reserved));

            // int Id 4
            this.fileStream.Seek(startPosition + 2, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes((startPosition / 278) + 1));

            // char[] FirstName 120
            this.fileStream.Seek(startPosition + 6, SeekOrigin.Begin);
            var bytesFirstName = Encoding.ASCII.GetBytes(parametresOfRecord.FirstName.Length <= 60 ? parametresOfRecord.FirstName : parametresOfRecord.FirstName[..60]);
            this.fileStream.Write(bytesFirstName);

            // char[] LastName 120
            this.fileStream.Seek(startPosition + 126, SeekOrigin.Begin);
            var bytesLastName = Encoding.ASCII.GetBytes(parametresOfRecord.LastName.Length <= 60 ? parametresOfRecord.LastName : parametresOfRecord.LastName[..60]);
            this.fileStream.Write(bytesLastName);

            // int Year 4
            this.fileStream.Seek(startPosition + 246, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Year));

            // int Month 4
            this.fileStream.Seek(startPosition + 250, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Month));

            // int Day 4
            this.fileStream.Seek(startPosition + 254, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Day));

            // char Sex 2
            this.fileStream.Seek(startPosition + 258, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(parametresOfRecord.Sex));

            // decimal Salary 16
            int[] intArray = new int[4];
            decimal.GetBits(parametresOfRecord.Salary).CopyTo(intArray, 0);

            this.fileStream.Seek(startPosition + 260, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(intArray[0]));
            this.fileStream.Seek(startPosition + 264, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(intArray[1]));
            this.fileStream.Seek(startPosition + 268, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(intArray[2]));
            this.fileStream.Seek(startPosition + 272, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(intArray[3]));

            // short YearsOfService 2
            this.fileStream.Seek(startPosition + 276, SeekOrigin.Begin);
            this.fileStream.Write(BitConverter.GetBytes(parametresOfRecord.YearsOfService));
        }

        private FileCabinetRecord GetRecordFromFile(int position)
        {
            FileCabinetRecord record = new FileCabinetRecord();

            byte[] bytes = new byte[278];
            this.fileStream.Seek(position, SeekOrigin.Begin);
            this.fileStream.Read(bytes);

            // get Id
            record.Id = BitConverter.ToInt32(bytes[2..6], 0);

            // get FirstName
            record.FirstName = Encoding.ASCII.GetString(bytes[6..126]).TrimEnd(' ');

            // get LastName
            record.LastName = Encoding.ASCII.GetString(bytes[126..246]).TrimEnd(' ');

            // get DateOfBirth
            int year = BitConverter.ToInt32(bytes[246..250], 0);
            int month = BitConverter.ToInt32(bytes[250..254], 0);
            int day = BitConverter.ToInt32(bytes[254..258], 0);
            record.DateOfBirth = DateTime.Parse($"{month}/{day}/{year}", cultureInfo, DateTimeStyles.AdjustToUniversal);

            // get Sex
            record.Sex = BitConverter.ToChar(bytes[258..260], 0);

            // get Salary
            int int0 = BitConverter.ToInt32(bytes[260..264], 0);
            int int1 = BitConverter.ToInt32(bytes[264..268], 0);
            int int2 = BitConverter.ToInt32(bytes[268..272], 0);
            int int3 = BitConverter.ToInt32(bytes[272..276], 0);
            bool sign = (int3 & 0x80000000) != 0;
            byte scale = (byte)((int3 >> 16) & 0x7F);

            record.Salary = new decimal(int0, int1, int2, sign, scale);

            // get YearsOfService
            record.YearsOfService = BitConverter.ToInt16(bytes[276..278], 0);

            return record;
        }

        private bool IsDeleted(int recordPosition)
        {
            byte[] bytes = new byte[1];
            this.fileStream.Seek(recordPosition, SeekOrigin.Begin);
            this.fileStream.Read(bytes);

            if (bytes[0] == 5)
            {
                return true;
            }

            return false;
        }
    }
}
