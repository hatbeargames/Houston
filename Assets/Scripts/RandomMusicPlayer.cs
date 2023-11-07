using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomMusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomSong();
    }

    void PlayRandomSong()
    {
        if (songs.Length == 0)
        {
            Debug.LogError("No songs provided!");
            return;
        }

        int randomIndex = Random.Range(0, songs.Length);
        audioSource.clip = songs[randomIndex];
        audioSource.Play();

        // Schedule the PlayRandomSong method to be called again 
        // once the current song finishes playing.
        Invoke("PlayRandomSong", audioSource.clip.length);
    }
}
