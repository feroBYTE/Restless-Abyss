using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Prologue", LoadSceneMode.Single);
    }
}
