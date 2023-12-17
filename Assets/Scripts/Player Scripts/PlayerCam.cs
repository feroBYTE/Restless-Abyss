using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public float bobbingIntensity;
    public float bobbingSpeed;

    private float bobbingOffset;

    float xRotation;
    float yRotation;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        ApplyBobbing();

        // Apply bobbingOffset before xRotation
        transform.rotation = Quaternion.Euler(bobbingOffset + xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void ApplyBobbing()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) != 0f || Mathf.Abs(vertical) != 0f)
        {
            bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingIntensity;
        }
        else
        {
            bobbingOffset = 0f;
        }
    }
}
