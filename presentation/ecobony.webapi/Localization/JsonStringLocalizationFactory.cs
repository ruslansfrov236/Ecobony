using Microsoft.Extensions.Caching.Distributed;

namespace ecobony.webapi.Localization
{
    public class JsonStringLocalizationFactory(IDistributedCache cache) : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalization(cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalization(cache);
        }
    }
}
