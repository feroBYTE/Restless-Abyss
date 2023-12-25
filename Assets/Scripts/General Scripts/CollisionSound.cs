using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource audioSource;

    // Drag your audio clip to this field in the Unity Editor
    public AudioClip collisionSound;

    // Specify the tags for objects that should trigger the collision sound
    public string[] collisionTags;

    // Specify the maximum distance for the sound to be heard
    public float maxSoundDistance = 10f;

    void Start()
    {
        // Ensure there is an AudioSource component attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found. Please attach an AudioSource component to the GameObject.");
        }
        else
        {
            // Assign the provided audio clip to the AudioSource
            audioSource.clip = collisionSound;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object has one of the specified tags
        foreach (string tag in collisionTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                // Check the distance between the collision point and this GameObject
                float distance = Vector3.Distance(collision.contacts[0].point, transform.position);

                // Play the collision sound only if the distance is within the specified range
                if (audioSource != null && collisionSound != null && distance <= maxSoundDistance)
                {
                    audioSource.PlayOneShot(collisionSound);
                }
                else
                {
                    if (audioSource == null)
                    {
                        Debug.LogError("AudioSource is null.");
                    }
                    if (collisionSound == null)
                    {
                        Debug.LogError("Collision sound is null.");
                    }
                }

                // Break out of the loop since we found a matching tag
                break;
            }
        }
    }
}
