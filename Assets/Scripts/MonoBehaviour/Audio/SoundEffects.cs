using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] Effects;
    public float Volume = 0.75f;

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
        tempAudioSource.volume = Volume;
        tempAudioSource.Play();

        Destroy(tempAudioSourceObject, Effects[index].length);
    }
}
