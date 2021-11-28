using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndStairs : MonoBehaviour
{
    [SerializeField] Transform firstStairTr;
    [SerializeField] GameObject stairPrefab;
    [SerializeField] int stepQuantity;

    void Start()
    {
        for (int i = 1; i < stepQuantity; i++)
        {
            GameObject step = Instantiate(stairPrefab, firstStairTr.position + new Vector3(3 * i, 0.4f * i, 0), Quaternion.identity, transform);
            step.GetComponentInChildren<TextMeshProUGUI>().text = "X" + (i + 1).ToString();
        }
    }
}