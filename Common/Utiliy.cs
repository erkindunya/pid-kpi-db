using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using CsvHelper;
using System.Collections.Generic;
using PID.Common;

namespace PID.Common
{
    public class Utiliy
    {
        public static string ExtractRequestPayload(HttpRequest request)
        {
            var body = default(string);

            if (request == null)
                throw new ArgumentNullException("Utility.ExtractRequestPayLoad : request is null");
            if (request.Body == null || request.Body.Length == 0)
                throw new ArgumentException("Utility.ExtractRequestPayLoad : request body is empty");

            using (var reader = new StreamReader(request.Body))
            {
                body = reader.ReadToEnd();

            }
            return body;

        }

        public static string TransformPayload<T>(string payload, IPIDMapper mapper, ILogger log)
        {


            if (string.IsNullOrEmpty(payload))
            {
                throw new ArgumentException("Utility.TransformPayload : Payload is empty ", nameof(payload));
            }

            var output = default(string);
            var buffer = new MemoryStream(Encoding.ASCII.GetBytes(payload.ToCharArray()));
            using (var bufferReader = new StreamReader(buffer))
            {
                using (var csvReader = new CsvReader(bufferReader))
                {
                    var counter = 0;
                    var records = new List<T>();

                    SetupCsvReader(csvReader);
                    counter = ReadCsv(mapper, log, csvReader, counter, records);

                    log.LogInformation($"Utility.TransformPayload :  Number or records in CSV {counter}" +
                        $"  Number of records that are processed and to be imported {records.Count}");

                    if (records.Count != 0)
                        output = ConvertToJson(records);
                }
            }
            return output;
        }

        private static void SetupCsvReader(CsvReader csvReader)
        {
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Read();
            csvReader.ReadHeader();
        }

        private static int ReadCsv<T>(IPIDMapper mapper, ILogger log, CsvReader csvReader, int index, List<T> records)
        {
            while (csvReader.Read())
            {
                index++;
                try
                {
                    var record = (T)mapper.Map(csvReader);
                    records.Add(record);
                }
                catch (Exception exception)
                {
                    log.LogWarning($"Utility.TransformPayload : Unable to map or read data at record number {index}. More information are here : {exception.Message}");
                }
            }

            return index;
        }

        private static string ConvertToJson<T>(List<T> records)
        {
            string output;
            try
            {
                output = JsonConvert.SerializeObject(records);
            }
            catch (Exception exception)
            {
                throw new Exception($"Utility.ConvertToJson : Unable to convert the Records to JSON. There are {records.Count} records to be imported. More information : {exception.Message} ");
            }

            return output;
        }
    }



}
