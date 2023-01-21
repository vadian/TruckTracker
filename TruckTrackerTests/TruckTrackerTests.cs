using SanFranGeoGrub.Data;

namespace TruckTrackerTests
{
    public class TruckTrackerTests
    {
        public TruckTracker CUT = new TruckTracker();

        [Fact]
        public async Task TestGetData()
        {
            var trucks = await CUT.GetData();
            Assert.NotNull(trucks);
            Assert.NotEmpty(trucks);
        }
    }
}