using System;

public class FileCabinetRecord
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public short FullAge
    {
        get { return (short)(DateTime.Now.Year - this.DateOfBirth.Year); }
    }

    public decimal AccountBalance { get; set; }

    public char Sex { get; set; }
}