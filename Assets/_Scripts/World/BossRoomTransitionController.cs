using UnityEngine;
using UnityEngine.Events;

namespace Etorium
{
    public class BossRoomTransitionController : MonoBehaviour
    {
        [SerializeField] private Boss boss;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject roomLocker;
        [SerializeField] private Bonfire bonfire;

        private bool isInTransitionTrigger = false;
        private Vector2 prevPos;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (roomLocker)
            {
                roomLocker.GetComponent<BoxCollider2D>().enabled = false;
            }

            if (bonfire != null)
            {
                bonfire.bonfireUsedEvent.AddListener(DeactivateDoorLock);
            }
            
            // Lắng nghe event khi player respawn để reset boss room
            GameManager.OnPlayerRespawn.AddListener(ResetBossRoomOnPlayerRespawn);
        }

        private void OnDestroy()
        {
            // Cleanup event listeners
            if (bonfire != null)
            {
                bonfire.bonfireUsedEvent.RemoveListener(DeactivateDoorLock);
            }
            GameManager.OnPlayerRespawn.RemoveListener(ResetBossRoomOnPlayerRespawn);
        }

        private void ResetBossRoomOnPlayerRespawn()
        {
            // Reset boss room khi player respawn
            DeactivateDoorLock();
            isInTransitionTrigger = false;
            Debug.Log("Boss room reset on player respawn");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isInTransitionTrigger = true;
                prevPos = player.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && isInTransitionTrigger && boss.GetCurrentHealth() > 0)
            {
                isInTransitionTrigger = false;

                Vector2 newPos = player.transform.position;
                Vector2 triggerPos = transform.position;

                bool crossedTheTrigger = (prevPos.x < triggerPos.x && newPos.x > triggerPos.x);

                if (crossedTheTrigger)
                {
                    ActivateDoorLock();
                }
            }
        }

        public void ActivateDoorLock()
        {
            if (boss != null)
            {
                boss.playerFound = true;
            }

            if (roomLocker != null)
            {
                roomLocker.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        public void DeactivateDoorLock()
        {
            if (boss != null)
            {
                boss.playerFound = false;
            }

            if (roomLocker != null)
            {
                roomLocker.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
