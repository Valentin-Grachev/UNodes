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


        [SerializeField] private EnvKey _editorConfigurationKey;
        [SerializeField] private EnvKey _buildConfigurationKey;
        [Space]
        [SerializeField] private List<ProviderConfiguration> _configurations;

        private const float INF = 10e6f;



        private void Awake() => Initialize();


        public void Initialize()
        {
            var configurationKey = Env.IsEditor ? 
                _editorConfigurationKey : _buildConfigurationKey;

            var configuration = _configurations.Find
                (configuration => configuration.EnvProvider.EnvKey == configurationKey);


            Saves.SetLocalAutosaveTimeInterval(configuration.LocalAutosaveIntervalSec);
            Saves.SetCloudAutosaveTimeInterval(configuration.CloudAutosaveIntervalSec);


            int initProcessCount = 6;

            bool paymentsIsInitialized = false;
            bool savesIsInitialized = false;
            bool envIsInitialized = false;

            Env.InitializeProvider(configuration.EnvProvider, () =>
            {
                envIsInitialized = true;
                TryInitializeGameTime();

                OnProviderInitialized();
            });

            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);
            GameReady.InitializeProvider(configuration.GameReadyProvider, OnProviderInitialized);

            CloudSaves.InitializeProvider(configuration.CloudSavesProvider, () =>
            {
                Saves.InitializeSaveData();
                
                savesIsInitialized = true;

                TryInitializeGameTime();
                TryHandleNonHandledPayments();

                OnProviderInitialized();
            });

            Payments.InitializeProvider(configuration.PaymentsProvider, () =>
            {
                paymentsIsInitialized = true;

                TryHandleNonHandledPayments();
                OnProviderInitialized();
            });


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0)
                {
                    IsInitialized = true;
                    OnInitialized?.Invoke();
                }
            }


            void TryInitializeGameTime()
            {
                if (savesIsInitialized && envIsInitialized)
                    GameTime.Initialize(SaveData.SaveTime, Env.TimeNowUtc);
            }

            void TryHandleNonHandledPayments()
            {
                if (savesIsInitialized && paymentsIsInitialized && Payments.IsSupportPayments)
                {
                    var products = Payments.GetNonHandledProducts();

                    if (products != null && products.Count > 0) 
                        Payments.HandlePayments(products);
                }
            }
        }

        private void Update()
        {
            if (!IsInitialized) return;

            Saves.Update();
            GameTime.Update();
        }




    }
}


