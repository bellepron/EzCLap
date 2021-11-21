using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour, ILevelStartObserver
{
    public static Follower Instance;

    public PathCreator pathCreator;
    public float speed = 0;
    float distanceTravelled;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        Observers.Instance.Add_LevelStartObserver(this);
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
}