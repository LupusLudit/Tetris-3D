using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Unity.Audio
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="SoundEffects"]/*'/>
    public class SoundEffects : MonoBehaviour
    {
        public AudioClip[] Effects;
        public int poolSize = 10;

        private float volume = 0.5f;
        private List<AudioSource> audioSourcePool;
        private int nextAudioIndex = 0;


        /// <summary>
        /// Initializes the audio source pool.
        /// This setup allows multiple overlapping sound effects without creating/destroying sources at runtime.
        /// </summary>
        void Start()
        {
            audioSourcePool = new List<AudioSource>(poolSize);
            for (int i = 0; i < poolSize; i++)
            {
                GameObject audioObj = new GameObject("PooledAudioSource_" + i);
                audioObj.transform.SetParent(this.transform);
                AudioSource source = audioObj.AddComponent<AudioSource>();
                source.playOnAwake = false;
                audioSourcePool.Add(source);
            }
        }

        /// <summary>
        /// Plays a sound effect by index from the Effects array.
        /// </summary>
        /// <param name="index">Index of the sound effect clip to play.</param>
        public void PlayEffect(int index)
        {
            if (index < 0 || index >= Effects.Length)
            {
                Debug.LogWarning("Invalid sound effect index.");
                return;
            }

            AudioSource source = GetNextAvailableAudioSource();
            if (source != null)
            {
                source.clip = Effects[index];
                source.volume = volume;
                source.Play();
            }
        }

        /// <summary>
        /// Finds the next available AudioSource from the pool that is not currently playing.
        /// If none are available, stops the oldest playing source and returns it.
        /// </summary>
        /// <returns>An AudioSource ready to be used for playback.</returns>
        private AudioSource GetNextAvailableAudioSource()
        {
            //Find available audio source
            for (int i = 0; i < poolSize; i++)
            {
                int index = (nextAudioIndex + i) % poolSize;
                if (!audioSourcePool[index].isPlaying)
                {
                    nextAudioIndex = (index + 1) % poolSize;
                    return audioSourcePool[index];
                }
            }

            //If no available audio source is found, return a fallback
            AudioSource fallback = audioSourcePool[nextAudioIndex];
            fallback.Stop();
            nextAudioIndex = (nextAudioIndex + 1) % poolSize;
            return fallback;
        }

        /// <summary>
        /// Sets the sound effect volume.
        /// </summary>
        /// <param name="vol">Volume level (0.0 to 1.0).</param>
        public void SetVolume(float vol) => volume = vol;
    }
}
