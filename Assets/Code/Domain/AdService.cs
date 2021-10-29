using System.Threading.Tasks;

namespace Domain
{
    public interface AdService
    {
        Task<RewardedAdStatus> ShowRewardedAd();
        void LoadRewardedAd();
        void Init(AdConfiguration adConfiguration);
    }
}