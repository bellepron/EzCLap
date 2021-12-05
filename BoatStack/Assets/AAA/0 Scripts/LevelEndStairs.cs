using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndStairs : MonoBehaviour, ILevelEndObserver
{
    [SerializeField] Transform firstStairTr;
    [SerializeField] GameObject stairPrefab;
    [SerializeField] int stepQuantity;
    [SerializeField] Transform lastStep;
    [SerializeField] ParticleSystem confetti0, confetti1, confetti2, confetti3;

    void Start()
    {
        Observers.Instance.Add_LevelEndObserver(this);

        GameObject step;
        for (int i = 1; i < stepQuantity; i++)
        {
            step = Instantiate(stairPrefab, firstStairTr.position + new Vector3(3 * i, 0.4f * i, 0), Quaternion.identity, transform);
            step.GetComponentInChildren<TextMeshProUGUI>().text = "X" + (i + 1).ToString();

            step.GetComponent<MeshRenderer>().material.color = new Color32((byte)(Random.Range(0, 255)), (byte)(Random.Range(0, 255)), (byte)(Random.Range(0, 255)), 255);
        }

        lastStep.transform.position = firstStairTr.position + new Vector3(3 * stepQuantity, 0.4f * stepQuantity, 0);
    }

    public void LevelEnd()
    {
        Invoke("PlayConfetties", 1.6f);
    }
    void PlayConfetties()
    {
        confetti0.Play();
        confetti1.Play();
        confetti2.Play();
        confetti3.Play();
    }
}