using UnityEngine;

//_______________________________________
//состояние на земле


public class PlayerOnGroundState : IState
{
    private PlayerController player;
    private float jumpTimer = 0f;

    public PlayerOnGroundState(PlayerController player)
    {
        this.player = player;
    }
    public void Execute()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
            player.ChangeState<PlayerOnFlyState>();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            player.LeftJump();
            player.ChangeState<PlayerOnFlyState>();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            player.RightJump();
            player.ChangeState<PlayerOnFlyState>();
        }
    }

    public void Enter()
    {
        
        Debug.Log("Игрок на земле");
        
    }

    public void Exit()
    {
        // Ничего не делаем при выходе
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}

public class PlayerOnFlyState : IState
{
    private PlayerController player;
    private float jumpTimer = 0f;
    private bool JumpFlag;

    public PlayerOnFlyState(PlayerController player)
    {
        this.player = player;
    }

    public void Execute()
    {
        jumpTimer += Time.deltaTime;

        // Движение в воздухе

        if (Input.GetKeyDown(KeyCode.Space) && JumpFlag)
        {
            player.Jump();
            JumpFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && JumpFlag)
        {
            player.LeftJump();
            JumpFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && JumpFlag)
        {
            player.RightJump();
            JumpFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            player.LeftDesh();
            Debug.Log("Левый дэш");
            //JumpFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            player.RightDesh();
            Debug.Log("Правый дэш");
            //JumpFlag = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.DownJump();
            Debug.Log("Прыжок вниз");
            //JumpFlag = false;
        }
        //зависание

        //меню телепортации
        player.BrakePlayerFall();
        


        // Получаем ввод
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical ).normalized;

        // Движение
        player.Move(direction, player.WalkSpeed);

    }

    public void Enter()
    {
        Debug.Log("Игрок в полёте");
        JumpFlag = true;
        player.Jump();
        jumpTimer = 0f;
        
        
    }

    public void Exit()
    {
        // Ничего не делаем при выходе
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts) {
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f) {
                player.ChangeState<PlayerOnGroundState>();
                break;
            }
        }
        
    }
}