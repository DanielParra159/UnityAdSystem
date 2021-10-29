namespace Domain
{
    public class InitAdServiceUseCase
    {
        private readonly AdService _adService;
        private readonly AdConfigurationProvider _adConfigurationProvider;

        public InitAdServiceUseCase(AdService adService,
            AdConfigurationProvider adConfigurationProvider)
        {
            _adService = adService;
            _adConfigurationProvider = adConfigurationProvider;
        }

        public async void Init()
        {
            var configuration = await _adConfigurationProvider.GetConfiguration();
            _adService.Init(configuration);
        }
    }
}