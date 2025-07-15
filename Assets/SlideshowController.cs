using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SlideshowController : MonoBehaviour
{
    [Header("Настройки слайд-шоу")]
    public GameObject[] slides; // Перетащите сюда слайды из иерархии
    public float slideDuration = 4f; // Время показа слайда
    public float fadeDuration = 1f; // Длительность перехода
    
    private int currentSlideIndex = 0;
    private bool isPlaying = true;
    private CanvasGroup[] canvasGroups;

    void Start()
    {
        controls = new PlayerInputActions();
        controls.Player.Enable(); // Включаем Action Map "Player"
        
        // Проверка наличия слайдов
        if (slides == null || slides.Length == 0)
        {
            Debug.LogError("Слайды не назначены!");
            return;
        }

        // Инициализация CanvasGroups
        canvasGroups = new CanvasGroup[slides.Length];
        for (int i = 0; i < slides.Length; i++)
        {
            if (slides[i] != null)
            {
                // Гарантируем наличие CanvasGroup
                canvasGroups[i] = slides[i].GetComponent<CanvasGroup>();
                if (canvasGroups[i] == null)
                {
                    canvasGroups[i] = slides[i].AddComponent<CanvasGroup>();
                }
                
                // Начальное состояние: только первый слайд видим
                canvasGroups[i].alpha = (i == 0) ? 1 : 0;
                slides[i].SetActive(true);
            }
        }

        // Запуск автоматической смены слайдов
        InvokeRepeating(nameof(NextSlide), slideDuration, slideDuration);
    }

    private PlayerInputActions controls;
    void Update()
    {
        // Пропуск по клику
        if (isPlaying && controls.Player.Jump.WasReleasedThisFrame())
        {
            SkipToNextSlide();
        }
    }

    void NextSlide()
    {
        if (!isPlaying) return;
        
        // Переход к следующему слайду
        currentSlideIndex++;
        
        if (currentSlideIndex >= slides.Length)
        {
            EndIntro();
            return;
        }

        // Запуск анимации перехода
        FadeTransition(currentSlideIndex - 1, currentSlideIndex);
    }

    void FadeTransition(int outIndex, int inIndex)
    {
        // Анимация исчезновения текущего слайда
        canvasGroups[outIndex].DOFade(0, fadeDuration)
            .OnComplete(() => slides[outIndex].SetActive(false));
        
        // Анимация появления нового слайда
        slides[inIndex].SetActive(true);
        canvasGroups[inIndex].DOFade(1, fadeDuration);
    }

    void SkipToNextSlide()
    {
        if (!isPlaying) return;
        
        // Отмена текущего таймера
        CancelInvoke(nameof(NextSlide));
        
        if (currentSlideIndex >= slides.Length - 1)
        {
            EndIntro();
        }
        else
        {
            // Быстрый переход к следующему слайду
            FadeTransition(currentSlideIndex, currentSlideIndex + 1);
            currentSlideIndex++;
            
            // Перезапуск таймера
            InvokeRepeating(nameof(NextSlide), slideDuration, slideDuration);
        }
    }

    void EndIntro()
    {
        isPlaying = false;
        CancelInvoke(); // Остановка всех вызовов
        
        canvasGroups[currentSlideIndex].DOFade(0, fadeDuration);
        
        cameraController.StartGame();
    }
    
    public CameraController cameraController;

    bool SceneExists(string sceneName)
    {
        // Проверка существования сцены в билде
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}