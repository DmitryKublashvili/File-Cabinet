using System;
using FileCabinetApp;
using System.Collections.Generic;

namespace FileCabinetGenerator
{
    class RecordsGenerator
    {
        private readonly string fileType;
        private readonly string fileName;
        private readonly int recordsAmount;
        private readonly Random random = new Random();
        private int startId;

        private readonly List<FileCabinetRecord> records = new List<FileCabinetRecord>();

        /// <summary>
        /// Initialize instance of RecordsGenerator type.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="fileName"></param>
        /// <param name="recordsAmount"></param>
        /// <param name="startId"></param>
        public RecordsGenerator(string fileType, string fileName, int recordsAmount, int startId)
        {
            this.fileType = fileType;
            this.fileName = fileName;
            this.recordsAmount = recordsAmount > 0 ? recordsAmount : 0;
            this.startId = startId > 0 ? startId : 0;
        }

        public void CreateRecords()
        {
            for (int i = 0; i < recordsAmount; i++)
            {
                records.Add(GetRandomRecord());
            }
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var item in records)
            {
                yield return item;
            }
        }

        private FileCabinetRecord GetRandomRecord()
        {
            var record = new FileCabinetRecord();

            record.Sex = GetRandomSex();
            record.FirstName = GetRandomName(record.Sex);
            record.LastName = GetRecordLastName(record.Sex);
            record.Id = GetRecordId();
            record.DateOfBirth = GetRandomDateOfBirth();
            record.Salary = GetRandomSalary();
            record.YearsOfService = GetRandomYersOfService();

            return record;
        }

        private string GetRandomName(char sex)
        {
            string[] firstFemaleNames = { "Mary", "Svetlana", "Alexandra", "Anna", "Nataly" };
            string[] firstMaleNames = { "Roman", "Andrey", "Victor", "Sergy", "Paul", "Igor", "Eduard", "Alexandr", "Nikolay", "Uladzimir", "Ales", "Nik", "Mike" };

            return sex is 'F' ? firstFemaleNames[random.Next(firstFemaleNames.Length)] : firstMaleNames[random.Next(firstMaleNames.Length)];
        }

        private string GetRecordLastName(char sex)
        {
            string[] lastFemaleLastNames = { "Kolesnikova", "Tsihanovskaya", "Gerasimenia", "Tsepkalo", "Aleksievich", "Hershe" };
            string[] lastMaleLastNames = { "Bondarenko", "Zelcer", "Babariko", "Tsihanovsky", "Latushko", "Chaly", "Dedok", "Palches", "Losik", "Statkevich", "Nekliaev", "Pushkin" };

            return sex is 'F' ? lastFemaleLastNames[random.Next(lastFemaleLastNames.Length)] : lastMaleLastNames[random.Next(lastMaleLastNames.Length)];
        }

        private int GetRecordId() => startId++;

        private DateTime GetRandomDateOfBirth()
        {
            DateTime date = DateTime.Now;

            date = date.AddDays(random.Next(-364, 1));
            date = date.AddMonths(random.Next(-11, 1));
            date = date.AddYears(random.Next(-60, -16));

            return date;
        }

        private char GetRandomSex() => this.startId % 2 == 0 ? 'F' : 'M';

        private decimal GetRandomSalary() => random.Next(2_000, 100_000);

        private short GetRandomYersOfService() => (short)random.Next(50);
    }
}
