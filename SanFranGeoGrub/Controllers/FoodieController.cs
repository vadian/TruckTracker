using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanFranGeoGrub.Data;
using Socrata;

namespace SanFranGeoGrub.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class FoodieController : ControllerBase
    {

        private readonly ILogger<FoodieController> _logger;

        public FoodieController(ILogger<FoodieController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAllTrucks")]
        public async Task<IEnumerable<FoodTruck>> Get()
        {
            return await TruckTracker.Instance.GetAllTrucks();
        }
    }
}