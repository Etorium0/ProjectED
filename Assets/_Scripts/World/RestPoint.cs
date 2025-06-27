using UnityEngine;
using UnityEngine.Events;

public class RestPoint : MonoBehaviour
{
    [Header("Rest Point Settings")]
    [SerializeField] private string restId;
    [SerializeField] private bool isActive = true;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject activeVisual;
    [SerializeField] private GameObject restingVisual;
    [SerializeField] private ParticleSystem restParticles;
    [SerializeField] private AudioClip restSound;
    
    [Header("Detection")]
    [SerializeField] private LayerMask playerLayer = -1;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private KeyCode restKey = KeyCode.E;
    
    public UnityEvent OnPlayerRested;
    
    private bool playerInRange = false;
    private GameManager gameManager;
    private AudioSource audioSource;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        
        // Tự động tạo ID nếu chưa có
        if (string.IsNullOrEmpty(restId))
        {
            restId = $"Rest_{transform.position.x}_{transform.position.y}";
        }
        
        UpdateVisuals();
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        // Kiểm tra player trong phạm vi
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        playerInRange = playerCollider != null && playerCollider.CompareTag("Player");
        
        // Cho phép rest khi player nhấn phím
        if (playerInRange && Input.GetKeyDown(restKey))
        {
            StartResting();
        }
        
        UpdateVisuals();
    }
    
    public void StartResting()
    {
        if (!isActive) return;
        
        // Heal player to full health
        HealPlayer();
        
        // Update rest point in GameManager
        if (gameManager != null)
        {
            gameManager.UpdateRestPoint(transform.position);
            Debug.Log($"Rest point updated in GameManager: {transform.position}");
        }
        
        // MANUAL SAVE at rest point - true Dark Souls style!
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
            Debug.Log($"Game SAVED at rest point: {restId}");
        }
        else
        {
            Debug.LogError("DataPersistenceManager not found!");
        }
        
        // Play effects
        PlayRestEffects();
        
        // Invoke events
        OnPlayerRested.Invoke();
        
        Debug.Log($"Player rested at: {restId}");
    }
    
    private void HealPlayer()
    {
        // Tìm player và heal full health
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var stats = player.GetComponentInChildren<Etorium.CoreSystem.Stats>();
            if (stats != null)
            {
                stats.Health.CurrentValue = stats.Health.MaxValue;
                Debug.Log("Player healed to full health!");
            }
        }
    }
    
    private void PlayRestEffects()
    {
        // Particle effects
        if (restParticles != null)
        {
            restParticles.Play();
        }
        
        // Sound effects
        if (restSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(restSound);
        }
    }
    
    private void UpdateVisuals()
    {
        // Show different visual based on player presence
        if (activeVisual != null)
        {
            activeVisual.SetActive(isActive && !playerInRange);
        }
        
        if (restingVisual != null)
        {
            restingVisual.SetActive(isActive && playerInRange);
        }
    }
    
    // Public methods
    public void SetActive(bool active)
    {
        isActive = active;
        UpdateVisuals();
    }
    
    public string GetRestId()
    {
        return restId;
    }
    
    public bool IsPlayerInRange()
    {
        return playerInRange;
    }
    
    private void OnDrawGizmosSelected()
    {
        // Hiển thị detection radius trong Scene view
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Hiển thị rest ID
        Gizmos.color = Color.cyan;
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"REST: {restId}");
        #endif
    }
    
    private void OnValidate()
    {
        // Auto-generate ID in editor
        if (string.IsNullOrEmpty(restId))
        {
            restId = $"Rest_{GetInstanceID()}";
        }
    }
}
