using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private Text deathCountText;
    [SerializeField] private Text restPositionText;
    [SerializeField] private Text playerPositionText;
    [SerializeField] private Text instructionsText;
    
    private GameManager gameManager;
    private bool isVisible = false;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (instructionsText != null)
        {
            instructionsText.text = "F1: Toggle Debug UI\nF5: Manual Save\nF9: Load Game\nF10: New Game";
        }
        
        // Hide panel initially if assigned
        if (debugPanel != null)
        {
            debugPanel.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Handle input
        HandleInput();
        
        // Update UI if visible
        if (isVisible)
        {
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        if (gameManager != null)
        {
            if (deathCountText != null)
                deathCountText.text = $"Deaths: {gameManager.GetDeathCount()}";
                
            if (restPositionText != null)
                restPositionText.text = $"Rest Position: {gameManager.GetLastCheckpointPosition()}";
                
            if (playerPositionText != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    playerPositionText.text = $"Player Position: {player.transform.position}";
                else
                    playerPositionText.text = "Player: Not Found";
            }
        }
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleUI();
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // Manual save
            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Manual save triggered!");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            // Reload game
            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.LoadGame();
                Debug.Log("Game reloaded!");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F10))
        {
            // New game
            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.NewGame();
                Debug.Log("New game started!");
            }
        }
    }
    
    private void ToggleUI()
    {
        isVisible = !isVisible;
        
        if (debugPanel != null)
        {
            debugPanel.SetActive(isVisible);
        }
        else
        {
            // Fallback: toggle entire gameObject
            gameObject.SetActive(isVisible);
        }
        
        Debug.Log($"Debug UI: {(isVisible ? "ON" : "OFF")}");
    }
}
