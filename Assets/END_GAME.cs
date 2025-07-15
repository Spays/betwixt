using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class END_GAME : MonoBehaviour
{
    public GameObject DUMAITE;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Увеличиваем масштаб в 100 раз относительно текущего за 5 секунд
        // Если начальный масштаб (1,1,1), то станет (100,100,100)
        Vector3 targetScale = transform.localScale * 100f;
        transform.DOScale(targetScale, 5f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                DUMAITE.SetActive(true);
            }); // Ease для плавности (опционально)
        
        GetComponent<Collider2D>().enabled = false;
    }

    void OnTriggerEnter()
    {
        
    }
}
