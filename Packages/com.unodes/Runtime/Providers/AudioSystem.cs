using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace UNodes
{
    public static class AudioSystem
    {
        public static event Action OnMusicChanged;
        public static event Action OnSoundChanged;

        private static Provider _provider;
        private static AudioPool _audioPool;
        private static AudioMixerGroup _soundMixerGroup;
        private static AudioMixerGroup _musicMixerGroup;
        private static float _originSoundVolume;
        private static float _originMusicVolume;


        public abstract class Provider : CommonProvider
        {
            public AudioMixer AudioMixer => Resources.Load<AudioMixer>("AudioMixer");


            public event Action OnMusicChanged; public void InvokeMusicChanged() => OnMusicChanged?.Invoke();
            public event Action OnSoundChanged; public void InvokeSoundChanged() => OnSoundChanged?.Invoke();

            public abstract bool MusicIsEnabled { get; set; }
            public abstract bool SoundIsEnabled { get; set; }

        }

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(onInitialized: () =>
            {
                _provider = provider;
                _provider.OnMusicChanged += () => OnMusicChanged?.Invoke();
                _provider.OnSoundChanged += () => OnSoundChanged?.Invoke();

                var audioPool = new GameObject("AudioPool");
                audioPool.transform.SetParent(Startup.Container);
                _audioPool = audioPool.AddComponent<AudioPool>();

                _soundMixerGroup = _provider.AudioMixer.FindMatchingGroups("Sound")[0];
                _musicMixerGroup = _provider.AudioMixer.FindMatchingGroups("Music")[0];

                onInitialized?.Invoke();
            });



        public static bool MusicIsEnabled
        {
            get => _provider.MusicIsEnabled;
            set => _provider.MusicIsEnabled = value;
        }

        public static bool SoundIsEnabled
        {
            get => _provider.SoundIsEnabled;
            set => _provider.SoundIsEnabled = value;
        }


        public static async UniTask<AudioSource> Play(AudioUnit audioUnit)
        {
            var audioClip = await audioUnit.LoadAudioClip();

            bool isSound = audioUnit.Channel == AudioUnit.ChannelType.Sound;

            var audioSource = isSound ? _audioPool.GetAudioSource() : _audioPool.MusicAudioSource;

            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = isSound ? _soundMixerGroup : _musicMixerGroup;
            audioSource.volume = audioUnit.Volume;
            audioSource.loop = audioUnit.IsLoop;
            audioSource.pitch = audioUnit.Pitch;

            audioSource.Play();

            return audioSource;
        }




    }
}
