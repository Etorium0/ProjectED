using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugSaveUI : MonoBehaviour
{
    [Header("Debug UI")]
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private KeyCode toggleKey = KeyCode.F1;
    
    private GameManager gameManager;
    private bool isDebugVisible = false;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (debugPanel != null)
        {
            debugPanel.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDebugUI();
        }
        
        // Keyboard shortcuts for debug commands
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DebugSave();
        }
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            DebugLoad();
        }
        
        if (Input.GetKeyDown(KeyCode.F10))
        {
            DebugNewGame();
        }
        
        if (isDebugVisible)
        {
            UpdateDebugText();
        }
    }
    
    private void ToggleDebugUI()
    {
        isDebugVisible = !isDebugVisible;
        
        if (debugPanel != null)
        {
            debugPanel.SetActive(isDebugVisible);
        }
    }
    
    private void UpdateDebugText()
    {
        if (debugText == null) return;
        
        string debugInfo = $"=== SAVE DEBUG INFO ===\n";
        
        // Get data from GameManager (death count, rest position)
        if (gameManager != null)
        {
            debugInfo += $"Death Count: {gameManager.GetDeathCount()}\n";
            debugInfo += $"Rest Point: {gameManager.GetLastCheckpointPosition()}\n";
        }
        
        // Get player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            debugInfo += $"Player Pos: {player.transform.position}\n";
        }
        
        // Check if save file exists
        bool hasSaveData = DataPersistenceManager.instance?.HasGameData() ?? false;
        debugInfo += $"Has Save: {(hasSaveData ? "✓" : "✗")}\n";
        
        debugInfo += $"\n=== CONTROLS ===\n";
        debugInfo += $"F1: Toggle Debug UI\n";
        debugInfo += $"F5: Manual Save (Force)\n";
        debugInfo += $"F9: Load Game\n";
        debugInfo += $"F10: New Game\n";
        debugInfo += $"E: Rest (when near Rest Point)\n";
        debugInfo += $"\n=== TRUE DARK SOULS STYLE ===\n";
        debugInfo += $"• Only saves at Rest Points/Bonfires\n";
        debugInfo += $"• No auto-save on scene change\n";
        debugInfo += $"• No auto-save on quit\n";
        debugInfo += $"• Quit without rest = lose progress!\n";
        
        debugText.text = debugInfo;
    }
    
    private string FormatTime(float totalSeconds)
    {
        int hours = Mathf.FloorToInt(totalSeconds / 3600f);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        
        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
    
    // Debug buttons using DataPersistenceManager
    public void DebugSave()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("DEBUG: Manual save triggered!");
        }
        else
        {
            Debug.LogError("DataPersistenceManager not found!");
        }
    }
    
    public void DebugLoad()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.LoadGame();
            Debug.Log("DEBUG: Game reloaded!");
        }
        else
        {
            Debug.LogError("DataPersistenceManager not found!");
        }
    }
    
    public void DebugNewGame()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.NewGame();
            Debug.Log("DEBUG: New game started!");
        }
        else
        {
            Debug.LogError("DataPersistenceManager not found!");
        }
    }
}
