using System;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Etorium.DataPersistence;
using Etorium.DataPersistence.Data;
using Etorium.CoreSystem;   // Cho WeaponInventory
using Etorium.Weapons;      // Cho WeaponDataSO

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private Transform initialSpawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;
    
    public event Action<GameState> OnGameStateChanged;
    private GameState currentGameState = GameState.Gameplay;


    #region Respawn
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
            if (player != null)
            {
                player.transform.position = lastCheckpointPosition;
                player.SetActive(true); // Nếu player từng bị disable khi chết
                // Reset velocity
                var rb = player.GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = Vector2.zero;
                // Reset animation, trạng thái khác nếu cần
                CVC.m_Follow = player.transform;
                respawn = false;
                // Thông báo cho tất cả boss/enemy rằng player đã respawn
                OnPlayerRespawn.Invoke();
            }
            else
            {
                Debug.LogError("Player reference is null in GameManager!");
            }
        }
    }
    #endregion

    #region SaveLogic
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

        var weaponInventory = player.GetComponentInChildren<WeaponInventory>();
        if (weaponInventory != null && data.weaponInventoryIds != null)
        {
            WeaponDataSO[] allWeapons = Resources.LoadAll<WeaponDataSO>("WeaponData");
            for (int i = 0; i < data.weaponInventoryIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(data.weaponInventoryIds[i]))
                {
                    var weapon = Array.Find(allWeapons, w => w.Id == data.weaponInventoryIds[i]);
                    weaponInventory.TrySetWeapon(weapon, i, out _);
                }
                else
                {
                    weaponInventory.TrySetWeapon(null, i, out _); // Slot rỗng
                }
            }
            // Gán lại vũ khí đang cầm nếu cần
            // currentEquippedIndex = data.equippedWeaponIndex;
        }
    }
    
    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
        data.lastRestPosition = this.lastCheckpointPosition;
        data.playerPosition = this.lastCheckpointPosition; // Update player position too

        var weaponInventory = player.GetComponentInChildren<WeaponInventory>();
        if (weaponInventory != null)
        {
            // Lưu toàn bộ inventory
            var weaponList = weaponInventory.weaponData;
            data.weaponInventoryIds = new string[weaponList.Length];
            for (int i = 0; i < weaponList.Length; i++)
            {
                data.weaponInventoryIds[i] = weaponList[i] != null ? weaponList[i].Id : null;
            }

            // Lưu index vũ khí đang cầm (nếu có)
            data.equippedWeaponIndex = 0; // hoặc lấy đúng index vũ khí đang cầm
        }
    }
    #endregion
    
    // Getter methods
    public int GetDeathCount()
    {
        return deathCount;
    }
    
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    #region Game States

    public void ChangeState(GameState state)
    {
        if (state == currentGameState)
            return;

        switch (state)
        {
            case GameState.UI:
                EnterUIState();
                break;
            case GameState.Gameplay:
                EnterGameplayState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentGameState = state;
        OnGameStateChanged?.Invoke(currentGameState);
    }

    private void EnterUIState()
    {
        Time.timeScale = 0f;
    }

    private void EnterGameplayState()
    {
        Time.timeScale = 1f;
    }


    public enum GameState
    {
        UI,
        Gameplay
    }

    #endregion
}
