using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    
    public int playerHP = 100;
    public int playerKillPower = 100;
    public int playerEssention = 0;
    public int playerFlowerTeleportation = 0;
    public int playerCristalTeleportation = 0;
    public int playerCloudTeleportation = 0;
    public int playerMaxEssention = 3;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Чтобы не уничтожался при переключении сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
