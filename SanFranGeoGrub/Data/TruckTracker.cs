using System.Collections;
using Microsoft.OData.Client;
using Socrata;

namespace SanFranGeoGrub.Data;

public class TruckTracker
{
    public static TruckTracker Instance = new TruckTracker();

    public async Task<IEnumerable<FoodTruck>> GetData()
    {
        var serviceRoot = "https://data.sfgov.org/api/odata/v4/rqzj-sfat";
        var context = new Socrata.Service(new Uri(serviceRoot));

        var trucks = await context.FoodTruck.ExecuteAsync();

        foreach (var truck in trucks) { 
            Console.WriteLine(truck);
        }

        return trucks;
    }
}
