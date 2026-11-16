using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UNodes.YandexGames
{
    public class YandexGamesSdk : MonoBehaviour
    {
        public static event Action OnGameApiPauseChanged;

        private static Action _onInterstitialOpened;
        private static Action<bool> _onInterstitialClosed;

        private static Action _onRewardedOpened;
        private static Action _onRewardedRewarded;
        private static Action<bool> _onRewardedClosed;

        private static Action<string> _onSaveDataReceived;
        private static Action _onSetSavesHandled;

        private static Action<bool> _onCanReviewHanled;
        private static Action<bool> _onReviewHandled;

        public static bool IsPaused
        {
            get
            {
#if UNITY_WEBGL
                return _ApiIsPaused() == 1;
#else
                return false;
#endif
            }
        }


        public static bool IsInitialized
        {
            get
            {
#if UNITY_WEBGL
                return _CheckSdkInit() == 1;
#else
                return false;
#endif
            }
        }

        public static string GetLanguageCode()
        {
#if UNITY_WEBGL
            return _GetLanguageCode();
#else
            return null;
#endif
        }

        public static DateTime GetServerTime()
        {
#if UNITY_WEBGL
            return DateTime.UnixEpoch.AddMilliseconds((long)_GetServerTimeMillis());
#else
            return DateTime.UtcNow;
#endif
        }

        public static string GetDeviceType()
        {
#if UNITY_WEBGL
            return _GetDeviceType();
#else
            return null;
#endif
        }

        public static void GameReady()
        {
#if UNITY_WEBGL
            _GameReady();
#endif
        }

        public static void ShowInterstitial(Action onOpened = null, Action<bool> onClosed = null)
        {
            _onInterstitialOpened = onOpened;
            _onInterstitialClosed = onClosed;
#if UNITY_WEBGL
            _ShowInterstitial();
#endif
        }

        private void OnInterstitialOpened() => _onInterstitialOpened?.Invoke();
        private void OnInterstitialClosed(string wasShown) 
            => _onInterstitialClosed?.Invoke(wasShown == "1");

        public static void ShowRewarded(Action onOpened = null, Action onRewarded = null, Action<bool> onClosed = null)
        {
            _onRewardedOpened = onOpened;
            _onRewardedRewarded = onRewarded;
            _onRewardedClosed = onClosed;

#if UNITY_WEBGL
            _ShowRewarded();
#endif
        }

        private void OnRewardedOpened() => _onRewardedOpened?.Invoke();
        private void OnRewardedRewarded() => _onRewardedRewarded?.Invoke();
        private void OnRewardedClosed(string wasShown) => 
            _onRewardedClosed?.Invoke(wasShown == "1");


        public static void ShowBanner()
        {
#if UNITY_WEBGL
            _ShowBanner();
#endif
        }

        public static void HideBanner()
        {
#if UNITY_WEBGL
            _HideBanner();
#endif
        }

        public static void RequestSaves(string saveKey, Action<string> onDataReceived)
        {
            _onSaveDataReceived = onDataReceived;
#if UNITY_WEBGL
            _RequestSaves(saveKey);
#endif
        }

        private void OnSaveDataReceived(string data) => _onSaveDataReceived?.Invoke(data);

        
        public static void SetSaves(string saveKey, string saveData, Action onHandled)
        {
            _onSetSavesHandled = onHandled;
#if UNITY_WEBGL
            _SetSaves(saveKey, saveData);
#endif
        }

        private void OnSetSavesSuccess() => _onSetSavesHandled?.Invoke();


        public static void RequestCanReview(Action<bool> onHandled)
        {
            _onCanReviewHanled = onHandled;
#if UNITY_WEBGL
            _RequestCanReview();
#endif
        }

        private void OnCanReviewHandled(string canReview) 
            => _onCanReviewHanled?.Invoke(canReview == "1");



        public static void RequestReview(Action<bool> onHandled)
        {
            _onReviewHandled = onHandled;
#if UNITY_WEBGL
            _RequestReview();
#endif
        }

        private void OnReviewHandled(string gameWasRated)
            => _onReviewHandled?.Invoke(gameWasRated == "1");


        private void OnGameApiPauseChange() => OnGameApiPauseChanged?.Invoke();


        public static void GameplayStart()
        {
#if UNITY_WEBGL
            _GameplayStart();
#endif
        }

        public static void GameplayStop()
        {
#if UNITY_WEBGL
            _GameplayStop();
#endif
        }



#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern int _CheckSdkInit();
        [DllImport("__Internal")] private static extern string _GetLanguageCode();
        [DllImport("__Internal")] private static extern double _GetServerTimeMillis();
        [DllImport("__Internal")] private static extern string _GetDeviceType();
        [DllImport("__Internal")] private static extern void _GameReady();
        [DllImport("__Internal")] private static extern void _ShowInterstitial();
        [DllImport("__Internal")] private static extern void _ShowRewarded();
        [DllImport("__Internal")] private static extern void _ShowBanner();
        [DllImport("__Internal")] private static extern void _HideBanner();
        [DllImport("__Internal")] private static extern void _RequestSaves(string saveKey);
        [DllImport("__Internal")] private static extern void _SetSaves(string saveKey, string saveData);
        [DllImport("__Internal")] private static extern void _RequestCanReview();
        [DllImport("__Internal")] private static extern void _RequestReview();
        [DllImport("__Internal")] private static extern int _ApiIsPaused();
        [DllImport("__Internal")] private static extern void _GameplayStart();
        [DllImport("__Internal")] private static extern void _GameplayStop();


#endif






    }
}
