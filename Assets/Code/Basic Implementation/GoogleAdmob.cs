using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMobileAds.Api;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Submodules.UnityAdSystem.Assets.Code.Basic_Implementation
{
    public class GoogleAdmob
    {
        private RewardedAd _rewardedAd;
        private readonly string _placementID;
        private readonly string _rewardID; //used only for playfab ID
        private readonly string _adID; //used for google admob 

        public GoogleAdmob(string placementID, string rewardID, string adID)
        {
            _placementID = placementID;
            _rewardID = rewardID;
            _adID = adID;
            
            ConfigurationMobileAds();

        }

        private void ConfigurationMobileAds()
        {
            MobileAds.Initialize(initStatus => { });
            List<string> deviceIds = new List<string>();
            deviceIds.Add("AEB928235AFC892C31F711F5D9BF5A6B");
            RequestConfiguration requestConfiguration = new RequestConfiguration
                    .Builder()
                .SetTestDeviceIds(deviceIds)
                .build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        public RewardedAd RequestRewardedAd() 
        {
            _rewardedAd = new RewardedAd(_adID);
            ConfigureEvents();
            // Create an empty ad request.
            // Load the rewarded ad with the request.
            return _rewardedAd;
        }

        public void LoadRewardedAd()
        {
            AdRequest request = new AdRequest.Builder().Build();
            _rewardedAd.LoadAd(request);
        }

        private void ConfigureEvents()
        {
            // Called when an ad request has successfully loaded.
            _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            _rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            _rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        }

        private void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            PlayFabClientAPI.ReportAdActivity(
                new ReportAdActivityRequest
                    {Activity = AdActivity.Start, PlacementId = _placementID, RewardId = _rewardID},
                result => OnSuccess(result, taskCompletionSource), error => OnError(error, taskCompletionSource));
            MonoBehaviour.print("HandleRewardedAdLoaded event received");
            Task.Run(() => taskCompletionSource.Task);

            UserChoseToWatchAd();
        }

        private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardedAdFailedToLoad event received with message: "
                + args.LoadAdError.GetResponseInfo());
        }

        private void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            PlayFabClientAPI.ReportAdActivity(
                new ReportAdActivityRequest
                    {Activity = AdActivity.Opened, PlacementId = _placementID, RewardId = _rewardID},
                result => OnSuccess(result, taskCompletionSource), error => OnError(error, taskCompletionSource));
            MonoBehaviour.print("HandleRewardedAdOpening event received");
            Task.Run(() => taskCompletionSource.Task);

        }

        private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardedAdFailedToShow event received with message: "
                + args.Message);
        }

        private void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            PlayFabClientAPI.ReportAdActivity(
                new ReportAdActivityRequest
                    {Activity = AdActivity.Closed, PlacementId = _placementID, RewardId = _rewardID},
                result => OnSuccess(result, taskCompletionSource), error => OnError(error, taskCompletionSource));
            MonoBehaviour.print("HandleRewardedAdClosed event received");
            Task.Run(() => taskCompletionSource.Task);
            
        }

        private void HandleUserEarnedReward(object sender, Reward args)
        {

            var taskCompletionSource = new TaskCompletionSource<bool>();

            PlayFabClientAPI.ReportAdActivity(
                new ReportAdActivityRequest {Activity = AdActivity.End, PlacementId = _placementID, RewardId = _rewardID},
                result => OnSuccess(result, taskCompletionSource),
                error => OnError(error, taskCompletionSource));

            var rewardAdActivity = new RewardAdActivityRequest
            {
                PlacementId = _placementID,
                RewardId = _rewardID
            };
            
            PlayFabClientAPI.RewardAdActivity(rewardAdActivity, result =>
            {
                Debug.Log(result.RewardResults.GrantedItems);
            }, error => Debug.LogWarning("failed to reward playfab"));
            
            Task.Run(() => taskCompletionSource.Task);

        }

        private void OnError(PlayFabError error, TaskCompletionSource<bool> taskCompletionSource)
        {
            taskCompletionSource.SetResult(false);
            throw new Exception(error.GenerateErrorReport());
        }

        private void OnSuccess(ReportAdActivityResult result, TaskCompletionSource<bool> taskCompletionSource)
        {
            taskCompletionSource.SetResult(true);
            Debug.Log("result " + result.ToString());
        }

        private void UnsubscribeEvents()
        {
            _rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            _rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            _rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            _rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            _rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
            // Called when the ad is closed.
            _rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        }

        private void UserChoseToWatchAd()
        {
            if (!_rewardedAd.IsLoaded()) return;
            
            _rewardedAd.Show();
        }
    }
}