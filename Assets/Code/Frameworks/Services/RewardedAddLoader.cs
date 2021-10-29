using System.Threading.Tasks;

namespace Frameworks.Services
{
    public interface RewardedAddLoader
    {
        Task<RewardedAd> Load();
    }
}