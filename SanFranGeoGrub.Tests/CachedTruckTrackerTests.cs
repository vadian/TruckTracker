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
    public async Task it_should_filter_by_street()
    {
        var mockTrucks = new List<FoodTruck>()
        {
            new () { Applicant = "Applicant One", Location_address = "Good" },
            new () { Applicant = "Applicant Two", Location_address = "Bad" },
        };

        truckDataSource.Setup(x => x.GetAllFoodTrucks()).ReturnsAsync(mockTrucks);
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetTruckByStreet("GOOD");

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

    [Fact]
    public async Task it_should_filter_by_location()
    {
        var mockTrucks = new List<FoodTruck>()
        {
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Correct", Latitude = -2, Longitude = -2},
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Correct", Latitude = 2, Longitude = 2 },
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Correct", Latitude = 3, Longitude = 3 },
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Correct", Latitude = -4, Longitude = 4 },
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Correct", Latitude = 1, Longitude = 1 },
            new () { Applicant = "Valid", Status = "APPROVED", __id = "Incorrect", Latitude = 5, Longitude = -5 },
        };

        truckDataSource.Setup(x => x.GetAllFoodTrucks()).ReturnsAsync(mockTrucks);
        CachedTruckTracker CUT = new CachedTruckTracker(truckDataSource.Object);

        var trucks = await CUT.GetTrucksNear(0, 0);

        Assert.Equal(5, trucks.Count());
        Assert.True(trucks.All((x) => "Correct".Equals(x.__id)));
    }
}
