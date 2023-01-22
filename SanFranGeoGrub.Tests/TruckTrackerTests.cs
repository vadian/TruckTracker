namespace SanFranGeoGrub.Tests;

public class CachedTruckTrackerTests
{
    private Mock<TruckDataSource> truckDataSource = new();

    [Fact]
    public async Task TestGetData()
    {
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetAllTrucks();

        Assert.NotNull(trucks);
        Assert.NotEmpty(trucks);
    }
}
