using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public GameObject[] CameraTpPoints;
    public int StepFlag;

    private void OnEnable()
    {
        PlayerOnFlyState.OnTeleported += CameraTeleporting;
    }

    private void OnDisable()
    {
        PlayerOnFlyState.OnTeleported -= CameraTeleporting;
    }

    private void CameraTeleporting()
    {
        if (StepFlag < CameraTpPoints.Length-1)
        {
            transform.position = CameraTpPoints[StepFlag].transform.position;
            StepFlag++;
        }
        else
        {
            transform.position = CameraTpPoints[StepFlag].transform.position;
            StepFlag = 0;
        }
    }
}
