using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

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

        public async Task WriteClientListToCsvAsync(List<Client> clients)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);

            if(!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8))
                {
                    using(var writer = new CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                    {
                        await Task.Run(() =>
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
                        });                    
                    }
                }
            }

        }

        public async Task<List<Client>> ReadClientListFromCsvAsync()
        {
            

            DirectoryInfo dirInfo = new DirectoryInfo(_pathToDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(_pathToDirectory, _csvFileName);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    using(var reader = new CsvReader(streamReader, CultureInfo.CurrentCulture))
                    {
                        return await Task.Run(() => reader.GetRecords<Client>().ToList());
                    }
                }
            }

        }


        public async Task ClientSerializationWriteToFileAsync(List<Client> client, string pathToDirectory, string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, fileName);
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                string clientSerialize = JsonConvert.SerializeObject(client);
                await writer.WriteAsync(clientSerialize);
            }

        }

        public async Task<List<Client>> ClientDeserializationReadFromFileAsync(string pathToDirectory, string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathToDirectory);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            string fullPath = Path.Combine(pathToDirectory, fileName);
            using (StreamReader reader = new StreamReader(fullPath))
            {
                string clientSerialize = await reader.ReadToEndAsync();
                var clientDeserialize = JsonConvert.DeserializeObject<List<Client>>(clientSerialize);
                return clientDeserialize;
            }

        }
    }
}
