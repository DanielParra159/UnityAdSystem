using System;
using Domain;

namespace InterfaceAdapters
{
    public interface AdSDKAdapter
    {
        void ShowRewardedAd(Action<RewardedAdStatus> callback);
        void LoadRewardedAd();
        void Init(AdConf configuration);
    }
}