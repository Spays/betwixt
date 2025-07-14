using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using DG.Tweening;

public class PulsatingScript : MonoBehaviour
{
    [Header("Pulsation Settings")] [Tooltip("Начальный масштаб объекта")]
    public Vector3 startScale = Vector3.one;

    [Tooltip("Максимальный масштаб при пульсации")]
    public Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Tooltip("Длительность одной пульсации (в секундах)")]
    public float pulseDuration = 0.8f;

    [Tooltip("Задержка между пульсациями")]
    public float pulseDelay = 0.2f;

    [Tooltip("Тип анимации")] public Ease easeType = Ease.InOutSine;

    [Tooltip("Запускать пульсацию автоматически при старте")]
    public bool playOnStart = true;

    private Sequence _pulseSequence;

    private void Start()
    {
        if (playOnStart)
        {
            StartPulsating();
        }
    }

    public void StartPulsating()
    {
        // Останавливаем предыдущую анимацию, если она была
        StopPulsating();

        // Устанавливаем начальный масштаб
        transform.localScale = startScale;

        // Создаем последовательность анимаций
        _pulseSequence = DOTween.Sequence();

        // Увеличение масштаба
        _pulseSequence.Append(transform.DOScale(endScale, pulseDuration / 2).SetEase(easeType));

        // Уменьшение масштаба
        _pulseSequence.Append(transform.DOScale(startScale, pulseDuration / 2).SetEase(easeType));

        // Задержка между пульсациями
        _pulseSequence.AppendInterval(pulseDelay);

        // Бесконечное повторение
        _pulseSequence.SetLoops(-1, LoopType.Restart);
    }

    public void StopPulsating()
    {
        if (_pulseSequence != null && _pulseSequence.IsActive())
        {
            _pulseSequence.Kill();
            _pulseSequence = null;

            // Возвращаем исходный масштаб
            transform.localScale = startScale;
        }
    }

    private void OnDestroy()
    {
        StopPulsating();
    }

    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, endScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, startScale);
    }
}
