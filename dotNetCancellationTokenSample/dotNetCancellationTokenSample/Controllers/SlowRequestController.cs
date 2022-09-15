using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace dotNetCancellationTokenSample.Controllers
{
    public class SlowRequestController: Controller
    {
        private readonly ILogger _logger;
         
        public SlowRequestController(ILogger<SlowRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/slowtest")]
        public async Task<string> GetSlow(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to do slow work");

            // slow async action, e.g. call external api
            await Task.Delay(10_000, cancellationToken);

            var message = "Finished slow delay of 10 seconds.";

            _logger.LogInformation(message);

            return message;
        }


        [HttpGet("/slowtest2")]
        public Task<string> GetSlow2(CancellationToken cancellationToken) 
        { 
            _logger.LogInformation("Starting to do slow work");

            for(var i=0; i<10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // slow non-cancellable work
                Thread.Sleep(1000);
            }
            var message = "Finished slow delay of 10 seconds.";

            _logger.LogInformation(message);

            return Task.FromResult(message);
        }


    }
}
