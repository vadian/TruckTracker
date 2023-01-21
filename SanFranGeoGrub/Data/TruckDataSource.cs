using Socrata;

namespace SanFranGeoGrub.Data
{
    public interface ITruckDataSource
    {
        Task<List<FoodTruck>> GetFoodTrucks();
    }
    public class TruckDataSource : ITruckDataSource
    {
        public async Task<List<FoodTruck>> GetFoodTrucks()
        {
            var serviceRoot = "https://data.sfgov.org/api/odata/v4/";
            var context = new Socrata.Service(new Uri(serviceRoot));

            var trucks = await context.FoodTruck.ExecuteAsync();
            return trucks.ToList();

        }
    }
}
