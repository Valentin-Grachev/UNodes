using System;
using GamePush;

namespace UNodes.GamePush
{
    public class GamePushAudioSystem : AudioSystem.Provider
    {

        public override bool MusicIsEnabled 
        { 
            get => !GP_Sounds.IsMuted(SoundType.Music); 
            set
            {
                if (!value) GP_Sounds.Mute(SoundType.Music);
                else GP_Sounds.Unmute(SoundType.Music);
            }
        }
        public override bool SoundIsEnabled 
        {
            get => !GP_Sounds.IsMuted(SoundType.SFX);
            set
            {
                if (!value) GP_Sounds.Mute(SoundType.SFX);
                else GP_Sounds.Unmute(SoundType.SFX);
            }
        }

        public override void Initialize(Action onInitialized)
        {
            GP_Sounds.OnMuteMusic += InvokeMusicChanged;
            GP_Sounds.OnUnmuteMusic += InvokeMusicChanged;

            GP_Sounds.OnMuteSFX += InvokeSoundChanged;
            GP_Sounds.OnUnmuteSFX += InvokeSoundChanged;


            onInitialized?.Invoke();
        }
    }
}
