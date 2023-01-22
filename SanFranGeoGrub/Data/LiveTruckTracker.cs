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