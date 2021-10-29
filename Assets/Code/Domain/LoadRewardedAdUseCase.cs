namespace Domain
{
    public class LoadRewardedAdUseCase
    {
        private readonly AdService _adService;

        public LoadRewardedAdUseCase(AdService adService)
        {
            _adService = adService;
        }

        public void Load()
        {
            _adService.LoadRewardedAd();
        }

    }
}