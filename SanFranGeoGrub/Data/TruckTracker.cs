using Socrata;

namespace SanFranGeoGrub.Data;

public class TruckTracker
{
    private readonly ITruckDataSource _truckSource;
    private readonly IFileAccess _fileAccess;
    public static TruckTracker Instance => _instance ??= new TruckTracker(new TruckDataSource(), new FileAccess());

    private static TruckTracker? _instance;
    private IEnumerable<FoodTruck>? _trucks;

    public TruckTracker(ITruckDataSource truckSource, IFileAccess fileAccess)
    {
        _truckSource = truckSource;
        _fileAccess = fileAccess;
    }

    public async Task<IEnumerable<FoodTruck>> GetAllTrucks()
    {
        if (_trucks is null)
        {
            _trucks = await _fileAccess.ReadFromCache();
        }

        return _trucks ??= await FetchOdataAsync();
    }

    internal async Task<IEnumerable<FoodTruck>> FetchOdataAsync()
    {
        var trucks = await _truckSource.GetFoodTrucks();

        if (trucks.Any())
        {
            await _fileAccess.SaveToCache(trucks);
        }

        return trucks;
    }
}
