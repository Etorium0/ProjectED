// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Boss_heal : StateMachineBehaviour
// {
//     private BossMain bossComponent;
//     private float healDuration = 3f; // Thời gian heal
//     private float healTimer = 0f;
//     private bool hasHealed = false;

//     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         bossComponent = animator.GetComponent<BossMain>();
//         healTimer = 0f;
//         hasHealed = false;
        
//         // Đảm bảo boss không thể bị gián đoạn trong 1 giây đầu
//         bossComponent.isInvulnerable = true;
        
//         Debug.Log("Boss bắt đầu heal!");
//     }

//     // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         healTimer += Time.deltaTime;
        
//         // Nếu boss bị gián đoạn trong khi heal
//         if (bossComponent.isHealing && bossComponent.healInterruptionTimer > 1f)
//         {
//             animator.SetBool("isHealing", false);
//             bossComponent.isInvulnerable = false;
//             return;
//         }
        
//         // Thực hiện heal sau khi hoàn thành animation
//         if (healTimer >= healDuration && !hasHealed)
//         {
//             bossComponent.Heal();
//             hasHealed = true;
//         }
        
//         // Kết thúc trạng thái heal
//         if (healTimer >= healDuration + 0.5f) // Thêm 0.5s để animation kết thúc
//         {
//             animator.SetBool("isHealing", false);
//             bossComponent.isInvulnerable = false;
//         }
//     }

//     // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
//     override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//     {
//         // Đảm bảo reset các trạng thái
//         bossComponent.isInvulnerable = false;
//         animator.SetBool("isHealing", false);
        
//         Debug.Log("Boss kết thúc heal!");
//     }
// } 