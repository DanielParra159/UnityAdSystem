using System.Threading.Tasks;

namespace Domain
{
    public interface AdConfigurationProvider
    {
        Task<AdConfiguration> GetConfiguration();
    }
}