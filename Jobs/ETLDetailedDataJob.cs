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
using PID.Data.Mappers;
using PID.Common;

namespace PID.Jobs
{
    public static class PIDDataTransformJob
    {
        [FunctionName("DetailedDataETL")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request Updated.");

            IActionResult output;
            var trasnformed = default(string);
            var mapper = new DetailedDataMapper();

            log.LogInformation("Starting PID Detailed Data ETL Job");
            log.LogInformation($"Request Size is {req.ContentLength}");

            try
            {
                string payload = Utiliy.ExtractRequestPayload(req);
                trasnformed = Utiliy.TransformPayload<DetailedRecord>(payload, mapper, log);
            }
            catch (ArgumentNullException exception)
            {
                log.LogError(exception.Message);
            }
            catch (ArgumentException exception)
            {
                log.LogError(exception.Message);
            }
            finally
            {
                output = trasnformed != string.Empty ?
                    (ActionResult)new OkObjectResult($"{trasnformed}")
                    : new BadRequestObjectResult("Transforming PID data failed.");
            }
            return output;
        }



    }
    }
