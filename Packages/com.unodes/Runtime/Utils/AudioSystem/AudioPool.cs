using System.Collections.Generic;
using UnityEngine;

namespace UNodes
{
    public class AudioPool : MonoBehaviour
    {
        private AudioSource _musicAudioSource;
        private Stack<AudioSource> _freeAudioSources = new();
        private List<AudioSource> _busyAudioSources = new();
        

        public AudioSource MusicAudioSource => _musicAudioSource ??= CreateAudioSource();


        private void Awake()
        {
            GameTime.OnSecondTick += OnSecondTick;
        }

        public AudioSource GetAudioSource()
        {
            if (_freeAudioSources.Count > 0)
            {
                var audioSource = _freeAudioSources.Pop();
                _busyAudioSources.Add(audioSource);
                return audioSource;
            }
            else
            {
                var newAudioSource = gameObject.AddComponent<AudioSource>();
                newAudioSource.playOnAwake = false;
                _busyAudioSources.Add(newAudioSource);
                return newAudioSource;
            }
        }


        private void OnSecondTick()
        {
            foreach (var audioSource in _busyAudioSources)
            {
                if (!audioSource.isPlaying)
                    _freeAudioSources.Push(audioSource);
            }

            _busyAudioSources.RemoveAll(audioSource => !audioSource.isPlaying);
        }


        private AudioSource CreateAudioSource()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            return audioSource;
        }



    }
}



