using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour, ILevelStartObserver, IWinObserver, ILoseObserver
{
    public static Follower Instance;

    public PathCreator pathCreator;
    public float speed = 0;
    float distanceTravelled;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Observers.Instance.Add_LevelStartObserver(this);
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
    }

    public void LevelStart()
    {
        StartCoroutine(MyUpdate());
    }
    IEnumerator MyUpdate()
    {
        while (true)
        {
            Follow();

            yield return null;
        }
    }

    void Follow()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    public void WinScenario()
    {
        Stop();
    }

    public void LoseScenario()
    {
        Stop();
    }

    void Stop()
    {
        speed = 0;
        Observers.Instance.Remove_LevelStartObserver(this);
        Observers.Instance.Remove_WinObserver(this);
        Observers.Instance.Remove_LoseObserver(this);
        this.enabled = false;
    }
}