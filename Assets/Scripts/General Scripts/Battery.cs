using UnityEngine;

public class Battery : MonoBehaviour
{
    public float batteryValue = 40f;
    public AudioClip collectSound;
    private bool isCollected = false;
    public float rotationSpeed = 50f; // Degrees per second
    public float floatAmplitude = 0.2f; // Height variation
    public float floatFrequency = 1f; // Cycles per second

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isCollected)
            return;

        // Rotate
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Float
        float newY = Mathf.Sin(Time.time * Mathf.PI * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected || other.gameObject.tag != "Player")
        {
            return;
        }

        Flashlight flashlight = FindObjectOfType<Flashlight>();

        if (flashlight != null && flashlight.GetBatteryLevel() < 100f)
        {
            CollectBattery();
        }
    }

    private void CollectBattery()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        if (flashlight != null)
        {
            flashlight.SetNearbyBattery(this);
            if (flashlight.additionalAudioSource != null && collectSound != null)
            {
                flashlight.additionalAudioSource.PlayOneShot(collectSound);
            }
        }
        isCollected = true;
        Destroy(gameObject, 0.1f);
    }
}
