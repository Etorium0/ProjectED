using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Etorium.DataPersistence;
using Etorium.DataPersistence.Data;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private Transform initialSpawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private Vector3 lastCheckpointPosition;

    private CinemachineVirtualCamera CVC;
    
    // Event để thông báo khi player respawn
    public static UnityEvent OnPlayerRespawn = new UnityEvent();
    
    // Death count
    private int deathCount = 0;

    private void Awake()
    {
        // Should be a singleton, but for now we'll find it.
        // A better approach would be a static instance.
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        
        // Khởi tạo với vị trí spawn ban đầu
        lastCheckpointPosition = initialSpawnPoint.position;
    }

    private void Start()
    {
        // Player position will be loaded by DataPersistenceManager automatically
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void UpdateRestPoint(Vector3 pos)
    {
        lastCheckpointPosition = pos;
        Debug.Log($"Rest point updated to: {pos}");
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
        
        // Tăng death count
        deathCount++;
        Debug.Log($"Death count: {deathCount}");
    }

    private void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, lastCheckpointPosition, Quaternion.identity);
            playerTemp.SetActive(true);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
            
            // Thông báo cho tất cả boss/enemy rằng player đã respawn
            OnPlayerRespawn.Invoke();
        }
    }
    
    // IDataPersistence implementation
    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
        
        // Load rest position if it exists
        if (data.lastRestPosition != Vector3.zero)
        {
            lastCheckpointPosition = data.lastRestPosition;
            
            // Move player to last rest position if player exists
            if (player != null && player.activeInHierarchy)
            {
                player.transform.position = data.lastRestPosition;
                Debug.Log($"Player loaded at rest position: {data.lastRestPosition}");
            }
        }
        else
        {
            // New game - use initial spawn point
            lastCheckpointPosition = initialSpawnPoint.position;
            if (player != null && player.activeInHierarchy)
            {
                player.transform.position = initialSpawnPoint.position;
                Debug.Log($"New game - Player at initial spawn: {initialSpawnPoint.position}");
            }
        }
    }
    
    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
        data.lastRestPosition = this.lastCheckpointPosition;
        data.playerPosition = this.lastCheckpointPosition; // Update player position too
    }
    
    // Getter methods
    public int GetDeathCount()
    {
        return deathCount;
    }
    
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
