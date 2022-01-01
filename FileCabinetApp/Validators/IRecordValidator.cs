namespace FileCabinetApp
{
    /// <summary>
    /// Define the functionality of validators type.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validate parameters of record.
        /// </summary>
        /// <param name="parametresOfRecord">ParametresOfRecord instance.</param>
        void ValidateParameters(ParametresOfRecord parametresOfRecord);
    }
}