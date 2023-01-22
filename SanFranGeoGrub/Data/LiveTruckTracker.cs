using Socrata;

namespace SanFranGeoGrub.Data;

public class LiveTruckTracker
{
    protected readonly ITruckDataSource _truckSource;
    private static LiveTruckTracker? _instance;
    public static LiveTruckTracker Instance => _instance ??= new LiveTruckTracker(new TruckDataSource());

    public LiveTruckTracker(ITruckDataSource truckSource)
    {
        _truckSource = truckSource;
    }


    public virtual async Task<IEnumerable<FoodTruck>> GetTruckByName(string name, string? status = null)
    {
        return await _truckSource.GetTrucksByName(name, status);
    }

    public virtual async Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street)
    {
        return await _truckSource.GetTrucksByStreet(street);
    }

}

public class CachedTruckTracker : LiveTruckTracker
{
    private IEnumerable<FoodTruck>? _allTrucks;
    
    private static CachedTruckTracker? _instance;
    public new static CachedTruckTracker Instance = _instance ??= new CachedTruckTracker(new TruckDataSource());

    public async Task<IEnumerable<FoodTruck>> GetAllTrucks()
    {
        return _allTrucks ??= await _truckSource.GetAllFoodTrucks();
    }

    
    public CachedTruckTracker(ITruckDataSource dataSource) : base(dataSource) { }

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

    public override async Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street)
    {
        var trucks = await GetAllTrucks();
        trucks = trucks
            .Where(x => x.Location_address.ToLower().Contains(street.ToLower()));

        return trucks;
    }

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
