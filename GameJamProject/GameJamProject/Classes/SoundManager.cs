using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJamProject
{
    static class SoundManager
    {


        static Dictionary<string, SoundEffect> soundeffects;
        
        public static void Initialise()
        {
            soundeffects = new Dictionary<string, SoundEffect>();
        }


        public static void LoadSoundEffect(string name, string source)
        {
            SoundEffect effect = Game1.gameInstance.Content.Load<SoundEffect>(source);
            soundeffects.TryAdd(name, effect);
        }

        public static void PlaySoundEffect(string name)
        {
            SoundEffect effect;
            if(soundeffects.TryGetValue(name, out effect))
            {
                effect.Play();
            }
        }
    }
}
