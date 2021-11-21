using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandMove : MonoBehaviour
{
    float firstPos;
    void Start()
    {
        firstPos = transform.position.x;
        Move();
    }
    void Move()
    {
        transform.position = new Vector2(firstPos, transform.position.y);
        transform.DOMoveX(transform.position.x + 500, 1.5f).OnComplete(() => Move());
    }
}