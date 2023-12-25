using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Tag to identify objects that can be grabbed
    private const string CanGrabTag = "CanGrab";

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "CanGrab" tag
        if (collision.collider.CompareTag(CanGrabTag))
        {
            // Ignore the collision with objects having the "CanGrab" tag
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
        }
    }
}
