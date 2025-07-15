using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   
    public GameObject UICanvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!UICanvas.activeSelf)
            {
                UICanvas.SetActive(true);
                GameManager.Instance.PauseGame();
            }
            else
            {
                UICanvas.SetActive(false);
                GameManager.Instance.ResumeGame();
            }
        }
    }
}
