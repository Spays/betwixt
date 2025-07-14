using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnFlyStateNew : MonoBehaviour
{
    // Inspector variables
    [Header("Movement")]
    [SerializeField] private float airAcceleration = 10f; // How fast to accelerate horizontally in air
    [SerializeField] private float maxAirSpeed = 5f; // Max horizontal speed in air

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f; // Initial upward force for jump
    [SerializeField] private float doubleJumpForce = 8f; // Upward force for double jump (usually slightly less than jumpForce)
    [SerializeField] private bool enableDoubleJump = true; // Enable/disable double jump feature
    [SerializeField] private float jumpCutMultiplier = 0.5f; // Reduce velocity when jump button released (for variable height)
    [SerializeField] private float risingGravityScale = 1f; // Gravity multiplier when going up (lower for floatier jumps)
    [SerializeField] private float fallingGravityScale = 2f; // Gravity multiplier when falling (higher for snappier descent)

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck; // Position to check for ground (e.g., feet)
    [SerializeField] private float groundCheckRadius = 0.1f; // Radius of overlap circle
    [SerializeField] private LayerMask groundLayer; // LayerMask for ground objects

    [Header("Timers")]
    [SerializeField] private float coyoteTime = 0.1f; // Time after leaving ground to still jump
    [SerializeField] private float jumpBufferTime = 0.1f; // Time to buffer jump input before landing

    // Private variables
    private Rigidbody2D rb;
    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool isJumping;
    private bool canDoubleJump; // Tracks if double jump is available
    private bool jumpRequested; // Flag to reliably capture jump input across frames
    
    [SerializeField] private SkeletonAnimation skeletonAnimation; // Ссылка на компонент Spine
    private bool hasDoubleJumped; // Флаг для отслеживания, был ли двойной прыжок (для выбора Fall/DoubleJumpFall)
    private string currentAnimation; // Для предотвращения повторного сета одной анимации

    private PlayerInputActions controls;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        controls = new PlayerInputActions();
        controls.Player.Enable(); // Включаем Action Map "Player"
        
        // var animations = skeletonAnimation.skeleton.Data.Animations; // Получаем коллекцию анимаций
        // Debug.Log("Available animations:");
        //
        // foreach (var anim in animations.Items)
        // {
        //     if (anim != null)
        //     {
        //         Debug.Log($"- {anim.Name} (Duration: {anim.Duration}s)");
        //     }
        // }
    }

    private void Update()
    {
        // Handle jump input
        if (controls.Player.Jump.WasPressedThisFrame())
        {
            jumpRequested = true;
            jumpBufferTimer = jumpBufferTime;
        }

        // Reduce buffer timer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Variable jump height: Cut jump if button released while ascending
        if (controls.Player.Jump.WasReleasedThisFrame() && isJumping && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }
    }

    private void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Coyote time logic and double jump reset
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            isJumping = false;
            canDoubleJump = true; // Reset double jump on ground
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        // On ground: Stop horizontal movement
        if (isGrounded)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

// Handle primary jump (with coyote and buffer)
        bool canPrimaryJump = (isGrounded || coyoteTimer > 0f) && jumpBufferTimer > 0f;
        if (canPrimaryJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            coyoteTimer = 0f; // Consume coyote time
            jumpBufferTimer = 0f; // Consume buffer
            jumpRequested = false; // Reset request after handling
            hasDoubleJumped = false; // Сброс флага двойного прыжка
        }

// Handle double jump (if enabled, available, and requested)
        if (enableDoubleJump && canDoubleJump && jumpRequested && !isGrounded && !canPrimaryJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            isJumping = true;
            canDoubleJump = false; // Consume double jump
            jumpRequested = false; // Reset request after handling
            hasDoubleJumped = true; // Устанавливаем флаг двойного прыжка
        }

// Если на земле, сброс флага двойного прыжка (добавьте после if (isGrounded) { rb.velocity = new Vector2(0f, rb.velocity.y); })
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        // Aerial horizontal movement
        if (!isGrounded)
        {
            float horizontalInput = controls.Player.Move.ReadValue<float>(); // Чтение оси (float от -1 до 1)
            float targetVelocityX = horizontalInput * maxAirSpeed;
            float acceleration = airAcceleration * Time.fixedDeltaTime;
    
            // Smoothly accelerate towards target
            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, targetVelocityX, acceleration),
                rb.velocity.y
            );
        }
        
        // Gravity manipulation for better feel
        if (rb.velocity.y > 0f && isJumping)
        {
            rb.gravityScale = risingGravityScale;
        }
        else
        {
            rb.gravityScale = fallingGravityScale;
        }

        // Reset jump request at the end of FixedUpdate to catch any missed inputs
        jumpRequested = false;
    }

    // Optional: Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    
    private void OnDestroy()
    {
        controls.Player.Disable(); // Отключаем Action Map
    }
    
    private void LateUpdate()
    {
        // Определяем состояние и выбираем анимацию
        string newAnimation = string.Empty;

        if (isGrounded)
        {
            newAnimation = "jump/animation"; // На земле — Idle
        }
        else
        {
            if (rb.velocity.y > 0f)
            {
                // Подъём
                newAnimation = hasDoubleJumped ? "double_jump1" : "jump0";
            }
            else
            {
                // Падение
                newAnimation = hasDoubleJumped ? "fall_double_jump" : "fall_animation";
            }
        }

        // Устанавливаем анимацию только если она изменилась (для оптимизации)
        if (newAnimation != currentAnimation)
        {
            currentAnimation = newAnimation;
            skeletonAnimation.AnimationState.SetAnimation(0, newAnimation, true); // true для loop; false для one-shot, если нужно (например, для JumpUp)
        }
    }
}