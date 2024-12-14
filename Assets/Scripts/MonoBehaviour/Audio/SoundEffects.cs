using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource effectSource;
    public AudioClip[] Effects;

    private void Start()
    {
        effectSource = GetComponent<AudioSource>();
        effectSource.volume = 0.75f;
    }
    public void PlayEffect(int index)
    {
        effectSource.clip = Effects[index];
        effectSource.Play();
    }
}
