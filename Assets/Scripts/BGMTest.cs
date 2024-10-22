using UnityEngine;

public class BGMTest : MonoBehaviour
{
    public AudioClip bgmClip;    // Drag and drop your BGM audio file in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Create an AudioSource component if one doesn't already exist
        audioSource = gameObject.AddComponent<AudioSource>();

        // Set the AudioSource settings
        audioSource.clip = bgmClip;        // Assign the BGM clip
        audioSource.loop = true;           // Set to loop the BGM
        audioSource.playOnAwake = true;    // Start playing when the game starts

        // Play the BGM
        audioSource.Play();
    }
}
