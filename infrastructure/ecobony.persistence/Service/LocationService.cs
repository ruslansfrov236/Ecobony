


namespace ecobony.persistence.Service
{
    public class LocationService(ILocationReadRepository locationRead,
                                  ILocationWriteRepository locationWrite,
                                  IConfiguration configuration) : ILocationService
    {
        public async Task<bool> Create(CreateLocationDto_s model)
        {

            var client = new RestClient("https://us1.locationiq.com/v1/search.php");
            var request = new RestRequest()
                .AddHeader("accept", "application/json")
                .AddQueryParameter("key", configuration["LocationIQ:Key"])
                .AddQueryParameter("q", model.Address)
                .AddQueryParameter("format", "json");

            var response = await client.ExecuteGetAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return false;

            var json = JsonDocument.Parse(response.Content);
            var firstResult = json.RootElement[0];

            var lat = firstResult.GetProperty("lat").GetString();
            var lon = firstResult.GetProperty("lon").GetString();
            var displayName = firstResult.GetProperty("display_name").GetString();

            string country = "";
            string city = "";

            if (firstResult.TryGetProperty("address", out var address))
            {
                if (address.TryGetProperty("country", out var c))
                    country = c.GetString();
                if (address.TryGetProperty("city", out var ct))
                    city = ct.GetString();
                else if (address.TryGetProperty("town", out var town))
                    city = town.GetString();
                else if (address.TryGetProperty("village", out var village))
                    city = village.GetString();
            }



            var location = new Location
            {
                Id = Guid.NewGuid(),
                Country = country,
                City = city,
                Address = displayName,
                Latitude = double.Parse(lat, CultureInfo.InvariantCulture),
                Longitude = double.Parse(lon, CultureInfo.InvariantCulture),
                isDeleted = false
            };


            await locationWrite.AddAsync(location);
            await locationWrite.SaveChangegesAsync();
            return true;
        }


        public async Task<bool> Delete(string id)
        {
            if (!Guid.TryParse(id, out var _))
                throw new ArgumentException("Invalid ID format", nameof(id));

            var location = await locationRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == true);

            locationWrite.Delete(location);
            await locationWrite.SaveChangegesAsync();
            return true;


        }

        public async Task<List<Location>> GetAdminAll()
        => await locationRead.GetAll().ToListAsync();

        public async Task<Location> GetById(string id)
        {
            if (!Guid.TryParse(id, out var _))
                throw new ArgumentException("Invalid ID format", nameof(id));

            var location = await locationRead.GetByIdAsync(id) ?? throw new KeyNotFoundException("Location not found");

            return location;
        }

        public async Task<List<Location>> GetClientAll()
        => await locationRead.GetFilter(a => a.isDeleted == false).ToListAsync();

        public async Task<bool> Restore(string id)
        {
            if (!Guid.TryParse(id, out var _))
                throw new ArgumentException("Invalid ID format", nameof(id));

            var location = await locationRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == true);

            location.isDeleted = false;
            locationWrite.Update(location);
            await locationWrite.SaveChangegesAsync();
            return true;
        }

        public async Task<bool> SoftDelete(string id)
        {
            if (!Guid.TryParse(id, out var _))
                throw new ArgumentException("Invalid ID format", nameof(id));

            var location = await locationRead.GetSingleAsync(a => a.Id == Guid.Parse(id) && a.isDeleted == false);

            location.isDeleted = true;
            locationWrite.Update(location);
            await locationWrite.SaveChangegesAsync();
            return true;
        }

        public async Task<bool> Update(UpdateLocationDto_s model)
        {
            if (!Guid.TryParse(model.Id, out var _))
                throw new ArgumentException("Invalid ID format", nameof(model.Id));

            var location = await locationRead.GetSingleAsync(a => a.Id == Guid.Parse(model.Id) && a.isDeleted == true);


            var client = new RestClient("https://us1.locationiq.com/v1/search.php");
            var request = new RestRequest()
                .AddHeader("accept", "application/json")
                .AddQueryParameter("key", configuration["LocationIQ:Key"])
                .AddQueryParameter("q", model.Address)
                .AddQueryParameter("format", "json");

            var response = await client.ExecuteGetAsync(request);

            if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
                return false;

            var json = JsonDocument.Parse(response.Content);
            var firstResult = json.RootElement[0];

            var lat = firstResult.GetProperty("lat").GetString();
            var lon = firstResult.GetProperty("lon").GetString();
            var displayName = firstResult.GetProperty("display_name").GetString();

            string country = "";
            string city = "";

            if (firstResult.TryGetProperty("address", out var address))
            {
                if (address.TryGetProperty("country", out var c))
                    country = c.GetString();
                if (address.TryGetProperty("city", out var ct))
                    city = ct.GetString();
                else if (address.TryGetProperty("town", out var town))
                    city = town.GetString();
                else if (address.TryGetProperty("village", out var village))
                    city = village.GetString();
            }

            location.Country = country;
            location.City = city;
            location.Address = displayName;
            location.Latitude = double.Parse(lat, CultureInfo.InvariantCulture);
            location.Longitude = double.Parse(lon, CultureInfo.InvariantCulture);



            locationWrite.Update(location);
            await locationWrite.SaveChangegesAsync();
            return true;
        }
    }
}