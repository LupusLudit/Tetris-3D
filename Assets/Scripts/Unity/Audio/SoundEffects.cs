using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] Effects;
    public Slider VolumeSlider;
    public int poolSize = 10;

    private float volume = 0.5f;
    private List<AudioSource> audioSourcePool;
    private int nextAudioIndex = 0;

    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(SetVolume);

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

    private void SetVolume(float vol)
    {
        volume = vol;
    }
}
