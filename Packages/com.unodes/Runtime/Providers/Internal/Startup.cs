using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UNodes.Data;
using UNodes.Extensions;
using UNodes.Providers;

namespace UNodes
{
    [DefaultExecutionOrder(-10000)]
    public class Startup : MonoBehaviour
    {
        public static event Action OnInitialized;
        public static bool IsInitialized { get; private set; } = false;
        public static Transform Container { get; private set; }

        private static bool _startupIsCreated = false;
        private static ProviderConfiguration _configuration;
        

        [SerializeField] private EnvKey _editorConfigurationKey;
        [SerializeField] private EnvKey _buildConfigurationKey;
        [Space]
        [SerializeField] private BaseBootstrap _controllerCreator;
        [SerializeField] private BasePaymentHandler _paymentHandler;
        [SerializeField] private BaseSaveEncoder _saveEncoder;
        [SerializeField] private RectTransform _defaultViewerContainer;
        [Space]
        [SerializeField] private List<ProviderConfiguration> _configurations;



        private void Awake()
        {
            if (_startupIsCreated)
            {
                Destroy(gameObject);
                return;
            }

            Container = transform;

            SetActiveRootGameObjects(false);

            DontDestroyOnLoad(gameObject);
            _startupIsCreated = true;

            Initialize();
        }


        public void Initialize()
        {
            new ScreenState();
            ViewerSettings.SetDefaultContainer(_defaultViewerContainer);

            var configurationKey = Env.IsEditor ? 
                _editorConfigurationKey : _buildConfigurationKey;

            _configuration = Resources.Load<ProviderConfiguration>("DefaultConfiguration");
            
            if (configurationKey != EnvKey.Default)
            {
                _configuration = _configurations.FindOrExeption
                    (configuration => configuration.EnvProvider.EnvKey == configurationKey);
            }

            Saves.SetSaveEncoder(_saveEncoder);
            Saves.SetLocalAutosaveTimeInterval(_configuration.LocalAutosaveIntervalSec);
            Saves.SetCloudAutosaveTimeInterval(_configuration.CloudAutosaveIntervalSec);


            int initProcessCount = 7;

            Env.InitializeProvider(_configuration.EnvProvider, OnProviderInitialized);
            Ads.InitializeProvider(_configuration.AdsProvider, OnProviderInitialized);
            Socials.InitializeProvider(_configuration.SocialsProvider, OnProviderInitialized);
            Marking.InitializeProvider(_configuration.MarkingProvider, OnProviderInitialized);
            CloudSaves.InitializeProvider(_configuration.CloudSavesProvider, OnProviderInitialized);
            Payments.InitializeProvider(_configuration.PaymentsProvider, OnProviderInitialized);
            AudioSystem.InitializeProvider(_configuration.AudioSystemProvider, OnProviderInitialized);


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0)
                {
                    OnAllProvidersInitialized();
                    _controllerCreator.Run(onFinished: FinishInitialization);
                }
            }

        }


        private void OnAllProvidersInitialized()
        {
            Localization.Initialize(Env.DeviceLanguage);

            Saves.InitializeSaveData();
            GameTime.Initialize(SaveData.SaveTime, Env.TimeNowUtc);

            if (Payments.IsSupportPayments)
            {
                var products = Payments.GetNonHandledProducts();

                if (products != null && products.Count > 0)
                    Payments.HandlePayments(products);
            }

            if (_configuration.ShowBannerOnStart)
            {
                bool needHideBanner = _configuration.HideBannerIfNoAdsPurchased && SaveData.NoAdsIsPurchased;
                Ads.SetBanner(!needHideBanner);
            }

            Payments.OnPaymentHandled += OnPaymentHandled;
        }

        private void OnPaymentHandled(ProductKey productKey, bool success)
        {
            if (success && productKey == ProductKey.NoAds && _configuration.HideBannerIfNoAdsPurchased)
                Ads.SetBanner(false);
        }

        private void FinishInitialization()
        {
            SetActiveRootGameObjects(true);

            IsInitialized = true;
            OnInitialized?.Invoke();

            SingleManager.Start();
        }


        private void Update()
        {
            if (!IsInitialized) return;

            SingleManager.Update();
            ScreenState.Update();
        }

        private void FixedUpdate()
        {
            if (!IsInitialized) return;

            Saves.FixedUpdate();
            GameTime.FixedUpdate();
            SingleManager.FixedUpdate();
        }



        private void SetActiveRootGameObjects(bool active)
        {
            foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (rootObject != gameObject)
                    rootObject.SetActive(active);
            }

        }



    }
}


