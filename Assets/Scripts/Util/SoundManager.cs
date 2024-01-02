using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI percentageText;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        Save();

        // Update the percentage text
        UpdatePercentageText(volume);
    }

    private void UpdatePercentageText(float volume)
    {
        int percentage = Mathf.RoundToInt(volume * 100);
        percentageText.text = percentage + "%";
    }

    private void Load()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = savedVolume;

        // Update the percentage text when loading
        UpdatePercentageText(savedVolume);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
