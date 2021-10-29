using System.Threading.Tasks;
using Frameworks.View;
using UnityEngine;

namespace Frameworks.Services
{
    public class RewardedAddLoaderImpl : MonoBehaviour, RewardedAddLoader
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private RewardedAdView _rewardedAdViewPrefab;

        public async Task<RewardedAd> Load()
        {
            var rewardedAdView = Instantiate(_rewardedAdViewPrefab, _container);
            await Task.Delay(1000);
            return rewardedAdView;
        }
    }
}