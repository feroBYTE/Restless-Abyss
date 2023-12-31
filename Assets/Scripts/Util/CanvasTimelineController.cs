using UnityEngine;
using UnityEngine.Playables;

public class CanvasTimelineController : MonoBehaviour
{
    public PlayableDirector timeline; // Reference to your canvas timeline

    // Method to skip to a specific point in the timeline
    public void SkipToTime(float time)
    {
        // Ensure the timeline is not null
        if (timeline != null)
        {
            // Set the timeline time to the specified time
            timeline.time = time;
        }
    }
}
