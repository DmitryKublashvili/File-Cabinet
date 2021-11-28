using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements functions of file cabinet.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator recordValidator;

        private readonly string storageFilePath = "cabinet-records.db";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        public FileCabinetFilesystemService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
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

            long startPosition;

            using (FileStream fileStream = new FileStream(this.storageFilePath, FileMode.OpenOrCreate))
            {
                startPosition = fileStream.Length;

                // short reserved 2
                short reserved = default;
                fileStream.Seek(startPosition, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(reserved));

                // int Id 4
                fileStream.Seek(startPosition + 2, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes((startPosition / 278) + 1));

                // char[] FirstName 120
                fileStream.Seek(startPosition + 6, SeekOrigin.Begin);
                var bytesFirstName = Encoding.ASCII.GetBytes(parametresOfRecord.FirstName.Length <= 60 ? parametresOfRecord.FirstName : parametresOfRecord.FirstName[..60]);
                fileStream.Write(bytesFirstName);

                // char[] LastName 120
                fileStream.Seek(startPosition + 126, SeekOrigin.Begin);
                var bytesLastName = Encoding.ASCII.GetBytes(parametresOfRecord.LastName.Length <= 60 ? parametresOfRecord.LastName : parametresOfRecord.LastName[..60]);
                fileStream.Write(bytesLastName);

                // int Year 4
                fileStream.Seek(startPosition + 246, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Year));

                // int Month 4
                fileStream.Seek(startPosition + 250, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Month));

                // int Day 4
                fileStream.Seek(startPosition + 254, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(parametresOfRecord.DateOfBirth.Day));

                // char Sex 2
                fileStream.Seek(startPosition + 258, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(parametresOfRecord.Sex));

                // decimal Salary 16
                int[] intArray = new int[4];
                decimal.GetBits(parametresOfRecord.Salary).CopyTo(intArray, 0);

                fileStream.Seek(startPosition + 260, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(intArray[0]));
                fileStream.Seek(startPosition + 264, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(intArray[1]));
                fileStream.Seek(startPosition + 268, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(intArray[2]));
                fileStream.Seek(startPosition + 272, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(intArray[3]));

                // short YearsOfService 2
                fileStream.Seek(startPosition + 276, SeekOrigin.Begin);
                fileStream.Write(BitConverter.GetBytes(parametresOfRecord.YearsOfService));

                return (int)(startPosition / 278) + 1;
            }
        }

        /// <summary>
        /// Edits selected (by ID) record.
        /// </summary>
        /// <param name="parametresOfRecord">Parametres of record.</param>
        public void EditRecord(ParametresOfRecord parametresOfRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that date of birth.
        /// </summary>
        /// <param name="searchingDate">Search birth date.</param>
        /// <returns>ReadOnlyCollection of records that have that birth date.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime searchingDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that first name.
        /// </summary>
        /// <param name="firstName">Search first name.</param>
        /// <returns>ReadOnlyCollection of records that have that first name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an ReadOnlyCollection of records that have that last name.
        /// </summary>
        /// <param name="lastName">Search last name.</param>
        /// <returns>ReadOnlyCollection of records that have that last name.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets ReadOnlyCollection of all records.
        /// </summary>
        /// <returns>ReadOnlyCollection of all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets records count.
        /// </summary>
        /// <returns>Records count.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }
    }
}
