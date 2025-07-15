using UnityEngine;
using DG.Tweening;

public class RockingAnimation : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private float angle = 5f; // Угол покачивания
    [SerializeField] private float duration = 1.5f; // Длительность одного качания
    [SerializeField] private Ease easeType = Ease.InOutSine; // Тип плавности

    void Start()
    {
        StartRocking();
    }

    void StartRocking()
    {
        // Создаем последовательность
        Sequence rockingSequence = DOTween.Sequence();
        
        // Анимация вправо
        rockingSequence.Append(transform.DORotate(
            new Vector3(0, 0, angle), 
            duration, 
            RotateMode.Fast
        ).SetEase(easeType));
        
        // Анимация влево
        rockingSequence.Append(transform.DORotate(
            new Vector3(0, 0, -angle), 
            duration, 
            RotateMode.Fast
        ).SetEase(easeType));
        
        // Зацикливаем
        rockingSequence.SetLoops(-1, LoopType.Restart);
    }
}