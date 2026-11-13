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
        

        [SerializeField] private EnvKey _editorConfigurationKey;
        [SerializeField] private EnvKey _buildConfigurationKey;
        [Space]
        [SerializeField] private BaseHolderRegistrator _holderRegistrator;
        [SerializeField] private BasePaymentHandler _paymentHandler;
        [SerializeField] private BaseSaveEncoder _saveEncoder;
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
            var configurationKey = Env.IsEditor ? 
                _editorConfigurationKey : _buildConfigurationKey;

            var configuration = Resources.Load<ProviderConfiguration>("DefaultConfiguration");
            
            if (configurationKey != EnvKey.Default)
            {
                _configurations.FindOrExeption
                    (configuration => configuration.EnvProvider.EnvKey == configurationKey);
            }

            Saves.SetSaveEncoder(_saveEncoder);
            Saves.SetLocalAutosaveTimeInterval(configuration.LocalAutosaveIntervalSec);
            Saves.SetCloudAutosaveTimeInterval(configuration.CloudAutosaveIntervalSec);


            int initProcessCount = 7;

            Env.InitializeProvider(configuration.EnvProvider, OnProviderInitialized);
            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);
            GameReady.InitializeProvider(configuration.GameReadyProvider, OnProviderInitialized);
            CloudSaves.InitializeProvider(configuration.CloudSavesProvider, OnProviderInitialized);
            Payments.InitializeProvider(configuration.PaymentsProvider, OnProviderInitialized);
            AudioSystem.InitializeProvider(configuration.AudioSystemProvider, OnProviderInitialized);


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0)
                {
                    OnAllProvidersInitialized();
                    _holderRegistrator.RegisterItems(onRegistered: FinishInitialization);
                }
            }

        }


        private void OnAllProvidersInitialized()
        {
            Saves.InitializeSaveData();
            GameTime.Initialize(SaveData.SaveTime, Env.TimeNowUtc);

            if (Payments.IsSupportPayments)
            {
                var products = Payments.GetNonHandledProducts();

                if (products != null && products.Count > 0)
                    Payments.HandlePayments(products);
            }



        }

        private void FinishInitialization()
        {
            SetActiveRootGameObjects(true);

            IsInitialized = true;
            OnInitialized?.Invoke();

            HolderManager.Start();
        }


        private void Update()
        {
            if (!IsInitialized) return;

            HolderManager.Update();
        }

        private void FixedUpdate()
        {
            if (!IsInitialized) return;

            Saves.FixedUpdate();
            GameTime.FixedUpdate();
            HolderManager.FixedUpdate();
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


