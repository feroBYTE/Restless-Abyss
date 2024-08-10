using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalButtonSoundManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource audioSource;      // The AudioSource component to play the sounds
    public AudioClip hoverSound;         // Sound to play on hover
    public AudioClip clickSound;         // Sound to play on click

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover sound
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
