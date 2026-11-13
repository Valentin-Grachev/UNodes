using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UNodes
{
    public static class AddressablesExtensions
    {
        public static async UniTask<T> LoadAssetByKey<T>(this AssetReferenceT<T> assetReference) where T : Object
        {
            string key = (string)assetReference.RuntimeKey;
            return await Addressables.LoadAssetAsync<T>(key);
        }
    }
}
