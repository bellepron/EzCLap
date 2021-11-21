using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBalance : MonoBehaviour
{
    GameObject playerHolder;
    void Start()
    {
        playerHolder = GameObject.FindWithTag("Player Holder");
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, -1 * (playerHolder.transform.eulerAngles.z - 270));
    }
}