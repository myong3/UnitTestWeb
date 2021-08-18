using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestWeb.Controllers
{
    public class WeatherForecastController : ControllerBase
    {
        public readonly string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public readonly string DebitAmountLessThanZeroMessage = "Debit amount is less than zero";

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController()
        {
        }
    }
}
