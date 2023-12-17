using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    public float EffectIntensity;
    public float EffectIntensityX;
    public float EffectSpeed;
    public float BobbingResetSpeed = 5f; // Adjust the reset speed as needed
    public float SprintSpeedMultiplier = 2f; // Adjust the sprint speed multiplier as needed
    public float SprintDuration = 3f; // Adjust the sprint duration as needed
    public float SprintCooldown = 3f; // Adjust the sprint cooldown as needed

    private PositionFollower followerInstance;
    private Vector3 originalOffset;
    private float sinTime;
    private bool isMoving = false;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private float sprintCooldownTimer = 0f;

    void Start()
    {
        followerInstance = GetComponent<PositionFollower>();
        originalOffset = followerInstance.offset;
    }

    void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting && sprintCooldownTimer <= 0f)
        {
            StartCoroutine(Sprint());
        }

        if (isSprinting)
        {
            sinTime += Time.deltaTime * (EffectSpeed * SprintSpeedMultiplier);
            sprintTimer -= Time.deltaTime;

            if (sprintTimer <= 0f)
            {
                isSprinting = false;
                sprintCooldownTimer = SprintCooldown;
            }
        }
        else
        {
            sprintCooldownTimer -= Time.deltaTime;

            if (inputVector.magnitude > 0f)
            {
                sinTime += Time.deltaTime * EffectSpeed;
                isMoving = true;
            }
            else
            {
                sinTime = 0f;
                isMoving = false;

                if (!IsInvoking("ResetBobbing"))
                {
                    InvokeRepeating("ResetBobbing", 0f, 0.05f); // Adjust the interval as needed
                }
            }
        }

        float sinAmountY = -Mathf.Abs(EffectIntensity * Mathf.Sin(sinTime));
        Vector3 sinAmountX = followerInstance.transform.right * EffectIntensity * Mathf.Cos(sinTime) * EffectIntensityX;

        if (isMoving || isSprinting)
        {
            followerInstance.offset = new Vector3
            {
                x = originalOffset.x,
                y = originalOffset.y + sinAmountY,
                z = originalOffset.z
            };

            followerInstance.offset += sinAmountX;
        }
    }

    void ResetBobbing()
    {
        followerInstance.offset = Vector3.Lerp(followerInstance.offset, originalOffset, Time.deltaTime * BobbingResetSpeed);
    }

    IEnumerator Sprint()
    {
        isSprinting = true;
        sprintTimer = SprintDuration;

        yield return null;
    }
}
