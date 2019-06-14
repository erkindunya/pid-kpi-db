using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using PID.Data;

namespace Kier.PIDDataTransformer
{
    public static class PIDDataTransformJob
    {
        [FunctionName("PIDDataTransformJob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request Updated.");

            //string name = req.Query["name"];
            var inputStream = req.Body;
            var inputData = string.Empty;
            
            using ( var streamReader = new StreamReader(inputStream))
            {
                inputData = streamReader.ReadToEnd();
            }

            var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(inputData.ToCharArray()));
            using (var streamReader = new StreamReader(memoryStream))
            {
                using (var csvReader = new CsvReader(streamReader))
                {
                    csvReader.Configuration.HasHeaderRecord = true;
                    var serializedRecords = new List<DetailedRecord>();
                    csvReader.Read();
                    csvReader.ReadHeader();

                    while (csvReader.Read())
                    {
                        var record = new DetailedRecord()
                        {
                            ReportingUnit = csvReader.GetField<string>(0),
                            ReportingUnitName = csvReader.GetField<string>(1),
                            ProjectNumber = csvReader.GetField<string>(2),
                            ProjectName = csvReader.GetField<string>(3),
                            KeyMemberName = csvReader.GetField<string>(4),
                            OracleEmployeeNumber = csvReader.GetField<string>(5),
                            KeyMemberRole = csvReader.GetField<string>(6),
                            CurrentlyActive = csvReader.GetField<string>(7),
                            EffectiveDatesFrom = csvReader.GetField<string>(8),
                            EffectiveDatesTo = csvReader.GetField<string>(9),
                            ActionedByName = csvReader.GetField<string>(10),
                            ActionedByOracleEmployeeNumber = csvReader.GetField<string>(11),
                            Use_In_PID = csvReader.GetField<string>(12)
                        };
                        serializedRecords.Add(record);  
                    }
                   
                                              
                   
                    inputData = JsonConvert.SerializeObject(serializedRecords);
                }
            }
           
                //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                //dynamic data = JsonConvert.DeserializeObject(requestBody);
                ////name = name ?? data?.name;

                return inputData != string.Empty
                    ? (ActionResult)new OkObjectResult($"{inputData}")
                    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
