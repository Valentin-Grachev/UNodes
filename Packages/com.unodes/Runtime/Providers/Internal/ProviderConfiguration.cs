using UnityEngine;
using UNodes.Providers.Missing;
using UNodes.Providers.Test;


namespace UNodes.Providers
{
    public class ProviderConfiguration : MonoBehaviour
    {
        [SerializeField] private EnableValue<float> _localAutosaveIntervalSec = new()
        {
            enabled = true,
            value = 1f
        };

        [SerializeField] private EnableValue<float> _cloudAutosaveIntervalSec = new()
        {
            enabled = false,
            value = 60f
        };



        [Space(15)]
        [SerializeField] private EnableValue<Env.Provider> _envProvider;
        [SerializeField] private EnableValue<Ads.Provider> _adsProvider;
        [SerializeField] private EnableValue<CloudSaves.Provider> _cloudSavesProvider;
        [SerializeField] private EnableValue<Payments.Provider> _paymentsProvider;
        [SerializeField] private EnableValue<Socials.Provider> _socialsProvider;
        [SerializeField] private EnableValue<GameReady.Provider> _gameReadyProvider;


        public float LocalAutosaveIntervalSec => _localAutosaveIntervalSec.enabled ?
            _localAutosaveIntervalSec.value : float.PositiveInfinity;

        public float CloudAutosaveIntervalSec => _cloudAutosaveIntervalSec.enabled ?
            _cloudAutosaveIntervalSec.value : float.PositiveInfinity;

        public Env.Provider EnvProvider => _envProvider.enabled ? 
            _envProvider.value : gameObject.AddComponent<MissingEnvProvider>();

        public Ads.Provider AdsProvider => _adsProvider.enabled ? 
            _adsProvider.value : gameObject.AddComponent<MissingAdsProvider>();

        public Socials.Provider SocialsProvider => _socialsProvider.enabled ?
            _socialsProvider.value : gameObject.AddComponent<MissingSocialsProvider>();

        public Payments.Provider PaymentsProvider => _paymentsProvider.enabled ? 
            _paymentsProvider.value : gameObject.AddComponent<MissingPaymentsProvider>();

        public GameReady.Provider GameReadyProvider => _gameReadyProvider.enabled ?
            _gameReadyProvider.value : gameObject.AddComponent<MissingGameReadyProvider>();

        public CloudSaves.Provider CloudSavesProvider => _cloudSavesProvider.enabled ? 
            _cloudSavesProvider.value : gameObject.AddComponent<MissingCloudSavesProvider>();



    }
}

