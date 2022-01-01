using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Printers
{
    /// <summary>
    /// Provides method for printing records.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints records.
        /// </summary>
        /// <param name="records">Records to print.</param>
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
