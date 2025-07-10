using System;
using UnityEngine;

// Контроллер игрока
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 8000f;
    [SerializeField] private float deshForce = 8000f;
    [SerializeField] private float maxDownSpeed = 10f;

    private StateMachine stateMachine;
    private Rigidbody2D rb;
    private bool isGrounded = true;

    //
    
    // Свойства для состояний
    public bool IsGrounded => isGrounded;
    public Rigidbody2D Rigidbody => rb;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float JumpForce => jumpForce;
    public float DeshForce => deshForce;
    public float MaxDownSpeed => maxDownSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Создаем машину состояний
        stateMachine = new StateMachine();
        
        // Добавляем состояния
        stateMachine.AddState(new PlayerOnFlyState(this));
        stateMachine.AddState(new PlayerOnGroundState(this));
        
        
        // Начальное состояние
        stateMachine.ChangeState<PlayerOnGroundState>();
    }
    
    void Update()
    {
        stateMachine.Update();
        
        // Проверка земли
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.1f);
    }
    
    public void Move(Vector3 direction, float speed)
    {
        Vector3 movement = direction * (speed * Time.deltaTime);
        transform.position += movement;
        
    }
    
    public void Jump()
    {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
    }

    public void LeftJump()
    {
            rb.velocity = new Vector2(-1 * JumpForce, JumpForce);
    }

    public void RightJump()
    {
        rb.velocity = new Vector2(1 * JumpForce, JumpForce);
    }
    public void DownJump()
    {
        rb.velocity = new Vector2(0, -1 * JumpForce);
    }

    public void RightDesh()
    {
        rb.velocity = new Vector2(1 * DeshForce, 0);
    }
    public void LeftDesh()
    {
        rb.velocity = new Vector2(-1 * DeshForce, 0);
    }

    public void BrakePlayerFall()
    {
        if (rb.velocity.y < 0)
        {
            if (rb.velocity.magnitude >= maxDownSpeed)
            {
                
                rb.velocity = rb.velocity.normalized * MaxDownSpeed;
            }
            
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        stateMachine.OnCollisionEnter2D(other);
    }

    public void ChangeState<T>() where T : IState
    {
        stateMachine.ChangeState<T>();
    }
    
    public bool IsInState<T>() where T : IState
    {
        return stateMachine.IsInState<T>();
    }
}