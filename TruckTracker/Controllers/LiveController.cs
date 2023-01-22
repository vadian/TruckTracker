using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruckTracker.Data;
using Socrata;

namespace TruckTracker.Controllers;

[AllowAnonymous]
[ApiController]
[Route("TruckTracker/api/v1/live")]
public class LiveController : ControllerBase
{
    [HttpGet("applicant/{name}", Name = "Get Location by Applicant Name from cache")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicant(string name) =>
        await LiveTruckTracker.Instance.GetTruckByName(name);

    [HttpGet("applicant/{name}/{status}", Name = "Get Location by Applicant Name and Status from cache")]
    public async Task<IEnumerable<FoodTruck>> GetByApplicantAndStatus(string name, string status) =>
        await LiveTruckTracker.Instance.GetTruckByName(name, status);

    [HttpGet("street/{street}", Name = "Get Location by Street Name from cache")]
    public async Task<IEnumerable<FoodTruck>> GetByStreet(string street) =>
        await LiveTruckTracker.Instance.GetTruckByStreet(street);
}