using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanFranGeoGrub.Data;
using Socrata;

namespace SanFranGeoGrub.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class GrubController : ControllerBase
    {

        private readonly ILogger<GrubController> _logger;

        public GrubController(ILogger<GrubController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAllTrucks")]
        public async Task<IEnumerable<FoodTruck>> GetAllTrucks()
        {
            return await TruckTracker.Instance.GetAllTrucks();
        }

        [HttpGet("GetSpecificTruck/{id}")]

    public async Task<FoodTruck> GetThisTruck(string id)
        {
            var trucks = await TruckTracker.Instance.GetAllTrucks();
            return trucks.Single(x => x.__id.Equals(id));
        }
    }
}