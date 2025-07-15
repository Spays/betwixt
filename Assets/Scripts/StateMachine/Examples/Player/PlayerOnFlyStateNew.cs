using System;
using Player;
using Spine.Unity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOnFlyStateNew : MonoBehaviour
{
    public PlayerConfig _playerConfig;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck; // Position to check for ground (e.g., feet)
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.2f, 0.1f); // Size of the overlap box (width, height)
    [SerializeField] private LayerMask groundLayer; // LayerMask for ground objects
    
    public static event Action OnTeleported;
    
    // Private variables
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded; // Tracks previous grounded state to detect landing
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float jumpCooldownTimer; // Timer for jump cooldown after landing
    private bool isJumping;
    private bool canDoubleJump; // Tracks if double jump is available
    private bool jumpRequested; // Flag to reliably capture jump input across frames
    
    [SerializeField] private SkeletonAnimation skeletonAnimation; // Ссылка на компонент Spine
    private bool hasDoubleJumped; // Флаг для отслеживания, был ли двойной прыжок (для выбора Fall/DoubleJumpFall)
    private string currentAnimation; // Для предотвращения повторного сета одной анимации

    private PlayerInputActions controls;
    private float horizontalInput; // Хранение ввода по горизонтали (читаем в Update)
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        controls = new PlayerInputActions();
        controls.Player.Enable(); // Включаем Action Map "Player"
        
        skeletonAnimation.skeleton.ScaleX = 1f; // Начальное направление (вправо)
        
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

    private int StepFlag = 0;
    private void Update()
    {
        if (isGrounded == false && controls.Player.Teleport.WasPressedThisFrame())
        {
            Debug.Log(PlayerStats.Instance);
            if(PlayerStats.Instance.playerFlowerTeleportation >= PlayerStats.Instance.essenceDoorstep)
            {
                if (StepFlag < PlayerStats.Instance.teleportPoints.Length-1)
                {
                    transform.position = PlayerStats.Instance.teleportPoints[StepFlag].transform.position;
                    StepFlag++;
                }
                else
                {
                    transform.position = PlayerStats.Instance.teleportPoints[StepFlag].transform.position;
                    StepFlag = 0;
                }

                OnTeleported();

                PlayerStats.Instance.playerFlowerTeleportation = PlayerStats.Instance.playerFlowerTeleportation - PlayerStats.Instance.teleportTax;
            }
            Debug.Log("Телепорт");
            return;
        }
        
        // Handle jump input
        if (controls.Player.Jump.WasPressedThisFrame())
        {
            jumpRequested = true;
            jumpBufferTimer = _playerConfig.jumpBufferTime;
        }

        // Reduce buffer timer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Variable jump height: Cut jump if button released while ascending
        if (controls.Player.Jump.WasReleasedThisFrame() && isJumping && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * _playerConfig.jumpCutMultiplier);
        }
        
        // Чтение горизонтального ввода (для движения и разворота)
        horizontalInput = controls.Player.Move.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer) != null;
        
        // Detect landing and start cooldown timer
        if (!wasGrounded && isGrounded)
        {
            jumpCooldownTimer = _playerConfig.jumpCooldown; // Start cooldown on landing
        }
        wasGrounded = isGrounded; // Update previous state

        // Reduce cooldown timer
        if (jumpCooldownTimer > 0f)
        {
            jumpCooldownTimer -= Time.fixedDeltaTime;
        }

        // Coyote time logic and double jump reset
        if (isGrounded)
        {
            coyoteTimer = _playerConfig.coyoteTime;
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
            
            // НОВАЯ ЛОГИКА: Разворот на земле по вводу (без движения)
            if (Mathf.Abs(horizontalInput) > 0.01f) // Небольшой порог, чтобы избежать jitter
            {
                skeletonAnimation.skeleton.ScaleX = Mathf.Sign(horizontalInput);
            }
        }

        // Handle primary jump (with coyote, buffer, and cooldown)
        bool canPrimaryJump = (isGrounded || coyoteTimer > 0f) && jumpBufferTimer > 0f && jumpCooldownTimer <= 0f;
        if (canPrimaryJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, _playerConfig.jumpForce);
            isJumping = true;
            coyoteTimer = 0f; // Consume coyote time
            jumpBufferTimer = 0f; // Consume buffer
            jumpRequested = false; // Reset request after handling
            hasDoubleJumped = false; // Сброс флага двойного прыжка
        }

        // Handle double jump (if enabled, available, and requested)
        if (_playerConfig.enableDoubleJump && canDoubleJump && jumpRequested && !isGrounded && !canPrimaryJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, _playerConfig.doubleJumpForce);
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
            float targetVelocityX = horizontalInput * _playerConfig.maxAirSpeed;
            float acceleration = _playerConfig.airAcceleration * Time.fixedDeltaTime;
    
            // Smoothly accelerate towards target
            rb.velocity = new Vector2(
                Mathf.MoveTowards(rb.velocity.x, targetVelocityX, acceleration),
                rb.velocity.y
            );
        }
        
        // Gravity manipulation for better feel
        if (rb.velocity.y > 0f && isJumping)
        {
            rb.gravityScale = _playerConfig.risingGravityScale;
        }
        else
        {
            rb.gravityScale = _playerConfig.fallingGravityScale;
        }

        // Reset jump request at the end of FixedUpdate to catch any missed inputs
        jumpRequested = false;
        
        // Flip based on movement direction (разворот при изменении направления движения по X) — только в воздухе
        if (!isGrounded && Mathf.Abs(rb.velocity.x) > 0.01f) // Небольшой порог, чтобы избежать изменений при нулевой скорости
        {
            skeletonAnimation.skeleton.ScaleX = Mathf.Sign(rb.velocity.x);
        }
    }

    // Optional: Visualize ground check in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
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