using Newtonsoft.Json;
using Socrata;

namespace SanFranGeoGrub.Data
{

    public interface IFileAccess
    {
        Task SaveToCache(IEnumerable<FoodTruck> trucks);
        Task<IEnumerable<FoodTruck>?> ReadFromCache();
    }

    public class FileAccess : IFileAccess
    {
        private const string CACHE_FILE = "trucks.json";
        private readonly string _cachePath;

        public FileAccess()
        {
            _cachePath = Path.Combine(Environment.CurrentDirectory, CACHE_FILE);
        }

        public async Task SaveToCache(IEnumerable<FoodTruck> trucks)
        {
            await File.WriteAllTextAsync(_cachePath, JsonConvert.SerializeObject(trucks));
        }

        public async Task<IEnumerable<FoodTruck>?> ReadFromCache()
        {
            IEnumerable<FoodTruck>? retVal = null;

            if (File.Exists(_cachePath))
            {
                try
                {
                    var text = await File.ReadAllTextAsync(_cachePath);
                    retVal = JsonConvert.DeserializeObject<IEnumerable<FoodTruck>>(text);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return retVal ?? new List<FoodTruck>();
        }
    }
}
