using System;
using UnityEngine;
using UnityEngine.Audio;

namespace UNodes
{
    public class DefaultAudioSystemProvider : AudioSystem.Provider
    {
        private float _originSoundVolume;
        private float _originMusicVolume;


        public override bool MusicIsEnabled 
        {
            get => PlayerPrefs.GetInt("Music", 1) == 1;
            set
            {
                PlayerPrefs.SetInt("Music", value ? 1 : 0);
                AudioMixer.SetFloat("MusicVolume", value ? _originMusicVolume : -80f);
                InvokeMusicChanged();
            }
        }
        public override bool SoundIsEnabled 
        { 
            get => PlayerPrefs.GetInt("Sound", 1) == 1; 
            set
            {
                PlayerPrefs.SetInt("Sound", value ? 1 : 0);
                AudioMixer.SetFloat("SoundVolume", value ? _originSoundVolume : -80f);
                InvokeSoundChanged();
            }
        }

        public override void Initialize(Action onInitialized)
        {
            if (!AudioMixer.GetFloat("SoundVolume", out _originSoundVolume))
                throw new Exception("[Default Audio System] Audio mixer doesn't contain parameter \"SoundVolume\"");

            if (!AudioMixer.GetFloat("MusicVolume", out _originMusicVolume))
                throw new Exception("[Default Audio System] Audio mixer doesn't contain parameter \"MusicVolume\"");

            AudioMixer.SetFloat("SoundVolume", 
                PlayerPrefs.GetInt("Sound", 1) == 1 ? _originSoundVolume : -80f);

            AudioMixer.SetFloat("MusicVolume", 
                PlayerPrefs.GetInt("Music", 1) == 1 ? _originMusicVolume : -80f);

            onInitialized?.Invoke();
        }



    }
}
