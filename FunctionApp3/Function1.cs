using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FunctionsApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var Data = JsonSerializer.Deserialize<Request>(requestBody);

                if (Data == null || string.IsNullOrWhiteSpace(Data.name))
                {
                    return new BadRequestObjectResult("Invalid input: name, a, and b are required.");
                }

                string name = Data.name;
                int a = Data.a;
                int b = Data.b;

                int sum = a + b;

                string responseMessage = $"Oleksandr Priadko: Hei {name}. Lukujen {a} ja {b} summa on {sum}.";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing request: {ex.Message}");
                return new BadRequestObjectResult("An error occurred while processing the request.");
            }
        }
    }
    public class Request
    {
        public string name { get; set; }
        public int a { get; set; }
        public int b { get; set; }
    }
}