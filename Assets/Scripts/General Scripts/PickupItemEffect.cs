using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemEffect : MonoBehaviour
{
    public float rotationSpeed = 150f; // Degrees per second, can be adjusted in the inspector
    public float floatAmplitude = 0.1f; // Height variation of the floating effect
    public float floatFrequency = 1f; // Cycles per second for the floating effect

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // Record the initial position to use as the base for the floating effect
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Float();
    }

    private void Rotate()
    {
        // Rotates the object around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void Float()
    {
        // Calculate the new Y position using a sine wave for a smooth floating effect
        float newY = Mathf.Sin(Time.time * Mathf.PI * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0, newY, 0);
    }
}
