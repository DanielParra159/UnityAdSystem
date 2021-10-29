using System;
using InterfaceAdapters;

namespace Frameworks.Services
{
    public class DefaultAdStrategy : AdSDKAdapter
    {
        private readonly RewardedAd _rewardedAd;
        private Action<RewardedAdStatus> _callback;

        public DefaultAdStrategy(RewardedAd rewardedAd)
        {
            _rewardedAd = rewardedAd;
        }

        public void ShowRewardedAd(Action<RewardedAdStatus> callback)
        {
            _callback = callback;
            _rewardedAd.OnOkButtonPressed += HandleOk;
            _rewardedAd.OnCancelButtonPressed += HandleCancel;
            _rewardedAd.OnErrorButtonPressed += HandleError;

            _rewardedAd.Show();
        }

        private void HandleOk()
        {
            ReturnWith(RewardedAdStatus.Ok);
            Reset();
        }

        private void HandleCancel()
        {
            ReturnWith(RewardedAdStatus.Cancel);
            Reset();
        }

        private void HandleError()
        {
            ReturnWith(RewardedAdStatus.Error);
            Reset();
        }

        private void ReturnWith(RewardedAdStatus result)
        {
            _callback(result);
        }

        private void Reset()
        {
            _rewardedAd.Hide();
            _rewardedAd.OnOkButtonPressed -= HandleOk;
            _rewardedAd.OnCancelButtonPressed -= HandleCancel;
            _rewardedAd.OnErrorButtonPressed -= HandleError;
        }
    }
}