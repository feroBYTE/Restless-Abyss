using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    private void Start()
    {
        // Ensure the options panel is initially deactivated
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainIntro");
    }

    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
