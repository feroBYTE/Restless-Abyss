using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource audioSource;       // Reference to the Audio Source
    public AudioClip introClip;           // Intro music clip
    public AudioClip mainLoopClip;        // Main looping music clip

    void Start()
    {
        // Play the intro clip once
        audioSource.PlayOneShot(introClip);

        // Schedule the main loop clip to start exactly when the intro clip ends
        double introEndTime = AudioSettings.dspTime + introClip.length;
        audioSource.clip = mainLoopClip;
        audioSource.PlayScheduled(introEndTime);
        audioSource.loop = true; // Set the main loop clip to loop
    }
}
