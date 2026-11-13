using UnityEngine;
using UNodes.Providers.Default;


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
        [SerializeField] private EnableValue<AudioSystem.Provider> _audioSystemProvider;


        public float LocalAutosaveIntervalSec => _localAutosaveIntervalSec.enabled ?
            _localAutosaveIntervalSec.value : float.PositiveInfinity;

        public float CloudAutosaveIntervalSec => _cloudAutosaveIntervalSec.enabled ?
            _cloudAutosaveIntervalSec.value : float.PositiveInfinity;

        public Env.Provider EnvProvider => _envProvider.enabled ? 
            _envProvider.value : GetOrCreateProvider<DefaultEnvProvider>();

        public Ads.Provider AdsProvider => _adsProvider.enabled ? 
            _adsProvider.value : GetOrCreateProvider<DefaultAdsProvider>();

        public Socials.Provider SocialsProvider => _socialsProvider.enabled ?
            _socialsProvider.value : GetOrCreateProvider<DefaultSocialsProvider>();

        public Payments.Provider PaymentsProvider => _paymentsProvider.enabled ? 
            _paymentsProvider.value : GetOrCreateProvider<DefaultPaymentsProvider>();

        public GameReady.Provider GameReadyProvider => _gameReadyProvider.enabled ?
            _gameReadyProvider.value : GetOrCreateProvider<DefaultGameReadyProvider>();

        public CloudSaves.Provider CloudSavesProvider => _cloudSavesProvider.enabled ? 
            _cloudSavesProvider.value : GetOrCreateProvider<DefaultCloudSavesProvider>();

        public AudioSystem.Provider AudioSystemProvider => _audioSystemProvider.enabled ?
            _audioSystemProvider.value : GetOrCreateProvider<DefaultAudioSystemProvider>();



        private TProvider GetOrCreateProvider<TProvider>() where TProvider : CommonProvider
        {
            var provider = gameObject.GetComponent<TProvider>();
            return provider != null ? provider : gameObject.AddComponent<TProvider>();
        }


    }
}

