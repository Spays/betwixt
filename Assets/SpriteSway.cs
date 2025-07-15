using UnityEngine;
using DG.Tweening;

public class SpriteSway : MonoBehaviour
{
    [Header("Параметры покачивания")]
    public float swayAngle = 10f;         // максимальный угол поворота
    public float swayDuration = 1.5f;     // сколько длится одно движение в одну сторону
    public Ease swayEase = Ease.InOutSine; // плавность движения

    private void Start()
    {
        // качаем объект туда-сюда по z-оси
        transform
            .DOLocalRotate(new Vector3(0, 0, swayAngle), swayDuration)
            .SetEase(swayEase)
            .SetLoops(-1, LoopType.Yoyo);
    }
}