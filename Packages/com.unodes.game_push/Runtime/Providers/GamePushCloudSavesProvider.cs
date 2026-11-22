using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UNodes.GamePush
{
    public class GamePushCloudSavesProvider : CloudSaves.Provider
    {
        [SerializeField] private string _saveKey = "saveData";

        private string _saveData;

        public override bool IsSupportCloudSaves => true;

        public override void Commit(string saveDataString, Action<bool> onHandled = null)
            => YandexGamesSdk.SetSaves(_saveKey, saveDataString, () => onHandled?.Invoke(true));

        public override string GetSaveDataString() => _saveData;

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => YandexGamesSdk.IsInitialized);

            YandexGamesSdk.RequestSaves(_saveKey, onDataReceived: (saveData) =>
            {
                _saveData = saveData;
                onInitialized?.Invoke();
            });
        }
    }
}
