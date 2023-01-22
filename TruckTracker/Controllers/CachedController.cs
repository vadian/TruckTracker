using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruckTracker.Data;
using Socrata;

namespace TruckTracker.Controllers;

[AllowAnonymous]
[ApiController]
[Route("TruckTracker/api/v1/cached")]
public class CachedController : ControllerBase
{
    private readonly ICachedTruckTracker _truckTracker;

    public CachedController(ICachedTruckTracker truckTracker)
    {
        _truckTracker = truckTracker;
    }

    [HttpGet("", Name = "Get All Locations")]
    public async Task<IEnumerable<FoodTruck>> GetAllTrucks() =>
        await _truckTracker.GetAllTrucks();

    [HttpGet("applicant/{name}", Name = "Get Location by Applicant Name")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicant(string name) =>
        await _truckTracker.GetTruckByName(name);

    [HttpGet("applicant/{name}/{status}", Name = "Get Location by Applicant Name and Status")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicantAndStatus(string name, string status)
        => await _truckTracker.GetTruckByName(name, status);
    
    [HttpGet("street/{street}", Name = "Get Location by Street Name")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(string street)
        => await _truckTracker.GetTruckByStreet(street);

    [HttpGet("location/{latitude}/{longitude}", Name = "Get Nearby Accepted Locations")]
    public async Task<IEnumerable<FoodTruck>> GetNearbyAccepted(decimal latitude, decimal longitude) =>
        await _truckTracker.GetTrucksNear(latitude, longitude);

    [HttpGet("location/{latitude}/{longitude}/includeAll", Name = "Get Nearby Locations")]
    public async Task<IEnumerable<FoodTruck>> GetAllNearby(decimal latitude, decimal longitude)
    => await _truckTracker.GetTrucksNear(latitude, longitude, includeAll: true);
}