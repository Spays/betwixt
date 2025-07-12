using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinCheck : MonoBehaviour
{
    public static event Action BattleCheck;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ПРОВЕРКА!!");
        BattleCheck();
    }
}
