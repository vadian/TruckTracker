
namespace SanFranGeoGrub.Tests;

public class TruckTrackerTests
{
    private Mock<IFileAccess> fileAccess = new ();
    private Mock<TruckDataSource> truckDataSource = new();

    [Fact]
    public async Task TestGetData()
    {
        TruckTracker CUT = new TruckTracker(truckDataSource.Object, fileAccess.Object);

        var trucks = await CUT.FetchOdataAsync();

        Assert.NotNull(trucks);
        Assert.NotEmpty(trucks);
    }
}
