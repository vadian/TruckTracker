using Socrata;

namespace TruckTracker.Data;

public interface ILiveTruckTracker
{
    public Task<IEnumerable<FoodTruck>> GetTruckByName(string name, string? status = null);
    public Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street);
}

public class LiveTruckTracker : ILiveTruckTracker
{
    protected readonly ITruckDataSource _truckSource;

    public LiveTruckTracker(ITruckDataSource truckSource)
    {
        _truckSource = truckSource;
    }


    public virtual async Task<IEnumerable<FoodTruck>> GetTruckByName(string name, string? status = null) =>
        await _truckSource.GetTrucksByName(name, status);

    public virtual async Task<IEnumerable<FoodTruck>> GetTruckByStreet(string street) =>
        await _truckSource.GetTrucksByStreet(street);
}