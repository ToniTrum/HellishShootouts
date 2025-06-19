using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public KeyCode dashKey = KeyCode.Space;

    [Header("Collision")]
    [Tooltip("Какие слои считается стеной при деше")]
    public LayerMask obstacleLayer;
    [Tooltip("Отступ от стены, чтобы не застрять в коллайдере")]
    public float skinWidth = 0.05f;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private Vector2 dashDirection;
    private Vector2 lastMoveDirection = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 inputDir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (inputDir != Vector2.zero)
            lastMoveDirection = inputDir;

        if (Input.GetKeyDown(dashKey)
            && Time.time >= lastDashTime + dashCooldown
            && !isDashing)
        {
            dashDirection = lastMoveDirection;
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        float startTime = Time.time;
        Vector2 startPos = rb.position;

        float maxDist = dashDistance;
        RaycastHit2D hit = Physics2D.Raycast(
            startPos,
            dashDirection,
            dashDistance + skinWidth,
            obstacleLayer
        );
        Vector2 targetPos;
        if (hit.collider != null)
        {
            float allowedDist = hit.distance - skinWidth;
            maxDist = Mathf.Max(0f, allowedDist);
        }
        targetPos = startPos + dashDirection * maxDist;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            Vector2 newPos = Vector2.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);
            yield return null;
        }

        rb.MovePosition(targetPos);

        isDashing = false;
    }
}
