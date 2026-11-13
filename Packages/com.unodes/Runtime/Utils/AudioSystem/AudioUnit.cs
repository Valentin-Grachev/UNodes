using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace UNodes
{
    [CreateAssetMenu(menuName = "UNodes/Audio Unit", fileName = "AudioUnit")]
    public class AudioUnit : ScriptableObject
    {
        public enum ChannelType { Sound, Music }


        [Space(10), SerializeField, Label("▶ PLAY")] private bool _clickToPlay;

        [field: Space(15)]
        [field: SerializeField] public ChannelType Channel { get; private set; } = ChannelType.Sound;
        [field: SerializeField] public bool IsLoop { get; private set; } = false;

        [field: Space(15)]
        [field: SerializeField] public AssetReferenceT<AudioClip> AudioClipReference { get; private set; }

        [Space(15)]
        [SerializeField] private bool _useRandomVolume;
        [SerializeField, HideIf(nameof(_useRandomVolume))] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, MinMaxSlider(0f, 1f), ShowIf(nameof(_useRandomVolume))] private Vector2 _randomVolume;
        [Space(15)]
        [SerializeField] private bool _useRandomPitch;
        [SerializeField, Range(0.1f, 3f), HideIf(nameof(_useRandomPitch))] private float _pitch = 1f;
        [SerializeField, MinMaxSlider(0.1f, 3f), ShowIf(nameof(_useRandomPitch))] private Vector2 _randomPitch;


        public async UniTask<AudioClip> LoadAudioClip() => await AudioClipReference.LoadAssetByKey();

        public float Volume => _useRandomVolume ?
            Random.Range(_randomVolume.x, _randomVolume.y) : _volume;

        public float Pitch => _useRandomPitch ?
            Random.Range(_randomPitch.x, _randomPitch.y) : _pitch;


        public void Play() => AudioSystem.Play(this).Forget();

        public async UniTask<AudioSource> PlayAsync() => await AudioSystem.Play(this);


        private void OnValidate()
        {
            if (Application.isPlaying && _clickToPlay) Play();
            _clickToPlay = false;
        }


    }




}


