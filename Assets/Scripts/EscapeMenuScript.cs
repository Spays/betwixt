using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuScript : MonoBehaviour
{
    public void OnClickRestart()
    {
        GameManager.Instance.RestartLevel();
    }

    public void OnClickExit()
    {
        GameManager.Instance.ExitGame();
    }
    
}
