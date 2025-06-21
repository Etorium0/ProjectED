using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
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

    private void Awake()
    {
        // Should be a singleton, but for now we'll find it.
        // A better approach would be a static instance.
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        lastCheckpointPosition = initialSpawnPoint.position;
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void UpdateCheckpoint(Vector3 pos)
    {
        lastCheckpointPosition = pos;
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, lastCheckpointPosition, Quaternion.identity);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
}
