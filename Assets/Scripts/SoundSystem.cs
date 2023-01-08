using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Architecture
{
    [
        RequireComponent(typeof(AudioSource))
    ]
    public class SoundSystem : Backend.AbstractSingleton<SoundSystem>
    {
        [Header("References")]
        [SerializeField] Transform sfx;
        [SerializeField] Transform music;

        readonly Dictionary<string, AudioSource> sfxDictionary = new();
        readonly Dictionary<string, AudioClip> musicDictionary = new();

        AudioSource mainMusicSource;

        protected override void SingletonAwake()
        {
            foreach (Transform child in sfx)
            {
                sfxDictionary.Add(child.name, child.GetComponent<AudioSource>());
            }
            foreach (Transform child in music)
            {
                musicDictionary.Add(child.name, child.GetComponent<AudioClip>());
            }

            mainMusicSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays a sound by name in the heirarchy.
        /// </summary>
        /// <param name="sound"></param>
        /// <exception cref="ArgumentException"></exception>
        public void PlaySound(string sound)
        {
#if UNITY_EDITOR
            if (!sfxDictionary.ContainsKey(sound))
            {
                throw new ArgumentException("Unknown/invalid sound name, ensure the GameObject containing it has the same name you have supplied to this function.");
            }
#endif
            sfxDictionary[sound].Play();
        }

        /// <summary>
        /// Stops the current song, if any, and plays the passed in song. This function ensures only one song is playing at a time.
        /// </summary>
        /// <param name="song"></param>
        /// <exception cref="ArgumentException">If the song name doesn't exist</exception>
        public void PlayMusic(string song)
        {
#if UNITY_EDITOR
            if (!musicDictionary.ContainsKey(song))
            {
                throw new ArgumentException("Unknown/invalid song name, ensure the GameObject containing it has the same name you have supplied to this function.");
            }
#endif
            mainMusicSource.Stop();
            mainMusicSource.clip = musicDictionary[song];
            mainMusicSource.Play();
        }

        /// <summary>
        /// Stops current music playback, if any.
        /// </summary>
        public void StopMusic()
        {
            mainMusicSource.Stop();
        }
    }
}