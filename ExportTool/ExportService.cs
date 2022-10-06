using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }

        public ExportService(string pathToDirectory, string csvFileName)
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }

        public void WriteClientListToCsv(List<Client> clients)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);

            if(!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = GetFullPathToFile(_pathToDirectory, _csvFileName);
            using(FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    using(var writer = new CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                    {
                        writer.WriteField("Id");
                        writer.WriteField("FirstName");
                        writer.WriteField("LastName");
                        writer.WriteField("PassportID");
                        writer.WriteField("DateOfBirth");
                        writer.WriteField("PhoneNumber");
                        writer.WriteField("Bonus");

                        writer.NextRecord();

                        foreach (Client client in clients)
                        {
                            writer.WriteField(client.Id);
                            writer.WriteField(client.FirstName);
                            writer.WriteField(client.LastName);
                            writer.WriteField(client.PassportID);
                            writer.WriteField(client.DateOfBirth);
                            writer.WriteField(client.PhoneNumber);
                            writer.WriteField(client.Bonus);

                            writer.NextRecord();
                        }

                        writer.Flush();
                    }
                }
            }

        }

        public List<Client> ReadClientListFromCsv()
        {
            

            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = GetFullPathToFile(_pathToDirectory, _csvFileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    using(var reader = new CsvReader(streamReader, CultureInfo.CurrentCulture))
                    {
                        return reader.GetRecords<Client>().ToList();
                    }
                }
            }

        }

        private string GetFullPathToFile(string pathToDirectory, string csvFileName)
        {
            return Path.Combine(pathToDirectory, csvFileName);
        }

        
    }
}
