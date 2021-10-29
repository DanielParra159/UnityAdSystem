
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
            if (result == RewardedAdStatus.Ok)
            {
                // TODO: Dar la recompensa use case
                return;
            }
            
            // TODO: Mostar mensaje de error use case
        }
    }
}