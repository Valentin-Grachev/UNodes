using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace UNodes.GamePush
{
    public class GamePushCloudSavesProvider : CloudSaves.Provider
    {
        [SerializeField] private string _saveKey = "saveData";

        private string _saveData = string.Empty;
        private readonly List<Action<bool>> _pendingCommits = new();
        private bool _syncInProgress;
        private bool _commitQueued;

        public override bool IsSupportCloudSaves => true;

        public override string GetSaveDataString() => _saveData;

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            GP_Player.OnSyncComplete += OnSyncComplete;
            GP_Player.OnSyncError += OnSyncError;

            _saveData = ReadSaveData();
            onInitialized?.Invoke();
        }

        public override void Commit(string saveDataString, Action<bool> onHandled = null)
        {
            _saveData = saveDataString ?? string.Empty;
            GP_Player.Set(_saveKey, _saveData);

            if (onHandled != null) _pendingCommits.Add(onHandled);

            if (_syncInProgress)
            {
                _commitQueued = true;
                return;
            }

            StartSync();
        }

        private void StartSync()
        {
            _syncInProgress = true;
            GP_Player.Sync(SyncStorageType.cloud);
        }

        private void OnSyncComplete() => FinishSync(true);

        private void OnSyncError() => FinishSync(false);

        private void FinishSync(bool success)
        {
            if (!_syncInProgress) return;

            var handlers = _pendingCommits.ToArray();
            _pendingCommits.Clear();
            _syncInProgress = false;

            if (_commitQueued)
            {
                _commitQueued = false;
                StartSync();
            }

            foreach (var handler in handlers) handler?.Invoke(success);
        }

        private string ReadSaveData()
            => GP_Player.Has(_saveKey) ? GP_Player.GetString(_saveKey) ?? string.Empty : string.Empty;

        private void OnDestroy()
        {
            GP_Player.OnSyncComplete -= OnSyncComplete;
            GP_Player.OnSyncError -= OnSyncError;
        }
    }
}
