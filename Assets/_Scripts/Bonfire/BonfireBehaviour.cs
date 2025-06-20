// using UnityEngine;
// using UnityEngine.Events;
// using TMPro;
// using Etorium.CoreSystem;
//
// public class BonfireBehaviour : MonoBehaviour
// {
//     public UnityEvent bonfireUsedEvent;
//     [SerializeField] private TMP_Text bonfireRestText;
//     [SerializeField] private float maxAlpha;
//
//     [SerializeField] public GameObject defaultBonfire;
//     private GameObject lastBonfireCloseTo;
//     public GameObject lastBonfireRestedAt;
//
//     private bool inRange;
//     private bool tooClose;
//
//     public Core core { get; private set; }
//     public Stats stats { get; private set; }
//     public Movement movement { get; private set; }
//     public Animator anim { get; private set; }
//     public AudioManager audioManager { get; private set; }
//     public Player player { get; private set; }
//
//     void Start()
//     {
//         core = GetComponentInChildren<Core>();
//         stats = core.GetCoreComponent<Stats>();
//         movement = core.GetCoreComponent<Movement>();
//         anim = GetComponent<Animator>();
//         audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
//         player = GetComponent<Player>();
//
//         if (lastBonfireCloseTo == null)
//             lastBonfireCloseTo = defaultBonfire;
//         if (lastBonfireRestedAt == null)
//             lastBonfireRestedAt = defaultBonfire;
//
//         bonfireRestText.SetText("");
//     }
//
//     void Update()
//     {
//         UpdateBonfireText();
//     }
//
//     void UpdateBonfireText()
//     {
//         bool isResting = IsPlayerResting();
//
//         if (!inRange || tooClose)
//         {
//             bonfireRestText.SetText("");
//         }
//         else if (!isResting)
//         {
//             bonfireRestText.SetText("PRESS Q TO REST");
//         }
//         else
//         {
//             bonfireRestText.SetText("PRESS Q TO GET UP");
//         }
//     }
//
//     // Hàm gọi khi muốn rest (ví dụ gọi từ PlayerState hoặc input handler)
//     public void RestAtBonfire(GameObject bonfire)
//     {
//         stats.Health.CurrentValue = stats.Health.MaxValue;
//         if (player != null && player.RestState != null)
//         {
//             player.StateMachine.ChangeState(player.RestState);
//         }
//
//         // Xoay mặt về phía bonfire
//         if (bonfire.transform.position.x < transform.position.x)
//         {
//             if (movement.FacingDirection > 0)
//                 movement.Flip();
//         }
//         else
//         {
//             if (movement.FacingDirection < 0)
//                 movement.Flip();
//         }
//
//         lastBonfireRestedAt = bonfire;
//         bonfireUsedEvent.Invoke();
//     }
//
//     public bool IsPlayerResting()
//     {
//         return player != null && player.StateMachine.CurrentState == player.RestState;
//     }
//
//     void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.gameObject.CompareTag("CheckpointRangeCheck"))
//         {
//             inRange = true;
//             lastBonfireCloseTo = collision.gameObject;
//         }
//         if (collision.gameObject.CompareTag("Checkpoint"))
//         {
//             tooClose = true;
//             lastBonfireCloseTo = collision.gameObject;
//         }
//         if (collision.gameObject.CompareTag("SoundRange"))
//         {
//             inRange = true;
//             audioManager.PlaySFX(audioManager.bonfireidle);
//         }
//     }
//
//     void OnTriggerExit2D(Collider2D collision)
//     {
//         if (collision.gameObject.CompareTag("CheckpointRangeCheck"))
//         {
//             inRange = false;
//         }
//         if (collision.gameObject.CompareTag("Checkpoint"))
//         {
//             tooClose = false;
//         }
//         if (collision.gameObject.CompareTag("SoundRange"))
//         {
//             inRange = false;
//             audioManager.PlaySFX(audioManager.bonfireidle);
//         }
//     }
// }