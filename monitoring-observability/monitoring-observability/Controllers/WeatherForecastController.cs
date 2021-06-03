using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace monitoring_observability.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private static readonly Counter weatherForecastcounter = Metrics.CreateCounter("weather_forecast_fetch", "Number of requests for fetch weather forecast");

        private static readonly Gauge JobsInQueue = Metrics
        .CreateGauge("weather_forecast_queued", "Number of jobs waiting for processing in the queue.");

        private static readonly Summary RequestSizeSummary = Metrics
        .CreateSummary("weather_forecast_track_number_input", "Summary of number parameter of track_input method");

        private static readonly Histogram OrderValueHistogram = Metrics
        .CreateHistogram("weather_forecast_rand", "Histogram of random numbers.",
        new HistogramConfiguration
        {
            // We divide measurements in 10 buckets of 100 each, up to $1000.
            Buckets = Histogram.LinearBuckets(start: 1, width: 100, count: 10)
        });

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Getting weather initiated");
            weatherForecastcounter.Inc();
            JobsInQueue.Inc();
            var rng = new Random();

            try
            {
                var rng1 = new Random();
                int number = rng1.Next(1, 50);
                if ( number< 20)
                {
                    throw new Exception(string.Format("random number {0}", number));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Get waeter error occurred");
            }


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        //
        [HttpGet]
        [Route("track_input")]
        public string TrackInput(int number)
        {
            _logger.LogInformation("track inputs initiated");
            // to mimic summary metric
            RequestSizeSummary.Observe(number);
            // to mimic histrogram metric
            var rng = new Random(); 
            OrderValueHistogram.Observe(rng.Next(1, 1000));
            return "s";
        }
    }
}
