using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
