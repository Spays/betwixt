using System.Collections.Generic;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public GameObject[] CameraTpPoints;
    private int _currentLevel;

    [Header("Nightmare")]
    public float timeBeforeNightmare = 5f;
    [FormerlySerializedAs("nightmareController")] [SerializeField] private NightmareBackground nightmareBackground;

    public Transform[] platformParents;
    
    private void OnEnable()
    {
        StartTimer();
        
        PlayerOnFlyStateNew.OnTeleported += CameraTeleporting;
    }

    private void OnDisable()
    {
        PlayerOnFlyStateNew.OnTeleported -= CameraTeleporting;
    }

    private void CameraTeleporting()
    {
        if (_currentLevel < CameraTpPoints.Length-1)
        {
            transform.position = CameraTpPoints[_currentLevel].transform.position;
            _currentLevel++;

            StartTimer();
        }
        else
        {
            transform.position = CameraTpPoints[_currentLevel].transform.position;
            _currentLevel = 0;

            StartTimer();
        }
    }

    public void StartTimer()
    {
        timer = 0f;
        triggered = false;
        
        nightmareBackground.Reset();
    }
    
    
    private float timer = 0f;
    private bool triggered = false;
    // public UnityEvent OnNightmareStart;

    void Update()
    {
        timer += Time.deltaTime;

        if (triggered == false && timer >= timeBeforeNightmare)
        {
            triggered = true;
            Debug.Log("Nightmare started");
            // OnNightmareStart.Invoke(); // подключаем в инспекторе анимацию/эффекты/бой
            // enabled = false; // выключаем, чтобы не срабатывало снова
            nightmareBackground.Fade();

            var platfromChangers = platformParents[_currentLevel].GetComponentsInChildren<PlatfromChanger>();
            
            foreach (var platfromChanger in platfromChangers)
            {
                platfromChanger.Show();
            }
            
            var removwers = platformParents[_currentLevel].GetComponentsInChildren<NightmarePlatformRemove>();
            
            foreach (var platfromChanger in removwers)
            {
                platfromChanger.Remove();
            }
        }
    }
}
