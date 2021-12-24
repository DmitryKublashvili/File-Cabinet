using FileCabinetApp;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Special class for XmlRoot attribut implementation.
    /// </summary>
    [XmlRoot("Records")]
    public class RecordsList
    {
        /// <summary>
        /// Empty ctor for serialization.
        /// </summary>
        public RecordsList()
        {
        }

        /// <summary>
        /// Records list.
        /// </summary>
        public List<FileCabinetRecord> Records { get; set; }
    }
}
