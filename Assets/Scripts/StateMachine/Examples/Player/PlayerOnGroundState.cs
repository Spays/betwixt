using UnityEngine;

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
        // else if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     player.LeftJump();
        //     player.ChangeState<PlayerOnFlyState>();
        // }
        // else if (Input.GetKeyDown(KeyCode.E))
        // {
        //     player.RightJump();
        //     player.ChangeState<PlayerOnFlyState>();
        // }
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