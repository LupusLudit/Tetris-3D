using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private AudioSource musicSource;
    public Slider VolumeSlider;
    public Toggle MusicToggle;
    public TMP_Dropdown MusicDropdown;
    public AudioClip[] MusicTracks;

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
        musicSource = GetComponent<AudioSource>();
        musicSource.volume = 0.5f;
        VolumeSlider.value = 0.5f;

        musicSource.clip = MusicTracks[0];
        StopMusic(); // Stop music to prevent it from playing on start

        VolumeSlider.onValueChanged.AddListener(SetVolume);
        MusicToggle.onValueChanged.AddListener(ToggleMusic);
        MusicDropdown.onValueChanged.AddListener(SwitchTrack);
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
        if (trackIndex < MusicTracks.Length)
        {
            musicSource.clip = MusicTracks[trackIndex];
            if (MusicToggle.isOn)
            {
                musicSource.Play();
            }
        }
    }
    public void StopMusic() =>
        musicSource.Stop();

    public void PlayMusic() =>
        musicSource.Play();
}
