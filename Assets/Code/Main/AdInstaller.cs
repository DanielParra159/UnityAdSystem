using System.Linq;
using System.Threading.Tasks;
using Frameworks.Services;
using Submodules.accountmodule.Code.UnityConfigurationAdapters.Installers;
using Submodules.BaseModule.Code.UnityConfigurationAdapter.Installers;
using Submodules.UnityAdSystem.Assets.Code.Basic_Implementation;
using Submodules.UnityAdSystem.Assets.Code.Basic_Implementation.Installers;
using Submodules.UnityAdSystem.Assets.Code.Domain;
using Submodules.UnityAdSystem.Assets.Code.Frameworks.Services;
using Submodules.UnityAdSystem.Assets.Code.InterfaceAdapters;
using Submodules.UnityAdSystem.Assets.Code.InterfaceAdapters.Gateways;
using UnityEngine;

namespace Submodules.UnityAdSystem.Assets.Code.Main
{
    public class AdInstaller : IInstaller
    {
        private readonly string _appID;
        private readonly string _identifierAd;
        private ShowRewardedAdUseCase _showRewardedAdUseCase;
        private LoadRewardedAdUseCase _loadRewardedAdUseCase;
        private ReportAdActivityUseCase _reportAdActivityUseCase;
        private RewardAdActivityUseCase _rewardAdActivityUseCase;
        private DeliverRewardedAdUseCase _deliverRewardedAdUseCase;
        private AccountInstaller _accountInstaller;

        private PlayfabRewardAdsService adPlacementService;
        private IAdSDKAdapter _adStrategy;
        public GoogleAdmob GoogleAdmob;

        public IAdSDKAdapter ADStrategy
        {
            get => _adStrategy;
            set => _adStrategy = value;
        }

        public AdInstaller(string appID, string identifierAd)
        {
            _appID = appID;
            _identifierAd = identifierAd;
        }

        public async Task InitInstaller()
        {
            adPlacementService = new PlayfabRewardAdsService(_appID, _identifierAd);
            var initAdPlacementsUseCase = new InitAdPlacementsUseCase(adPlacementService);
            var placementsAds = await initAdPlacementsUseCase.GetAdPlacements();
            var adPlacementDetails = placementsAds.FirstOrDefault();
            var placementID = adPlacementDetails?.PlacementId;
            var rewardID = adPlacementDetails?.RewardId;

            GoogleAdmob = new GoogleAdmob(PlayfabAdConfiguration.APP_ID_AD,
                 PlayfabAdConfiguration.NAME_ONE_VIDEO_THREE_HINTS_UNIT_ID, PlayfabAdConfiguration.ONE_VIDEO_THREE_HINTS_UNIT_ID);

            _adStrategy = GetAdStrategy();

            var adServiceImpl = new AdServiceImpl(_adStrategy);
            adServiceImpl.SetStatusRewardedAdCallback();

            var reportGateway = new ReportGateway(placementID, rewardID);
            var playfabRewardActivityAdService =
                new PlayfabRewardActivityAdService(placementID, rewardID);

            var adConfigurationProviderImpl = new AdConfigurationProviderImpl();
            adConfigurationProviderImpl.SetAdId(rewardID);
            var initAdServiceUseCase = new InitAdServiceUseCase(adServiceImpl, adConfigurationProviderImpl);

            _reportAdActivityUseCase = new ReportAdActivityUseCase(reportGateway, adServiceImpl);
            _rewardAdActivityUseCase = new RewardAdActivityUseCase(playfabRewardActivityAdService);

            _loadRewardedAdUseCase = new LoadRewardedAdUseCase(adServiceImpl);
            _showRewardedAdUseCase = new ShowRewardedAdUseCase(adServiceImpl);
            _deliverRewardedAdUseCase = new DeliverRewardedAdUseCase(adServiceImpl, playfabRewardActivityAdService);

            _deliverRewardedAdUseCase.DeliverReward();
            _reportAdActivityUseCase.ReportingAd();
            //initAdServiceUseCase.Init();
        }

        private IAdSDKAdapter GetAdStrategy()
        {
            return new GoogleAdStrategy();

            #if USE_UNITY_SDK
                return new UnityAdStrategy();
            #endif

        }
    }
}