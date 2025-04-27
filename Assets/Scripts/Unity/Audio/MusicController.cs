using UnityEngine;

namespace Assets.Scripts.Unity.Audio
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="MusicController"]/*'/>
    public class MusicController : MonoBehaviour
    {
        private AudioSource musicSource;
        public AudioClip[] MusicTracks;

        /*
        Links to music tracks used:
        1. https://pixabay.com/music/classical-string-quartet-tetris-theme-korobeiniki-rearranged-arr-for-strings-185592/
        2. https://www.youtube.com/watch?v=NmCCQxVBfyM
        3. https://www.youtube.com/watch?v=5wsy1S2ezyw
        4. https://www.youtube.com/watch?v=98H2UObS-mY
        5. https://www.youtube.com/watch?v=E8FQBjVlERk
        */

        /// <summary>
        /// Initializes the AudioSource component and sets initial volume and track.
        /// Stops music from playing automatically on start.
        /// </summary>
        private void Start()
        {
            musicSource = GetComponent<AudioSource>();
            musicSource.volume = 0.5f;

            musicSource.clip = MusicTracks[0];
            StopMusic(); // Stop music to prevent it from playing on start
        }

        /// <summary>
        /// Sets the volume level of the music playback.
        /// </summary>
        /// <param name="volume">Volume level (range 0.0 to 1.0).</param>
        public void SetVolume(float volume)
        {
            musicSource.volume = volume;
        }

        /// <summary>
        /// Enables or disables music playback.
        /// If enabling, starts playing the current track if it is not already playing.
        /// If disabling, pauses the music.
        /// </summary>
        /// <param name="isOn"><c>true</c> to enable music, <c>false</c> to disable.</param>
        public void ToggleMusic(bool isOn)
        {
            musicSource.enabled = isOn;

            if (isOn && (!musicSource.isPlaying || musicSource.time == 0)) musicSource.Play();
            else musicSource.Pause();
        }

        /// <summary>
        /// Switches the current music track to the specified index and plays it if music is enabled.
        /// </summary>
        /// <param name="trackIndex">Index of the music track to switch to.</param>
        /// <param name="musicOn">Whether the music should play after switching.</param>
        public void SwitchTrack(int trackIndex, bool musicOn)
        {
            if (trackIndex < MusicTracks.Length)
            {
                musicSource.clip = MusicTracks[trackIndex];
                if (musicOn)
                {
                    musicSource.Play();
                }
            }
        }

        /// <summary>
        /// Stops the currently playing music.
        /// </summary>
        public void StopMusic() => musicSource.Stop();

        /// <summary>
        /// Starts playing the current music track.
        /// </summary>
        public void PlayMusic() => musicSource.Play();
    }
}
