using UnityEngine;


namespace UNodes.Providers
{
    public class ProviderConfiguration : MonoBehaviour
    {

        [field: SerializeField] public Env.Provider EnvProvider { get; private set; }
        [field: SerializeField] public Ads.Provider AdsProvider { get; private set; }
        [field: SerializeField] public Socials.Provider SocialsProvider { get; private set; }
        [field: SerializeField] public Payments.Provider PaymentsProvider { get; private set; }
        [field: SerializeField] public GameReady.Provider GameReadyProvider { get; private set; }
        [field: SerializeField] public CloudSaves.Provider CloudSavesProvider { get; private set; }

    }
}

