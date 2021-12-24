using FileCabinetApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Using to write records to file in XML format.
    /// </summary>
    static class XmlExporter
    {
        /// <summary>
        /// Writes records to file in XML format.
        /// </summary>
        /// <param name="state">State for recording.</param>
        public static bool ExportInXmlFile(string fileName, IEnumerable<FileCabinetRecord> records, out string exceptionMessage)
        {
            RecordsList recordsList = new RecordsList() { Records = new List<FileCabinetRecord>(records) };

            try
            {
                XmlSerializer xml = new XmlSerializer(recordsList.GetType());

                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    xml.Serialize(sw, recordsList);
                }
            }
            catch (Exception e)
            {
                exceptionMessage = e.Message;
                return false;
            }

            exceptionMessage = string.Empty;

            return true;
        }
    }
}
