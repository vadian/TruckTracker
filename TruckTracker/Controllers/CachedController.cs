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
    [HttpGet("", Name = "Get All Locations")]
    public async Task<IEnumerable<FoodTruck>> GetAllTrucks() =>
        await CachedTruckTracker.Instance.GetAllTrucks();

    [HttpGet("applicant/{name}", Name = "Get Location by Applicant Name")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicant(string name) =>
        await CachedTruckTracker.Instance.GetTruckByName(name);

    [HttpGet("applicant/{name}/{status}", Name = "Get Location by Applicant Name and Status")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicantAndStatus(string name, string status)
        => await CachedTruckTracker.Instance.GetTruckByName(name, status);
    
    [HttpGet("street/{street}", Name = "Get Location by Street Name")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(string street)
        => await CachedTruckTracker.Instance.GetTruckByStreet(street);

    [HttpGet("location/{latitude}/{longitude}", Name = "Get Nearby Accepted Locations")]
    public async Task<IEnumerable<FoodTruck>> GetNearbyAccepted(decimal latitude, decimal longitude) =>
        await CachedTruckTracker.Instance.GetTrucksNear(latitude, longitude);

    [HttpGet("location/{latitude}/{longitude}/includeAll", Name = "Get Nearby Locations")]
    public async Task<IEnumerable<FoodTruck>> GetAllNearby(decimal latitude, decimal longitude)
    => await CachedTruckTracker.Instance.GetTrucksNear(latitude, longitude, includeAll: true);
}