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
        [SerializeField] private float _localSaveTimeIntervalSec = 1f;
        [Space]
        [SerializeField] private List<ProviderConfiguration> _configurations;


        private void Awake() => Initialize();


        public void Initialize()
        {
            Saves.SetLocalSaveTimeInterval(_localSaveTimeIntervalSec);

            var configurationKey = Env.IsEditor ? 
                _editorConfigurationKey : _buildConfigurationKey;

            var configuration = _configurations.Find
                (configuration => configuration.EnvProvider.EnvKey == configurationKey);

            int initProcessCount = 6;
            bool paymentsIsInitialized = false;
            bool savesIsInitialized = false;

            Env.InitializeProvider(configuration.EnvProvider, OnProviderInitialized);
            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);
            GameReady.InitializeProvider(configuration.GameReadyProvider, OnProviderInitialized);

            CloudSaves.InitializeProvider(configuration.CloudSavesProvider, () =>
            {
                Saves.InitializeSaveData();

                savesIsInitialized = true;

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

            void TryHandleNonHandledPayments()
            {
                if (savesIsInitialized && paymentsIsInitialized && Payments.IsSupportPayments)
                {
                    foreach (var productKey in Payments.GetNonHandledProducts())
                        PaymentHandler.ApplyPurchaseReward(productKey);
                }
            }
        }

    }
}


