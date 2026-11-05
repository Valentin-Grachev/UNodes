using UnityEngine;
using UNodes.Providers.Missing;
using UNodes.Providers.Test;


namespace UNodes.Providers
{
    public class ProviderConfiguration : MonoBehaviour
    {
        [field: SerializeField] public float LocalAutosaveIntervalSec { get; private set; } = 1f;
        [field: SerializeField] public bool UseCloudAutosave { get; private set; } = false;
        [field: SerializeField] public float CloudAutosaveIntervalSec { get; private set; } = 1f;


        [Space]
        [SerializeField] private Env.Provider _envProvider;
        [SerializeField] private Ads.Provider _adsProvider;
        [SerializeField] private Socials.Provider _socialsProvider;
        [SerializeField] private Payments.Provider _paymentsProvider;
        [SerializeField] private GameReady.Provider _gameReadyProvider;
        [SerializeField] private CloudSaves.Provider _cloudSavesProvider;



        public Env.Provider EnvProvider => 
            _envProvider ??= gameObject.AddComponent<MissingEnvProvider>();

        public Ads.Provider AdsProvider =>
            _adsProvider ??= gameObject.AddComponent<MissingAdsProvider>();

        public Socials.Provider SocialsProvider =>
            _socialsProvider ??= gameObject.AddComponent<MissingSocialsProvider>();

        public Payments.Provider PaymentsProvider =>
            _paymentsProvider ??= gameObject.AddComponent<MissingPaymentsProvider>();

        public GameReady.Provider GameReadyProvider =>
            _gameReadyProvider ??= gameObject.AddComponent<MissingGameReadyProvider>();

        public CloudSaves.Provider CloudSavesProvider =>
            _cloudSavesProvider ??= gameObject.AddComponent<MissingCloudSavesProvider>();



    }
}

