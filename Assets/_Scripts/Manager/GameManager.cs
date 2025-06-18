using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float respawnTime = 2f;

    private float respawnTimeStart;
    private bool respawn;

    private GameObject playerObj;
    private Animator anim;
    private GameObject mainCam;
    private PlayerStateList pState;
    private BonfireBehaviour bonfireBehaviour;

    private CinemachineVirtualCamera CVC;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        anim = playerObj.GetComponent<Animator>();
        pState = playerObj.GetComponent<PlayerStateList>();
        bonfireBehaviour = playerObj.GetComponent<BonfireBehaviour>();
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();

        if (CVC != null && playerObj != null)
            CVC.m_Follow = playerObj.transform;
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            // Lấy checkpoint gần nhất
            GameObject lastCheckPoint = bonfireBehaviour.lastBonfireRestedAt;
            if (lastCheckPoint == null)
                lastCheckPoint = bonfireBehaviour.defaultBonfire;

            // Đặt lại vị trí player về checkpoint (dịch sang phải 1 đơn vị nếu muốn)
            Vector3 newPos = lastCheckPoint.transform.position;
            newPos.x += 1f;
            newPos.z = playerObj.transform.position.z;
            playerObj.transform.position = newPos;

            // Reset trạng thái chết
            if (pState != null)
                pState.dead = false;

            // Reset trigger death animation
            if (anim != null)
                anim.ResetTrigger("Death");

            // Gọi restAtBonfire để hồi máu, trạng thái nghỉ,...
            if (bonfireBehaviour != null)
                bonfireBehaviour.RestAtBonfire(lastCheckPoint);

            // Đổi màu camera
            if (mainCam != null)
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(0, 0, 0, 255);

            // Đảm bảo camera follow lại player
            if (CVC != null && playerObj != null)
                CVC.m_Follow = playerObj.transform;

            respawn = false;
        }
    }
}