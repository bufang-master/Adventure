using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SGF;

namespace Scripts
{
    public class AudioControl
    {
        private static string BGMusic;

        public static AudioSource audioSourceBG;
        
        public static Dictionary<string, AudioClip> audio = new Dictionary<string, AudioClip>();

        public static void PlayBGMusic(AudioSource audio,float vol = 0.5f)
        {
            audioSourceBG = audio;
            if (AppConfig.Value.enableBgMusic)
            {
                audioSourceBG.volume = vol;
                audioSourceBG.Play();
            }
        }
        public static void StopBGMusic()
        {
            audioSourceBG.Stop();
        }

        public static void PlayEffect(string path,float vol = 1.0f)
        {
            if (AppConfig.Value.enableSoundEffect)
            {
                AudioClip clip = LoadClip(path);
                if (clip != null)
                {
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position,vol);
                }
                else
                {
                    //输出错误日志
                }
            }
        }

        public static AudioClip LoadClip(string path)
        {
            string name = path.Split('/').Last();
            if (!audio.ContainsKey(name))
            {
                AudioClip clip = Resources.Load<AudioClip>(path);
                if (clip != null)
                {
                    audio.Add(name, clip);
                }
            }
            return audio[name];
        }
    }
}
