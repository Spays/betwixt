using UnityEngine;

//
public interface IPlayerState : IState
{
    void OnCollisionEnter2D(Collision2D collision);
}