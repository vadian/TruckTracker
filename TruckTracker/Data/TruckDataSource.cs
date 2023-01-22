using Socrata;

namespace TruckTracker.Data
{
    public interface ITruckDataSource
    {
        Task<List<FoodTruck>> GetAllFoodTrucks();
        Task<List<FoodTruck>> GetTrucksByName(string name, string? status = null);
        Task<List<FoodTruck>> GetTrucksByStreet(string street);
    }
    public class TruckDataSource : ITruckDataSource
    {

        static readonly string SERVICE_ROOT = "https://data.sfgov.org/api/odata/v4/";

        public async Task<List<FoodTruck>> GetAllFoodTrucks() =>
            (await _getContext().FoodTruck.ExecuteAsync()).ToList();

        public Task<List<FoodTruck>> GetTrucksByName(string name, string? status = null)
        {
            var context = _getContext();
            var trucks = context.FoodTruck
                .Where(x => x.Applicant.ToLower().Contains(name.ToLower()));

            if (status is not null)
            {
                trucks = trucks.Where(x => x.Status.ToLower().Equals(status.ToLower()));
            }

            return Task.FromResult(trucks.ToList());
        }

        public Task<List<FoodTruck>> GetTrucksByStreet(string street) =>
            Task.FromResult(_getContext().FoodTruck
                .Where(x => x.Locationdescription.ToLower().Contains(street.ToLower()))
                .ToList());

        private static Service _getContext() => new (new (SERVICE_ROOT));
    }
}
