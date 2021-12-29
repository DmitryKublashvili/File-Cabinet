using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Reads state from XML file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Some text reader.</param>
        public FileCabinetRecordXmlReader(TextReader reader)
        {
            this.Reader = reader;
        }

        /// <summary>
        /// Gets or sets and sets text reader.
        /// </summary>
        /// <value>
        /// Textreader for reading text from file.
        /// </value>
        public TextReader Reader { get; set; }

        /// <summary>
        /// Reads records from file in XML format.
        /// </summary>
        /// <returns>List of loaded records.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5369:Use XmlReader for 'XmlSerializer.Deserialize()'", Justification = "Temp solution.")]
        public IList<FileCabinetRecord> ReadAll()
        {
            RecordsList records = new RecordsList();

            XmlSerializer xml = new XmlSerializer(records.GetType());

            records = (RecordsList)xml.Deserialize(this.Reader);

            this.Reader.Close();

            return records.Records;
        }
    }
}