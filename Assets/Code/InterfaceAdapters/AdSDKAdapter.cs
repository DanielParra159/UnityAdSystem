using System;

namespace InterfaceAdapters
{
    public interface AdSDKAdapter
    {
        void ShowRewardedAd(Action<RewardedAdStatus> callback);
    }
}