using Socrata;

namespace TruckTracker.Data;

public interface ICachedTruckTracker
{
    Task<IEnumerable<FoodTruck>> GetAllTrucks();
    Task<IEnumerable<FoodTruck>> GetTruckByName(string name, string? status = null);
    Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street);
    Task<IEnumerable<FoodTruck>> GetTrucksNear(decimal refLatitude, decimal refLongitude, bool includeAll = false);
}

public class CachedTruckTracker : LiveTruckTracker, ICachedTruckTracker
{
    private List<FoodTruck> _allTrucks = new ();
    private DateTime _lastTruckUpdate = DateTime.MinValue;
    private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(5);
    
    public CachedTruckTracker(ITruckDataSource dataSource) : base(dataSource) { }

    public async Task<IEnumerable<FoodTruck>> GetAllTrucks()
    {
        if (DateTime.Now - _lastTruckUpdate > _cacheExpirationTime)
        {
            _lastTruckUpdate = DateTime.Now;
            _allTrucks = await _truckSource.GetAllFoodTrucks();
        }
        return _allTrucks;
    }

    public override async Task<IEnumerable<FoodTruck>> GetTruckByName(string name, string? status = null)
    {
        var trucks = await GetAllTrucks();
        trucks = trucks.Where(x => x.Applicant.ToLower().Contains(name.ToLower()));

        if (status is not null)
        {
            trucks = trucks.Where(x => x.Status.ToLower().Equals(status.ToLower()));
        }

        return trucks;
    }

    public override async Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street) => 
        (await GetAllTrucks())
        .Where(x => x.Address.ToLower().Contains(street.ToLower()) ||
                    x.Locationdescription.ToLower().Contains(street.ToLower()));
    

    public virtual async Task<IEnumerable<FoodTruck>> GetTrucksNear(decimal refLatitude, decimal refLongitude, bool includeAll = false)
    {
        var trucks = await GetAllTrucks();

        double distance(decimal? lat, decimal? longitude)
        {
            if (lat is null or 0 || longitude is null or 0)
            {
                return double.MaxValue;
            }
            var latDiff = lat - refLatitude;
            var longDiff = longitude - refLongitude;

            return Math.Sqrt(Math.Pow((double)latDiff, 2) + Math.Pow((double)longDiff, 2));
        }

        if (!includeAll)
        {
            trucks = trucks.Where(x => x.Status == "APPROVED");
        }

        return trucks
            .OrderBy(x => distance(x.Latitude, x.Longitude))
            .Take(5);
    }
}