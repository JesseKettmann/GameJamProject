using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    static class SoundManager
    {
        static Dictionary<string, SoundEffect> soundeffects;
        static Song song;
        
        public static void Initialise()
        {
            soundeffects = new Dictionary<string, SoundEffect>();
            song = Game1.gameInstance.Content.Load<Song>("Audio/road_to_the_stronghold");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = Settings.volume*0.2f;
            MediaPlayer.IsRepeating = true;
        }


        public static void LoadSoundEffect(string name, string source)
        {
            SoundEffect effect = Game1.gameInstance.Content.Load<SoundEffect>(source);
            soundeffects.TryAdd(name, effect);
        }

        public static void PlaySoundEffect(string name, float volume = 1, float pitch = 4, float pan = 0)
        {
            if (pitch > 3)
                pitch = 0.55f + (float)Game1.random.NextDouble() * 0.1f;
            SoundEffect effect;
            if(soundeffects.TryGetValue(name, out effect))
            {
                effect.Play(volume, pitch, pan);
            }
        }

        public static SoundEffectInstance PlaySoundEffectInstance(string name)
        {
            SoundEffect effect;
            if (soundeffects.TryGetValue(name, out effect))
            {
                SoundEffectInstance e = effect.CreateInstance();
                return e;
            }
            return null;
        }
    }
}
