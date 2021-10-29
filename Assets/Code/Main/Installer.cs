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
        [SerializeField] private RewardedAdView _rewardedAdView;
        private ShowRewardedAdUseCase _showRewardedAdUseCase;

        private void Awake()
        {
            var defaultAdStrategy = new DefaultAdStrategy(_rewardedAdView);

            var adServiceImpl = new AdServiceImpl(defaultAdStrategy);

             _showRewardedAdUseCase = new ShowRewardedAdUseCase(adServiceImpl);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _showRewardedAdUseCase.Show(); 
            }
        }
    }
}