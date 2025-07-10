using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MaiMenu : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ExitButton;
    // Start is called before the first frame update
    void Start()
    {
        ExitButton.onClick.AddListener(ExitGame);
        StartButton.onClick.AddListener(MainMenuPressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MainMenuPressed()
    {
        GameManager.Instance.LoadScene("GameScene");
    }

    void ExitGame()
    {
        Application.Quit();
    }
    
}
