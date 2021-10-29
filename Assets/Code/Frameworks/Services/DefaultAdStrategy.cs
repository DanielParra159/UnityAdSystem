using System;
using InterfaceAdapters;

namespace Frameworks.Services
{
    public class DefaultAdStrategy : AdSDKAdapter
    {
        private RewardedAd _rewardedAd;
        private readonly RewardedAddLoader _rewardedAddLoader;
        private Action<RewardedAdStatus> _callback;
        
        private AdConf _conf;

        public DefaultAdStrategy(RewardedAddLoader rewardedAddLoader)
        {
            _rewardedAddLoader = rewardedAddLoader;
        }


        public void ShowRewardedAd(Action<RewardedAdStatus> callback)
        {
            if (_rewardedAd == null)
                throw new NullReferenceException("Rewarded ad not loaded");
            
            _callback = callback;
            _rewardedAd.OnOkButtonPressed += HandleOk;
            _rewardedAd.OnCancelButtonPressed += HandleCancel;
            _rewardedAd.OnErrorButtonPressed += HandleError;

            _rewardedAd.Show();
        }

        public async void LoadRewardedAd()
        {
            _rewardedAd = await _rewardedAddLoader.Load();
        }

        public void Init(AdConf conf)
        {
            _conf = conf;
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