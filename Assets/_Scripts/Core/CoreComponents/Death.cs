using UnityEngine;

namespace Etorium.CoreSystem
{
    public class Death : CoreComponent
    {
        [SerializeField] private GameObject[] deathParticles;

        private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager;
        private GameManager gameManager;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private Stats stats;
    
        protected override void Awake()
        {
            base.Awake();
            gameManager = FindObjectOfType<GameManager>();
        }

        public void Die()
        {
            foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            }

            if (core.transform.parent.CompareTag("Player"))
            {
                gameManager.Respawn();
            }
        
            core.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}