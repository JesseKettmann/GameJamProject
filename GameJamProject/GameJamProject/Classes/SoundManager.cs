﻿using Microsoft.Xna.Framework.Audio;
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

        public static void PlaySoundEffect(string name, float volume = 1, float pitch = 0, float pan = 0)
        {
            SoundEffect effect;
            if(soundeffects.TryGetValue(name, out effect))
            {
                effect.Play(volume, 0, 0);
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
