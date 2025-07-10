using UnityEngine;

// Интерфейс для всех состояний
public interface IState
{
    void Enter();    // Вызывается при входе в состояние
    void Execute();  // Вызывается каждый кадр
    void Exit();     // Вызывается при выходе из состояния
    
    void OnCollisionEnter2D(Collision2D collision);
}