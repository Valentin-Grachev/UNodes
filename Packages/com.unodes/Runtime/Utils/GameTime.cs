using System;
using UnityEngine;

namespace UNodes
{
    public static class GameTime
    {
        public delegate void OnTimePassedEvent(int seconds);

        public static event OnTimePassedEvent OnUnscaledTimePassed;
        public static event Action OnSecondTick;

        private static float _lastSecondPassTime = 0f;
        private static float _lastUnscaledSecondPassTime = 0f;
        private static float _gameplayTimeScale = 1f;
        private static int _pauseCount = 0;

        public static TimeSpan OfflineTime { get; private set; }
        public static bool IsFirstEnter { get; private set; }

        public static float GameplayTimeScale
        {
            get => _gameplayTimeScale;
            set
            {
                _gameplayTimeScale = value;
                if (_pauseCount == 0) Env.TimeScale = _gameplayTimeScale;
            }
        }


        public static void Initialize(DateTime lastSaveTime, DateTime timeNow)
        {
            IsFirstEnter = lastSaveTime == DateTime.MinValue;

            OfflineTime = IsFirstEnter ? TimeSpan.Zero : timeNow - lastSaveTime;
            if (OfflineTime < TimeSpan.Zero) OfflineTime = TimeSpan.Zero;
        }


        public static void Update()
        {
            if (Time.unscaledTime - _lastUnscaledSecondPassTime > 1f)
            {
                int seconds = (int)(Time.unscaledTime - _lastUnscaledSecondPassTime);
                _lastUnscaledSecondPassTime = Time.unscaledTime;

                OnUnscaledTimePassed?.Invoke(seconds);
            }

            if (Time.unscaledTime - _lastSecondPassTime > 1f)
            {
                _lastSecondPassTime = Time.unscaledTime;
                OnSecondTick?.Invoke();
            }
        }


        public static void AddPause()
        {
            _pauseCount++;
            Env.TimeScale = 0f;
        }

        public static void RemovePause()
        {
            _pauseCount = Mathf.Max(0, _pauseCount - 1);
            Env.TimeScale = _pauseCount == 0 ? _gameplayTimeScale : 0f;
        }




    }
}

