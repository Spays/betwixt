using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button StarGameButton;
    [SerializeField] private Button ExitButton;

    // Start is called before the first frame update
    void Start()
    {
        ExitButton.onClick.AddListener(ExitGame);
        StarGameButton.onClick.AddListener(StartGamePressed);
    }

    void StartGamePressed()
    {
        GameManager.Instance.LoadScene("Scenes/GameScene");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
