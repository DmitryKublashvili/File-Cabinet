﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Provides parameters for creating and editing records.
    /// </summary>
    public class ParametresOfRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParametresOfRecord"/> class.
        /// </summary>
        /// <param name="id">Integer num of record.</param>
        /// <param name="firstName">String value of first name.</param>
        /// <param name="lastName">String value of last name.</param>
        /// <param name="dateOfBirth">DateTime value of birth date.</param>
        /// <param name="sex">Char (one letter of M or F) representation of person gender.</param>
        /// <param name="salary">Decimal amount of salary.</param>
        /// <param name="yearsOfService">Short (integer) count years of service.</param>
        public ParametresOfRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char sex, decimal salary, short yearsOfService)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Sex = sex;
            this.Salary = salary;
            this.YearsOfService = yearsOfService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametresOfRecord"/> class.
        /// </summary>
        /// <param name="firstName">String value of first name.</param>
        /// <param name="lastName">String value of last name.</param>
        /// <param name="dateOfBirth">DateTime value of birth date.</param>
        /// <param name="sex">Char (one letter of M or F) representation of person gender.</param>
        /// <param name="salary">Decimal amount of salary.</param>
        /// <param name="yearsOfService">Short (integer) count years of service.</param>
        public ParametresOfRecord(string firstName, string lastName, DateTime dateOfBirth, char sex, decimal salary, short yearsOfService)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Sex = sex;
            this.Salary = salary;
            this.YearsOfService = yearsOfService;
        }

        /// <summary>
        /// Gets record ID.
        /// </summary>
        /// <value> Integer num of record.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets first name.
        /// </summary>
        /// <value> String value of first name.
        /// </value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets last name.
        /// </summary>
        /// <value> String value of last name.
        /// </value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets date of birth.
        /// </summary>
        /// <value> DateTime value of birth date.
        /// </value>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets sex of person.
        /// </summary>
        /// <value> Char (one letter of M or F) representation of person gender.
        /// </value>
        public char Sex { get; private set; }

        /// <summary>
        /// Gets salary.
        /// </summary>
        /// <value> Decimal amount of salary.
        /// </value>
        public decimal Salary { get; private set; }

        /// <summary>
        /// Gets count years of service.
        /// </summary>
        /// <value> Short (integer) count years of service.
        /// </value>
        public short YearsOfService { get; private set; }
    }
}