using UnityEngine;

namespace Etorium.CoreSystem
{
    public class Death : CoreComponent
    {
        [Header("Death Settings")]
        [SerializeField] private GameObject[] deathParticles;
        [SerializeField] private bool forceAsPlayer = false;
        
        [Header("Enemy Death Settings")]
        [SerializeField] private bool dropItemsOnDeath = false;
        [SerializeField] private GameObject[] itemDrops;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager;
        private GameManager gameManager;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private Stats stats;
        
        private bool cachedIsPlayer;
    
        protected override void Awake()
        {
            base.Awake();
            
            // Cache player check để tránh check tag mỗi lần
            cachedIsPlayer = forceAsPlayer || IsPlayerByTag() || IsPlayerByLayer();
            
            // Chỉ tìm GameManager nếu là player (performance)
            if (cachedIsPlayer)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
        }

        public void Die()
        {
            // Common death effects cho tất cả entities
            PlayDeathEffects();

            // Phân biệt logic death
            if (cachedIsPlayer)
            {
                HandlePlayerDeath();
            }
            else
            {
                HandleEnemyDeath();
            }
        }
        
        private void PlayDeathEffects()
        {
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }
        }
        
        private void HandlePlayerDeath()
        {
            // Trigger respawn (player sẽ respawn với full health)
            if (gameManager != null)
            {
                gameManager.Respawn();
            }
            
            core.transform.parent.gameObject.SetActive(false);
        }
        
        private void HandleEnemyDeath()
        {
            // Enemy death logic - có thể mở rộng
            // if (dropItemsOnDeath && itemDrops.Length > 0)
            // {
            //     DropItems();
            // }
            
            // Có thể thêm: give experience, update kill count, etc.
            
            core.transform.parent.gameObject.SetActive(false);
        }
        
        // private void DropItems()
        // {
        //     foreach (var item in itemDrops)
        //     {
        //         if (item != null)
        //         {
        //             Instantiate(item, transform.position, Quaternion.identity);
        //         }
        //     }
        // }
        
        // Multiple ways to detect player
        private bool IsPlayerByTag()
        {
            return core.transform.parent.CompareTag("Player");
        }
        
        private bool IsPlayerByLayer()
        {
            return core.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player");
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
        
        // Public methods để access từ bên ngoài
        public bool IsPlayer()
        {
            return cachedIsPlayer;
        }
        
        // Method để force change player status (nếu cần)
        public void SetAsPlayer(bool isPlayer)
        {
            cachedIsPlayer = isPlayer;
            
            if (isPlayer && gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
        }
    }
}