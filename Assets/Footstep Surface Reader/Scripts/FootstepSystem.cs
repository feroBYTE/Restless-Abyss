using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootstepSystem : MonoBehaviour
{
    [Range(0, 20f)]
    public float walkFrequency = 10.0f;
    [Range(0, 20f)]
    public float sprintFrequency = 20.0f;
    public UnityEvent onFootStep;

    private bool isWalking = false;
    private bool isSprinting = false;
    private bool wasSprinting = false;
    private bool isTriggered = false;

    void Update()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        // Update movement state
        wasSprinting = isSprinting;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && inputMagnitude > 0.5f)
        {
            isSprinting = true;
        }

        // Check if walking state has changed
        isWalking = !isSprinting && inputMagnitude > 0;

        // Play footsteps if walking or sprinting
        if (isWalking || isSprinting)
        {
            StartFootsteps();
        }
    }

    private void StartFootsteps()
    {
        float speed = isSprinting ? 9f : 5f;

        float frequency = isSprinting ? sprintFrequency : walkFrequency;

        // Adjust frequency based on state change
        if (wasSprinting && !isSprinting)
        {
            frequency = walkFrequency;
        }

        if (isWalking || isSprinting)
        {
            float sin = Mathf.Sin(Time.time * frequency);

            if (sin > 0.97f && !isTriggered)
            {
                isTriggered = true;
                Debug.Log("Footstep Triggered");
                onFootStep.Invoke();
            }
            else if (isTriggered && sin < -0.97f)
            {
                isTriggered = false;
            }
        }
    }
}
