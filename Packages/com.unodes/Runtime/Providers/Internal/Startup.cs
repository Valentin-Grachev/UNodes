using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Project;
using UNodes.Providers;

namespace UNodes
{
    public class Startup : MonoBehaviour
    {
        public static event Action OnInitialized;
        public static bool IsInitialized { get; private set; } = false;

        private static bool _startupIsCreated = false;

        [SerializeField] private EnvKey _editorConfigurationKey;
        [SerializeField] private EnvKey _buildConfigurationKey;
        [Space]
        [SerializeField] private List<ProviderConfiguration> _configurations;



        private void Awake()
        {
            if (_startupIsCreated)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            _startupIsCreated = true;

            Initialize();
        }


        public void Initialize()
        {
            var configurationKey = Env.IsEditor ? 
                _editorConfigurationKey : _buildConfigurationKey;

            var configuration = _configurations.Find
                (configuration => configuration.EnvProvider.EnvKey == configurationKey);

            if (configuration == null)
                throw new Exception($"[Startup] Configuration with key \"{configurationKey}\" doesn't exist!");


            Saves.SetLocalAutosaveTimeInterval(configuration.LocalAutosaveIntervalSec);
            Saves.SetCloudAutosaveTimeInterval(configuration.CloudAutosaveIntervalSec);


            int initProcessCount = 6;

            Env.InitializeProvider(configuration.EnvProvider, OnProviderInitialized);
            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);
            GameReady.InitializeProvider(configuration.GameReadyProvider, OnProviderInitialized);
            CloudSaves.InitializeProvider(configuration.CloudSavesProvider, OnProviderInitialized);
            Payments.InitializeProvider(configuration.PaymentsProvider, OnProviderInitialized);


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0)
                {
                    OnAllProvidersInitialized();
                    IsInitialized = true;
                    OnInitialized?.Invoke();
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



        private void FixedUpdate()
        {
            if (!IsInitialized) return;

            Saves.FixedUpdate();
            GameTime.FixedUpdate();
        }




    }
}


