using UnityEngine;
using DG.Tweening;

public class ContinuousRotation : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float rotationSpeed = 30f; // Градусов в секунду
    [SerializeField] private RotateMode mode = RotateMode.FastBeyond360;
    [SerializeField] private Ease easeType = Ease.Linear;

    void Start()
    {
        StartRotation();
    }

    void StartRotation()
    {
        // Рассчитываем длительность для полного оборота
        float duration = 360f / rotationSpeed;
        
        transform.DORotate(
            new Vector3(0, 0, 360), 
            duration, 
            mode
        )
        .SetEase(easeType)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative();
    }
}