using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogEffect : MonoBehaviour
{
    public float fogDensity = 0.02f;
    public Color fogColor = Color.gray;
    public float fogStartDistance = 0f;
    public float fogEndDistance = 300f;

    private void Update()
    {
        RenderSettings.fog = true;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
    }
}
