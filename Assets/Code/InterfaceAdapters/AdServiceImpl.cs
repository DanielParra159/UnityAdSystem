using System;
using System.Threading.Tasks;
using Domain;

namespace InterfaceAdapters
{
    public class AdServiceImpl : AdService
    {
        private readonly AdSDKAdapter _mainProvider;

        public AdServiceImpl(AdSDKAdapter mainProvider)
        {
            _mainProvider = mainProvider;
        }

        public Task<Domain.RewardedAdStatus> ShowRewardedAd()
        {
            var taskCompletionSource = new TaskCompletionSource<Domain.RewardedAdStatus>();
            _mainProvider.ShowRewardedAd(status => OnShowRewardedAdEnded(status, taskCompletionSource));

            return Task.Run(() => taskCompletionSource.Task);
        }

        public void LoadRewardedAd()
        {
            _mainProvider.LoadRewardedAd();
        }

        public void Init(AdConfiguration configuration)
        {
            var adConf = new AdConf(configuration.AdId);
            _mainProvider.Init(adConf);
        }

        private void OnShowRewardedAdEnded(InterfaceAdapters.RewardedAdStatus status,
            TaskCompletionSource<Domain.RewardedAdStatus> taskCompletionSource)
        {
            var rewardedAdStatus = MapStatus(status);
            taskCompletionSource.SetResult(rewardedAdStatus);
        }

        private static Domain.RewardedAdStatus MapStatus(RewardedAdStatus status)
        {
            switch (status)
            {
                case RewardedAdStatus.Ok:
                    return Domain.RewardedAdStatus.Ok;
                case RewardedAdStatus.Cancel:
                case RewardedAdStatus.Error:
                    return Domain.RewardedAdStatus.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}