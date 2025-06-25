using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TrailRenderer))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    [Tooltip("Сколько стамины расходуется при деше")]
    public float dashStaminaCost = 20f;
    [Tooltip("Скорость восстановления стамины (единиц в секунду)")]
    public float staminaRegenRate = 10f;
    public KeyCode dashKey = KeyCode.Space;

    [Header("Collision")]
    [Tooltip("Какие слои считаются стеной при деше")]
    public LayerMask obstacleLayer;
    [Tooltip("Отступ от стены, чтобы не застрять в коллайдере")]
    public float skinWidth = 0.05f;

    [Header("References")]
    [Tooltip("Ссылка на компонент PlayerStats для управления стаминой")]
    public PlayerStats playerStats;

    private Rigidbody2D rb;
    private Animator animator;
    private TrailRenderer trail;
    private bool isDash = false;
    private float lastDashTime = -Mathf.Infinity;
    private Vector2 dashDirection;
    private Vector2 lastMoveDirection = Vector2.right;

    private static readonly int IsDashHash = Animator.StringToHash("isDash");
    private static readonly int IsWalkHash = Animator.StringToHash("isWalk");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
            trail.emitting = false;
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        // Регенерация стамины
        if (!isDash && playerStats != null)
        {
            float toRegen = staminaRegenRate * Time.deltaTime;
            if (playerStats.CurrentStamina < playerStats.MaxStamina)
                playerStats.RestoreStamina(toRegen);
        }

        if (isDash)
            return;

        Vector2 inputDir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (inputDir != Vector2.zero)
            lastMoveDirection = inputDir;

        // Проверяем возможность дэша: есть ли стамина и время кулдауна
        if (Input.GetKeyDown(dashKey) && Time.time >= lastDashTime + dashCooldown)
        {
            if (playerStats != null && !playerStats.UseStamina(dashStaminaCost))
                return; // недостаточно выносливости

            dashDirection = lastMoveDirection;
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDash = true;
        lastDashTime = Time.time;

        animator.SetBool(IsWalkHash, false);
        animator.SetBool(IsDashHash, true);
        animator.Play("PlayerDash", 0, 0f);

        if (trail != null)
            trail.emitting = true;

        Vector2 startPos = rb.position;
        float startTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(
            startPos,
            dashDirection,
            dashDistance + skinWidth,
            obstacleLayer
        );

        float maxDist = dashDistance;
        if (hit.collider != null)
            maxDist = Mathf.Max(0f, hit.distance - skinWidth);

        Vector2 targetPos = startPos + dashDirection * maxDist;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            rb.MovePosition(Vector2.Lerp(startPos, targetPos, t));
            yield return null;
        }

        rb.MovePosition(targetPos);

        animator.SetBool(IsDashHash, false);
        if (trail != null)
            trail.emitting = false;

        animator.Play("PlayerIdle", 0, 0f);
        isDash = false;
    }
}
