using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PlayerSaver : Singleton<PlayerSaver>
{
    public static PlayerStats Instance;
    
    public int playerHP = 100;
    public int playerKillPower = 100;
    public int playerEssention = 0;
    public int playerFlowerTeleportation = 0;
    public int playerCristalTeleportation = 0;
    public int playerCloudTeleportation = 0;
    public int playerMaxEssention = 3;
    public int WinCount = 100;
    
    
    private void OnEnable()
    {
        WinCheck.BattleCheck += WinUpdate;

    }

    public void OnDisable()
    {
        WinCheck.BattleCheck -= WinUpdate;
    }
    void Awake()
    {
        if (Instance == null)
        {
            //Instance = this;
            DontDestroyOnLoad(gameObject); // Чтобы не уничтожался при переключении сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinUpdate()
    {
        if (PlayerStats.Instance.playerEssention < WinCount)
        {
            SceneManager.LoadScene(SceneManager.sceneCount);
        }
        else
        {
            Debug.Log("YouWin!!!!");
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
