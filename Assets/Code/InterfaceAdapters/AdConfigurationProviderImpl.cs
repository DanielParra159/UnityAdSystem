using System.Threading.Tasks;
using Domain;

namespace InterfaceAdapters
{
    public class AdConfigurationProviderImpl : AdConfigurationProvider
    {
        public async Task<AdConfiguration> GetConfiguration()
        {
            await Task.Delay(1000);
            return new AdConfiguration("asasd");
        }
    }
}