using DG.Tweening;
using UnityEngine;

namespace PlayerScripts
{
    public class PlatfromChanger : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        public SpriteRenderer _parentRenderer;

        public Color FadedColor;
        [SerializeField] private float duration = 5f;
        
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = FadedColor;
        }

        public void Show()
        {
            _renderer.DOFade(1, duration)
                .SetEase(Ease.InOutSine);
            
            _parentRenderer.DOFade(0, duration)
                .SetEase(Ease.InOutSine);
        }
    }
}