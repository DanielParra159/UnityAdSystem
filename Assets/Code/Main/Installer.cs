using System;
using Domain;
using Frameworks.Services;
using Frameworks.View;
using InterfaceAdapters;
using UnityEngine;

namespace Main
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private RewardedAddLoaderImpl _rewardedAddLoaderImpl;
        private ShowRewardedAdUseCase _showRewardedAdUseCase;
        private LoadRewardedAdUseCase _loadRewardedAdUseCase;

        private void Awake()
        {
            var defaultAdStrategy = new DefaultAdStrategy(_rewardedAddLoaderImpl);
            var unityAdStrategy = new UnityAdStrategy();

            var adServiceImpl = new AdServiceImpl(unityAdStrategy);

            var adConfigurationProviderImpl = new AdConfigurationProviderImpl();
            var initAdServiceUseCase = new InitAdServiceUseCase(adServiceImpl, adConfigurationProviderImpl);
            initAdServiceUseCase.Init();
            _loadRewardedAdUseCase = new LoadRewardedAdUseCase(adServiceImpl);
            _showRewardedAdUseCase = new ShowRewardedAdUseCase(adServiceImpl);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _loadRewardedAdUseCase.Load();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _showRewardedAdUseCase.Show();
            }
        }
    }
}