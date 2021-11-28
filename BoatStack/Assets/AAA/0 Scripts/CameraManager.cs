using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour, IWinObserver, ILevelEndObserver
{
    public static CameraManager Instance;

    [SerializeField] CinemachineVirtualCamera v_Cam1;
    [SerializeField] CinemachineVirtualCamera v_Cam2;
    [SerializeField] CinemachineVirtualCamera v_Cam3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);

        v_Cam1.Priority = 10;
        v_Cam2.Priority = 0;
        v_Cam3.Priority = 0;
    }

    public void Cam1_Cam2()
    {
        v_Cam1.Priority = 0;
        v_Cam2.Priority = 10;
    }
    public void Cam2_Cam3()
    {
        v_Cam2.Priority = 0;
        v_Cam3.Priority = 10;
    }

    public void WinScenario()
    {
        Cam1_Cam2();
    }

    public void LevelEnd()
    {
        Cam2_Cam3();
    }
}