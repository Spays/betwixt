using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uima : MonoBehaviour
{
    public Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartLevel();
        });
    }
}
