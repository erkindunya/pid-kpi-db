using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PID
{
    public static class SummaryDataETL
    {
        [FunctionName("SummaryDataETL")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            IActionResult output;
            var trasnformed = default(string);
            log.LogInformation("Starting PID Summary Data ETL Job");
            log.LogInformation($"Request Size is {req.ContentLength}");

            try {
                string payload = Utiliy.ExtractRequestPayload(req);
                trasnformed = Utiliy.TransformPayload(payload);
            }
            catch ( ArgumentNullException exception)
            {
                log.LogError(exception.Message);
            }
            catch (ArgumentException exception)
            {
                log.LogError(exception.Message);
            }
            finally
            {
                output = (trasnformed != string.Empty) ?
                    (ActionResult)new OkObjectResult($"{trasnformed}")
                    :new BadRequestObjectResult("Transforming PID data failed.");
            }
            return output;
        }
    }


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

        public static string TransformPayload<T>(string payload)
        {
            /*
             * Test Payload
             * Convert Payload to Bytes Array
             * Upload Bytes Payload to Memory Stream
             * Setup CSV Reader on Payload Memory stream
             * Setup headers
             * Loop Read the CSV
             * Load the Mapper Function
             */


            throw new NotImplementedException();
        }

      }



}
