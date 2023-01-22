using System.Drawing;
using System.Xml.Linq;
using Socrata;

namespace SanFranGeoGrub.Data
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

        public async Task<List<FoodTruck>> GetAllFoodTrucks()
        {
            var context = _getContext();

            var trucks = await context.FoodTruck.ExecuteAsync();
            return trucks.ToList();

        }

        private static Service _getContext()
        {
            var context = new Socrata.Service(new Uri(SERVICE_ROOT));
            return context;
        }

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

        public Task<List<FoodTruck>> GetTrucksByStreet(string street)
        {
            var context = _getContext();
            var trucks = context.FoodTruck
                .Where(x => x.Location_address.ToLower().Contains(street.ToLower()));

            return Task.FromResult(trucks.ToList());
        }

    }
}
