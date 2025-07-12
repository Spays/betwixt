using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Singleton<PlayerStats>
{
    public static PlayerStats Instance;
    
    public int playerHP = 100;
    public int playerKillPower = 100;
    public int playerEssention = 0;
    public int playerFlowerTeleportation = 0;
    [HideInInspector]public int playerCristalTeleportation = 0;
    [HideInInspector]public int playerCloudTeleportation = 0;
    [HideInInspector]public int playerMaxEssention = 3;
    public int essenceDoorstep = 10;
    public int WinCount = 100;

    public int teleportTax;
    
    public GameObject[] teleportPoints;
    
    private void OnEnable()
    {
        WinCheck.BattleCheck += WinUpdate;

    }

    public void OnDisable()
    {
        WinCheck.BattleCheck -= WinUpdate;
    }
    
    public void WinUpdate()
    {
        if (playerEssention < WinCount)
        {
            SceneManager.LoadScene(0);
            Debug.Log("YouLoose!!!!");
        }
        else
        {
            Debug.Log("YouWin!!!!");
        }
    }
    
    void Awake()
    {
        /*
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Чтобы не уничтожался при переключении сцен
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }
}
