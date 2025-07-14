using System;
using DG.Tweening;
using UnityEngine;

namespace PlayerScripts
{
    public class NightmareController : MonoBehaviour
    {
        private SpriteRenderer[] _renderers;
        
        public Color FadedColor;
        public float targetAlpha;
        [SerializeField] private float duration = 10f; // Продолжительность анимации в секундах
        
        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void Fade()
        {
            foreach (var renderer in _renderers)
            {
                renderer.DOFade(1, duration)
                    .SetEase(Ease.InOutSine);
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
            
            foreach (var renderer in _renderers)
            {
                renderer.color = FadedColor;
            }
        }
    }
}