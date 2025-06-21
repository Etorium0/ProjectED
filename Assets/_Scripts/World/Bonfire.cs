using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private GameManager gameManager;
    private bool activated = false;

    // You can add a particle effect or a light to show activation
    [SerializeField] private GameObject activationVisual; 

    private void Start()
    {
        // Find the GameManager in the scene. 
        // A Singleton pattern for GameManager would be more efficient.
        gameManager = FindObjectOfType<GameManager>();

        if (activationVisual != null)
        {
            activationVisual.SetActive(false);
        }
    }

    // Use OnTriggerEnter for 3D games or OnTriggerEnter2D for 2D games.
    private void OnTriggerEnter(Collider other)
    {
        if (activated) return; // Don't reactivate

        // Check if the object that entered the trigger is the player.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bonfire activated!");
            
            // Update the checkpoint in the GameManager
            if (gameManager != null)
            {
                gameManager.UpdateCheckpoint(transform.position);
                activated = true;

                if (activationVisual != null)
                {
                    activationVisual.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("GameManager not found in the scene!");
            }
        }
    }
} 