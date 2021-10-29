using System;
using InterfaceAdapters;
using UnityEngine.Advertisements;

namespace Frameworks.Services
{
    public class UnityAdStrategy : AdSDKAdapter, IUnityAdsInitializationListener,
        IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private AdConf _configuration;
        private Action<RewardedAdStatus> _callback;

        public void ShowRewardedAd(Action<RewardedAdStatus> callback)
        {
            _callback = callback;
            Advertisement.Show(_configuration.AdId, this);
        }

        public void LoadRewardedAd()
        {
            Advertisement.Load(_configuration.AdId, this);
        }

        public void Init(AdConf configuration)
        {
            _configuration = configuration;
            Advertisement.Initialize(configuration.GameId,
                configuration.TestMode,
                true,
                this);
        }

        public void OnInitializationComplete()
        {
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            _callback.Invoke(RewardedAdStatus.Error);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.COMPLETED:
                    _callback.Invoke(RewardedAdStatus.Ok);
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                case UnityAdsShowCompletionState.SKIPPED:
                    _callback.Invoke(RewardedAdStatus.Cancel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(showCompletionState), showCompletionState, null);
            }
        }
    }
}