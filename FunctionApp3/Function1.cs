using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp3
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            string name = req.Query["name"];
            string aString = req.Query["a"];
            string bString = req.Query["b"];

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(aString) || string.IsNullOrEmpty(bString))
            {
                string requestBody = Convert.ToString(new StreamReader(req.Body));
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;
                aString = aString ?? data?.a;
                bString = bString ?? data?.b;
            }

            if (string.IsNullOrEmpty(name) || !int.TryParse(aString, out int a) || !int.TryParse(bString, out int b))
            {
                return new BadRequestObjectResult("Please provide valid 'name', 'a', and 'b'");
            }

            return new OkObjectResult($"Hei {name}. Lukujen {a} ja {b} summa on {a + b}.");
        }
    }
}
