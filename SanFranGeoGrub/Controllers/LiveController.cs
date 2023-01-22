using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanFranGeoGrub.Data;
using Socrata;

namespace SanFranGeoGrub.Controllers;

[AllowAnonymous]
[ApiController]
[Route("TruckTracker/api/v1/live")]
public class LiveController : ControllerBase
{

    private readonly ILogger<LiveController> _logger;

    public LiveController(ILogger<LiveController> logger)
    {
        _logger = logger;
    }

    [HttpGet("trucks/search/applicant/{name}", Name = "Get Location by Applicant Name from cache")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicant(string name)
    {
        var trucks = await LiveTruckTracker.Instance.GetTruckByName(name);
        return trucks;
    }

    [HttpGet("trucks/search/street/{street}", Name = "Get Location by Street Name from cache")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(string street)
    {
        var trucks = await LiveTruckTracker.Instance.GetTruckByStreet(street);
        return trucks;
    }
}