using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] Effects;
    public Slider VolumeSlider;

    private float volume = 0.5f;

    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void PlayEffect(int index)
    {
        if (index < 0 || index >= Effects.Length)
        {
            Debug.LogWarning("Invalid sound effect index.");
            return;
        }

        GameObject tempAudioSourceObject = new GameObject("TempAudioSource");
        AudioSource tempAudioSource = tempAudioSourceObject.AddComponent<AudioSource>();

        tempAudioSource.clip = Effects[index];
        tempAudioSource.volume = volume;
        tempAudioSource.Play();

        Destroy(tempAudioSourceObject, Effects[index].length);
    }

    private void SetVolume(float vol) =>
        volume = vol;
}
