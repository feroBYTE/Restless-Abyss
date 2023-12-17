using System.Collections;
using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform Target;
    public float RotationSmoothing = 20f;

    void Update()
    {
        Quaternion targetRotation = Target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSmoothing);
    }
}
