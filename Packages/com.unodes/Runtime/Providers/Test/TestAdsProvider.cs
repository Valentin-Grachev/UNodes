using System;
using UnityEngine;

namespace UNodes.Providers.Test
{
    public class TestAdsProvider : Ads.Provider
    {
        [SerializeField] private bool _isSupportRewarded = true;
        [SerializeField] private bool _isSupportInterstitial = true;
        [SerializeField] private bool _isAdBlock = false;
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private bool _simulateAdError = false;

        public override bool IsSupportRewarded => _isSupportRewarded;
        public override bool IsSupportInterstitial => _isSupportInterstitial;
        public override bool IsAdBlock => _isAdBlock;

        public override void Initialize(Action onInitialized) 
            => onInitialized?.Invoke();

        public override void ShowInterstitial(Action<bool> onHandled = null)
        {
            bool success = !_simulateAdError;

            if (_showLogs)
            {
                string result = success ? "SUCCESS" : "ERROR";
                Debug.Log($"[Ads] Show Interstitial: {result}");
            }

            onHandled?.Invoke(success);
        }

        public override void ShowRewarded(Action<bool> onHandled = null)
        {
            bool success = !_simulateAdError;

            if (_showLogs)
            {
                string result = success ? "SUCCESS" : "ERROR";
                Debug.Log($"[Ads] Show Rewarded: {result}");
            }

            onHandled?.Invoke(success);
        }
    }
}

