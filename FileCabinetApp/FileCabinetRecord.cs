using System;
using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Implements model of file cabinet record.
    /// </summary>
    [XmlRoot(ElementName ="Record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets record ID.
        /// </summary>
        /// <value> Integer num of record.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        /// <value> String value of first name.
        /// </value>
        [XmlAttribute]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        /// <value> String value of last name.
        /// </value>
        [XmlAttribute]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        /// <value> DateTime value of birth date.
        /// </value>
        [XmlIgnore]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets value for (de)serialisation property DateOfBirth in short date format.
        /// </summary>
        /// <value>
        /// Value for (de)serialisation property DateOfBirth in short date format.
        /// </value>
        [XmlElement("DateOfBirth")]
        public string DateOfBirthString
        {
            get => this.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            set => this.DateOfBirth = DateTime.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets sex of person.
        /// </summary>
        /// <value> Char (one letter of M or F) representation of person gender.
        /// </value>
        [XmlIgnore]
        public char Sex { get; set; }

        /// <summary>
        /// Gets or sets value for (de)serialisation property Sex in string format.
        /// </summary>
        /// <value>
        /// Value for (de)serialisation property Sex in string format.
        /// </value>
        [XmlElement("Sex")]
        public string SexString
        {
            get => this.Sex.ToString();
            set => this.Sex = value is null ? throw new ArgumentNullException(nameof(value), "Sex can't be null.") : value[0];
        }

        /// <summary>
        /// Gets or sets salary.
        /// </summary>
        /// <value> Decimal amount of salary.
        /// </value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets count years of service.
        /// </summary>
        /// <value> Short (integer) count years of service.
        /// </value>
        public short YearsOfService { get; set; }

        /// <summary>
        /// Gets string record representation.
        /// </summary>
        /// <returns>string record representation.</returns>
        public override string ToString()
        {
            return " Id: " + this.Id + " " + this.FirstName + " " + this.LastName + " " + this.DateOfBirth.ToShortDateString() +
                " Sex: " + this.Sex + " Salary: " + this.Salary + " Years in service: " + this.YearsOfService;
        }
    }
}