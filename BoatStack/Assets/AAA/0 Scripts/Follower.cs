using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
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

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}