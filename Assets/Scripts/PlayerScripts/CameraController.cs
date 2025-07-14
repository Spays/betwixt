using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public GameObject[] CameraTpPoints;
    private int _currentLevel;

    [Header("Nightmare")]
    public float timeBeforeNightmare = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer
    public Color FadedColor;
    public float targetAlpha;
    [SerializeField] private float duration = 10f; // Продолжительность анимации в секундах
    
    private void OnEnable()
    {
        StartTimer();
        
        PlayerOnFlyStateNew.OnTeleported += CameraTeleporting;
    }

    private void OnDisable()
    {
        PlayerOnFlyStateNew.OnTeleported -= CameraTeleporting;
    }

    private void CameraTeleporting()
    {
        if (_currentLevel < CameraTpPoints.Length-1)
        {
            transform.position = CameraTpPoints[_currentLevel].transform.position;
            _currentLevel++;

            StartTimer();
        }
        else
        {
            transform.position = CameraTpPoints[_currentLevel].transform.position;
            _currentLevel = 0;

            StartTimer();
        }
    }

    public void StartTimer()
    {
        timer = 0f;
        triggered = false;
        
        spriteRenderer.color = FadedColor;
    }
    
    
    private float timer = 0f;
    private bool triggered = false;
    // public UnityEvent OnNightmareStart;

    void Update()
    {
        timer += Time.deltaTime;

        if (triggered == false && timer >= timeBeforeNightmare)
        {
            triggered = true;
            Debug.Log("Nightmare started");
            // OnNightmareStart.Invoke(); // подключаем в инспекторе анимацию/эффекты/бой
            // enabled = false; // выключаем, чтобы не срабатывало снова
            Fade();
        }
    }
    
    // Метод для изменения прозрачности
    public void Fade()
    {
        if (spriteRenderer != null)
        {
            // Используем DOFade для твининга альфа-канала
            spriteRenderer.DOFade(targetAlpha, duration)
                .SetEase(Ease.InOutSine) // Опционально: тип easing (плавность анимации)
                .OnComplete(() => Debug.Log("Анимация прозрачности завершена!")); // Опционально: коллбек по завершению
        }
        else
        {
            Debug.LogError("SpriteRenderer не найден!");
        }
    }
}
