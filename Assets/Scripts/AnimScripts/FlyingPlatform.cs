using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class FlyingPlatform : MonoBehaviour
{

    [Header("Waypoints")] [Tooltip("Начальная точка (EmptyObject)")]
    public Transform startPoint;

    [Tooltip("Конечная точка (EmptyObject)")]
    public Transform endPoint;

    [Header("Movement Settings")] [Tooltip("Скорость движения платформы")]
    public float moveSpeed = 3f;

    [Tooltip("Задержка на каждой точке перед обратным движением")]
    public float delayAtWaypoints = 1f;

    [Tooltip("Тип движения: PingPong (туда-обратно) или Restart (возврат в начало)")]
    public LoopType loopType = LoopType.Yoyo;

    private Sequence _moveSequence;

    private void Start()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("Start and End points must be assigned!");
            enabled = false;
            return;
        }

        // Устанавливаем начальную позицию
        transform.position = startPoint.position;

        // Запускаем движение
        StartMovement();
    }

    private void StartMovement()
    {
        // Рассчитываем время движения на основе расстояния и скорости
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        float duration = distance / moveSpeed;

        // Создаем последовательность DOTween
        _moveSequence = DOTween.Sequence();

        _moveSequence.Append(transform.DOMove(endPoint.position, duration).SetEase(Ease.InOutQuad))
            .AppendInterval(delayAtWaypoints)
            .Append(transform.DOMove(startPoint.position, duration).SetEase(Ease.InOutQuad))
            .AppendInterval(delayAtWaypoints)
            .SetLoops(-1, loopType);
    }

    private void OnDestroy()
    {
        if (_moveSequence != null)
        {
            _moveSequence.Kill();
        }
    }

    // Визуализация пути в редакторе
    private void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
        Gizmos.DrawSphere(startPoint.position, 0.2f);
        Gizmos.DrawSphere(endPoint.position, 0.2f);
    }

}
