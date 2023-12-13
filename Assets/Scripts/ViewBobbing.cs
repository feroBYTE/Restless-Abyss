using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{

    public float EffectIntensity;
    public float EffectIntensityX;
    public float EffectSpeed;

    private PositionFollower followerInstance;
    private Vector3 originalOffset;
    private float sinTime;

    void Start()
    {
        followerInstance = GetComponent<PositionFollower>();
        originalOffset = followerInstance.offset;
    }

    void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        if (inputVector.magnitude > 0f)
        {
            sinTime += Time.deltaTime * EffectSpeed;
        }
        else
        {
            sinTime = 0f;
        }

        float sinAmountY = -Mathf.Abs(EffectIntensity * Mathf.Sin(sinTime));
        Vector3 sinAmountX = followerInstance.transform.right * EffectIntensity * Mathf.Cos(sinTime) * EffectIntensityX;

        followerInstance.offset = new Vector3
        {
            x = originalOffset.x,
            y = originalOffset.y + sinAmountY,
            z = originalOffset.z
        };

        followerInstance.offset += sinAmountX;

    }
}
