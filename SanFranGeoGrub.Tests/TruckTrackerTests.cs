using Socrata;

namespace SanFranGeoGrub.Tests;

public class CachedTruckTrackerTests
{
    private Mock<ITruckDataSource> truckDataSource = new();

    [Fact]
    public async Task it_should_fetch_data_from_datasource()
    {
        var mockTrucks = new List<FoodTruck>()
        {
            new FoodTruck() { __id = "one" },
            new FoodTruck() { __id = "two" },
        };

        truckDataSource.Setup(x => x.GetAllFoodTrucks()).ReturnsAsync(mockTrucks);
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetAllTrucks();

        Assert.Equal(mockTrucks.Count, trucks.Count());
    }

    [Fact]
    public async Task it_should_filter_by_name()
    {
        var mockTrucks = new List<FoodTruck>()
        {
            new () { Applicant = "Applicant One" },
            new () { Applicant = "Applicant Two" },
        };

        truckDataSource.Setup(x => x.GetAllFoodTrucks()).ReturnsAsync(mockTrucks);
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetTruckByName("ONE");

        Assert.Single(trucks);
        Assert.Equal("Applicant One", trucks.Single().Applicant);
    }

    [Fact]
    public async Task it_should_filter_by_name_and_status()
    {
        var mockTrucks = new List<FoodTruck>()
        {
            new () { Applicant = "Valid One", Status = "INACTIVE" },
            new () { Applicant = "Valid Two", Status = "ACTIVE"   },
            new () { Applicant = "unknown",   Status = "INACTIVE" }
        };

        truckDataSource.Setup(x => x.GetAllFoodTrucks()).ReturnsAsync(mockTrucks);
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetTruckByName("valid", "inactive");

        Assert.Single(trucks);
        Assert.Equal("Valid One", trucks.Single().Applicant);
    }
}
