using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to your Pause Menu UI
    public string mainMenuSceneName = "MainMenu"; // Name of your Main Menu scene
    public MonoBehaviour[] componentsToDisable; // Components like player controller
    public AudioSource ambienceAudioSource; // Reference to the Ambience AudioSource

    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden and time is normal at the start
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // Show Pause Menu
        Time.timeScale = 0f;          // Freeze the game

        foreach (var component in componentsToDisable)
        {
            component.enabled = false; // Disable components like player controller
        }

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause the ambience sound
        if (ambienceAudioSource != null)
        {
            ambienceAudioSource.Pause(); // Pauses the ambience audio
        }

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        Time.timeScale = 1f;          // Resume the game

        foreach (var component in componentsToDisable)
        {
            component.enabled = true; // Re-enable components
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Resume the ambience sound
        if (ambienceAudioSource != null)
        {
            ambienceAudioSource.UnPause(); // Resumes the ambience audio
        }

        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;  // Make sure the time scale is reset before loading the main menu
        SceneManager.LoadScene(mainMenuSceneName);  // Load the Main Menu scene
    }
}
