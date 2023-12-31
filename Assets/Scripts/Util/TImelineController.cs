using UnityEngine;
using UnityEngine.InputSystem;

public class TimelineController : MonoBehaviour
{
    public CanvasTimelineController canvasTimelineController; // Reference to your canvas timeline controller
    public float skipToTime = 10.0f; // Time in seconds to skip to

    void Update()
    {
        // Check if the spacebar is pressed
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Call a method in your CanvasTimelineController to skip to a specific point
            canvasTimelineController.SkipToTime(skipToTime);
        }
    }
}
