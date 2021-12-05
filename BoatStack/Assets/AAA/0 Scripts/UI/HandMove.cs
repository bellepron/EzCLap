using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandMove : MonoBehaviour
{
    float firstPos;
    void Start()
    {
        transform.DOMoveX(transform.position.x + 400, 1f).SetLoops(-1);
    }
}