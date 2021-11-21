using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private static GameMusic music = null;

    void Awake()
    {
        if (music == null)
        {
            music = this;
            DontDestroyOnLoad(this);
        }
        else if (this != music)
        {
            Destroy(gameObject);
        }
    }
}