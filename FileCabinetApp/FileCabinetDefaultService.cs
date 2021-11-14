namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet default service.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates instance of the DefaultValidator.
        /// </summary>
        /// <returns>DefaultValidator instance.</returns>
        public override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
