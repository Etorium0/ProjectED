using UnityEngine;
using UnityEngine.Events;

public class Bonfire : MonoBehaviour
{
    public UnityEvent bonfireUsedEvent = new UnityEvent();
    private GameManager gameManager;
    private float lastUsedTime = 0f;
    private const float COOLDOWN_TIME = 1f; // Thời gian cooldown để tránh spam

    // You can add a particle effect or a light to show activation
    [SerializeField] private GameObject activationVisual;
    [SerializeField] private string bonfireId; 

    private void Start()
    {
        // Find the GameManager in the scene. 
        // A Singleton pattern for GameManager would be more efficient.
        gameManager = FindObjectOfType<GameManager>();

        // Auto-generate ID if empty
        if (string.IsNullOrEmpty(bonfireId))
        {
            bonfireId = $"Bonfire_{transform.position.x}_{transform.position.y}";
        }

        if (activationVisual != null)
        {
            activationVisual.SetActive(false);
        }
        
        // Lắng nghe event khi player respawn để reset bonfire visual
        GameManager.OnPlayerRespawn.AddListener(ResetBonfireOnPlayerRespawn);
    }

    private void OnDestroy()
    {
        // Cleanup event listener
        GameManager.OnPlayerRespawn.RemoveListener(ResetBonfireOnPlayerRespawn);
    }

    private void ResetBonfireOnPlayerRespawn()
    {
        if (activationVisual != null)
        {
            activationVisual.SetActive(false);
        }
        Debug.Log("Bonfire visual reset on player respawn");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > lastUsedTime + COOLDOWN_TIME)
        {
            UseBonfire(other);
        }
    }

    private void UseBonfire(Collider2D playerCollider)
    {
        Debug.Log("Bonfire activated!");
        lastUsedTime = Time.time;
        
        var playerCore = playerCollider.GetComponentInChildren<Etorium.CoreSystem.Core>();
        if (playerCore != null)
        {
            var stats = playerCore.GetCoreComponent<Etorium.CoreSystem.Stats>();
            if (stats != null)
            {
                stats.Health.Init(); // Heal the player to full
                Debug.Log("Player healed to full health.");
            }
            else
            {
                Debug.LogError("Stats Component not found on Player's Core!");
            }
        }
        else
        {
            Debug.LogError("Core not found on Player!");
        }
        
        // Update the rest point in the GameManager
        if (gameManager != null)
        {
            gameManager.UpdateRestPoint(transform.position + (Vector3.right * 1.5f));
            Debug.Log("Rest point updated to: " + transform.position);
            
            // MANUAL SAVE at bonfire - true Dark Souls style!
            if (DataPersistenceManager.instance != null)
            {
                DataPersistenceManager.instance.SaveGame();
                Debug.Log($"Game SAVED at bonfire: {bonfireId}");
            }
            else
            {
                Debug.LogError("DataPersistenceManager not found!");
            }
            
            if (activationVisual != null)
            {
                activationVisual.SetActive(true);
            }
            
            bonfireUsedEvent.Invoke();
        }
        else
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }
} 