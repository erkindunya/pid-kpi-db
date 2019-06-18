using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PID.Data.Mappers;
using PID.Data;
using PID.Common;

namespace PID.Jobs
{
    public static class SummaryDataETL
    {
        [FunctionName("SummaryDataETL")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            IActionResult output;
            var trasnformed = default(string);
            var mapper = new SummaryDataMapper();

            log.LogInformation("Starting PID Summary Data ETL Job");
            log.LogInformation($"Request Size is {req.ContentLength}");

            try
            {
                string payload = Utiliy.ExtractRequestPayload(req);
                trasnformed = Utiliy.TransformPayload<SummaryRecord>(payload, mapper, log);
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
