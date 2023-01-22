using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SanFranGeoGrub.Data;
using Socrata;

namespace SanFranGeoGrub.Controllers;

[AllowAnonymous]
[ApiController]
[Route("TruckTracker/api/v1/cached")]
public class CachedController : ControllerBase
{

    private readonly ILogger<CachedController> _logger;

    public CachedController(ILogger<CachedController> logger)
    {
        _logger = logger;
    }

    [HttpGet("trucks", Name = "Get All Locations")]
    public async Task<IEnumerable<FoodTruck>> GetAllTrucks()
    {
        return await CachedTruckTracker.Instance.GetAllTrucks();
    }

    [HttpGet("trucks/search/applicant/{name}/{status}", Name = "Get Location by Applicant Name")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicant(string name, string? status)
    {
        var trucks = await CachedTruckTracker.Instance.GetTruckByName(name, status);
        return trucks;
    }

    [HttpGet("trucks/search/street/{street}", Name = "Get Location by Street Name")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(string street)
    {
        var trucks = await CachedTruckTracker.Instance.GetTruckByStreet(street);
        return trucks;
    }

    [HttpGet("trucks/search/location/{latitude}/{longitude}/{includeAll}", Name = "Get Nearby Locations")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(decimal latitude, decimal longitude, bool includeAll = false)
    {
        var trucks = await CachedTruckTracker.Instance.GetTrucksNear(latitude, longitude, includeAll);
        return trucks;
    }
}