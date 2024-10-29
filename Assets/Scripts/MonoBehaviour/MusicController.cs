using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider volumeSlider;
    public Toggle musicToggle;
    public TMP_Dropdown musicDropdown;
    public AudioClip[] musicTracks;

    /*
     * links to music tracks used (proper documentation will be added later)
     * 1.https://pixabay.com/music/classical-string-quartet-tetris-theme-korobeiniki-rearranged-arr-for-strings-185592/
     * 2.https://www.youtube.com/watch?v=NmCCQxVBfyM
     * 3.https://www.youtube.com/watch?v=5wsy1S2ezyw
     * 4.https://www.youtube.com/watch?v=98H2UObS-mY
     * 5.https://www.youtube.com/watch?v=E8FQBjVlERk
     */

    private void Start()
    {
        musicSource.volume = 0.5f;
        volumeSlider.value = 0.5f;

        if (musicTracks.Length > 0)
        {
            musicSource.clip = musicTracks[0];
            musicSource.Play();
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        musicDropdown.onValueChanged.AddListener(SwitchTrack);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }

    public void SwitchTrack(int trackIndex)
    {
        if (trackIndex < musicTracks.Length)
        {
            musicSource.clip = musicTracks[trackIndex];
            if (musicToggle.isOn)
            {
                musicSource.Play();
            }
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
