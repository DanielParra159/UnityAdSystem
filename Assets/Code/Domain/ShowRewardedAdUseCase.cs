using UnityEngine;

namespace Domain
{
    public class ShowRewardedAdUseCase
    {
        private readonly AdService _adService;

        public ShowRewardedAdUseCase(AdService adService)
        {
            _adService = adService;
        }

        public async void Show()
        {
            var result = await _adService.ShowRewardedAd();
        }
    }
}