using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TRD2022.Hangfire.Infra.DAL
{
    public class FileCommunication
    {
        private string folderPath = "";
        private string filePath = "";

        public FileCommunication(string trdPath)
        {
            folderPath = trdPath;
        }

        /// <summary>
        /// Get data from the csv file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>each line from the file</returns>
        public List<string> GetDataFromCsv(string fileName)
        {
            try
            {
                filePath = $"{folderPath}\\{fileName}.csv";
                List<string> result = new List<string>();

                if (!Directory.Exists(folderPath))
                    return null;

                if (!File.Exists(filePath))
                    return null;

                result = File.ReadAllLines(filePath).Skip(1).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetJsonContent(string path)
        {
            string json = File.ReadAllText(path);
            return json;
        }

        public void WriteOnFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
