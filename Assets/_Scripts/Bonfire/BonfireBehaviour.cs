using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Etorium.CoreSystem;

public class BonfireBehaviour : MonoBehaviour
{
    public UnityEvent bonfireUsedEvent;
    [SerializeField] private TMP_Text bonfireRestText;
    [SerializeField] private float maxAlpha;

    [SerializeField] public GameObject defaultBonfire;
    private GameObject lastBonfireCloseTo;
    public GameObject lastBonfireRestedAt;

    private bool inRange;
    private bool tooClose;
    private bool isResting;

    private Core core;
    private Stats stats;
    private Movement movement;
    private Animator anim;
    private AudioManager audioManager;

    void Start()
    {
        // Lấy Core từ player (giả sử BonfireBehaviour gắn vào player)
        core = GetComponentInParent<Core>();
        stats = core.GetCoreComponent<Stats>();
        movement = core.GetCoreComponent<Movement>();
        anim = GetComponentInParent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        if (lastBonfireCloseTo == null)
            lastBonfireCloseTo = defaultBonfire;
        if (lastBonfireRestedAt == null)
            lastBonfireRestedAt = defaultBonfire;

        bonfireRestText.SetText("");

        isResting = true;
        SetPlayerImmobile();

        restAtBonfire(defaultBonfire);
    }

    void Update()
    {
        CheckPlayerRest();
    }

    void CheckPlayerRest()
    {
        // Nếu có trạng thái chết/dash thì return (tùy bạn muốn quản lý ở đâu)
        // if (isDead || isDashing) return;

        if (!inRange || tooClose)
        {
            bonfireRestText.SetText("");
        }
        else if (!isResting)
        {
            bonfireRestText.SetText("PRESS Q TO REST");
        }

        if (isResting)
        {
            bonfireRestText.SetText("PRESS Q TO GET UP");
        }

        if (Input.GetKeyDown(KeyCode.Q) && (inRange || isResting))
        {
            if (!isResting)
            {
                restAtBonfire(lastBonfireCloseTo);
            }
            else
            {
                bonfireRestText.SetText("PRESS Q TO REST");
                SetPlayerMobile();
                isResting = false;
            }
        }

        if (anim != null)
            anim.SetBool("Resting", isResting);
    }

    public void restAtBonfire(GameObject bonfire)
    {
        // Hồi máu
        stats.Health.CurrentValue = stats.Health.MaxValue;

        SetPlayerImmobile();

        // Xoay mặt về phía bonfire
        if (bonfire.transform.position.x < transform.position.x)
        {
            if (movement.FacingDirection > 0)
                movement.Flip();
        }
        else
        {
            if (movement.FacingDirection < 0)
                movement.Flip();
        }

        isResting = true;
        bonfireRestText.SetText("PRESS Q TO GET UP");
        lastBonfireRestedAt = bonfire;
        if (anim != null)
            anim.SetBool("Resting", true);
        bonfireUsedEvent.Invoke();
    }

    private void SetPlayerImmobile()
    {
        movement.CanSetVelocity = false;
        movement.SetVelocityZero();
        // Nếu có input handler thì disable input ở đây
    }

    private void SetPlayerMobile()
    {
        movement.CanSetVelocity = true;
        // Nếu có input handler thì enable input ở đây
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckpointRangeCheck"))
        {
            inRange = true;
            lastBonfireCloseTo = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            tooClose = true;
            lastBonfireCloseTo = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("SoundRange"))
        {
            inRange = true;
            audioManager.PlaySFX(audioManager.bonfireidle);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckpointRangeCheck"))
        {
            inRange = false;
        }
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            tooClose = false;
        }
        if (collision.gameObject.CompareTag("SoundRange"))
        {
            inRange = false;
            audioManager.PlaySFX(audioManager.bonfireidle);
        }
    }
}