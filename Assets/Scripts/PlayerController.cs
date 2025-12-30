using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerName = "Player1";
    public bool isFrozen = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 1;
    public float dashForce = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Key Bindings")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey1 = KeyCode.Space;
    public KeyCode jumpKey2 = KeyCode.W;
    public KeyCode crouchKey = KeyCode.S;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Dash Settings")]
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;

    private Rigidbody2D rb;
    public Rigidbody2D RB      // <<< 正确做法：用 getter 暴露 rb
    {
        get { return rb; }
    }

    private bool isGrounded;
    private int jumpCount;
    private bool isDashing;
    private float lastMoveDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 冻结
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 冷却
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // 地面检测
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if (isGrounded && !wasGrounded)
            jumpCount = 0;

        // 跳跃
        if ((Input.GetKeyDown(jumpKey1) || Input.GetKeyDown(jumpKey2)) &&
            (isGrounded || jumpCount < maxJumps))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        // 移动
        float move = 0f;
        bool left = Input.GetKey(leftKey);
        bool right = Input.GetKey(rightKey);

        if (left && right)
            move = 0f;
        else if (left)
            move = -1f;
        else if (right)
            move = 1f;

        if (!isDashing)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
            if (move != 0) lastMoveDirection = move;
        }

        // Dash
        if (Input.GetKeyDown(dashKey) && !isDashing && dashCooldownTimer <= 0f)
        {
            StartCoroutine(DoDash(lastMoveDirection));
            dashCooldownTimer = dashCooldown;
        }
    }

    private IEnumerator DoDash(float direction)
    {
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(direction * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
}
