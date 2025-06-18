using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Scene name for main menu
    
    private AudioManager audioManager;
    private PlayerInput playerInput;
    private InputAction escapeAction;

    void Start()
    {
        PauseMenuPanel.SetActive(false);
        
        // Setup input action for escape key
        escapeAction = new InputAction("Escape", InputActionType.Button, "<Keyboard>/escape");
        escapeAction.Enable();
        
        // Ensure EventSystem has proper UI Input Module
        SetupEventSystem();
        
        // Find AudioManager
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj != null)
            audioManager = audioObj.GetComponent<AudioManager>();
        else
            Debug.LogWarning("AudioManager not found!");
        
        // Find Player Input
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerInput = playerObj.GetComponent<PlayerInput>();
        }
    }
    
    private void SetupEventSystem()
    {
        // Check if EventSystem exists
        if (EventSystem.current == null)
        {
            // Create EventSystem if it doesn't exist
            GameObject eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
            eventSystemGO.AddComponent<InputSystemUIInputModule>();
        }
        else
        {
            // Make sure EventSystem has InputSystemUIInputModule
            if (EventSystem.current.GetComponent<InputSystemUIInputModule>() == null)
            {
                // Remove old Standalone Input Module if exists
                StandaloneInputModule oldModule = EventSystem.current.GetComponent<StandaloneInputModule>();
                if (oldModule != null)
                {
                    DestroyImmediate(oldModule);
                }
                
                // Add new Input System UI Input Module
                EventSystem.current.gameObject.AddComponent<InputSystemUIInputModule>();
            }
        }
    }

    void Update()
    {
        // Handle escape input to toggle pause/resume
        if (escapeAction.WasPressedThisFrame())
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Re-enable player input
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
        
        // Audio adjustments
        if (audioManager != null)
        {
            audioManager.musicSource.volume = audioManager.musicVolume;
            audioManager.SFXSource.pitch = Time.timeScale;
        }
        
        GameIsPaused = false;
    }

    private void Pause()
    {
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        
        // Ensure Canvas uses Unscaled Time and is interactive
        Canvas canvas = PauseMenuPanel.GetComponent<Canvas>();
        if (canvas == null)
            canvas = PauseMenuPanel.GetComponentInParent<Canvas>();
            
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // Ensure UI displays on top
            // Ensure canvas is set to Screen Space - Overlay or Camera
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }
        
        // Ensure CanvasGroup allows interaction
        CanvasGroup canvasGroup = PauseMenuPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = PauseMenuPanel.AddComponent<CanvasGroup>();
            
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        // Disable player input
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        
        // Audio adjustments
        if (audioManager != null)
        {
            audioManager.musicSource.volume = audioManager.musicVolume * 0.25f;
            audioManager.SFXSource.pitch = Time.timeScale;
        }
        
        GameIsPaused = true;
    }

    // Scene change for load menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading scene
        GameIsPaused = false;
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnDestroy()
    {
        // Cleanup input action
        escapeAction?.Disable();
        escapeAction?.Dispose();
    }
}