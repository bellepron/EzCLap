using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEndStairs : MonoBehaviour, ILevelEndObserver
{
    [SerializeField] Transform firstStairTr;
    [SerializeField] GameObject stairPrefab;
    public int stepCount;
    [SerializeField] Transform lastStep;
    [SerializeField] ParticleSystem confetti0, confetti1, confetti2, confetti3;

    public void ManuelStart()
    {
        Observers.Instance.Add_LevelEndObserver(this);

        GameObject step;
        for (int i = 1; i < stepCount; i++)
        {
            step = Instantiate(stairPrefab, firstStairTr.position + new Vector3(3 * i, 0.4f * i, 0), Quaternion.identity, transform);
            step.GetComponentInChildren<TextMeshProUGUI>().text = "X" + (i + 1).ToString();

            step.GetComponent<MeshRenderer>().material.color = new Color32((byte)(Random.Range(0, 255)), (byte)(Random.Range(0, 255)), (byte)(Random.Range(0, 255)), 255);
        }

        lastStep.transform.position = firstStairTr.position + new Vector3(3 * stepCount, 0.4f * stepCount, 0);
    }

    public void LevelEnd()
    {
        Invoke("PlayConfetties", 1.6f);
    }
    void PlayConfetties()
    {
        StartCoroutine(DelayConfetti());
    }
    IEnumerator DelayConfetti()
    {
        confetti0.Play();
        SoundManager.Instance.Confetti();
        yield return new WaitForSeconds(0.1f);
        confetti1.Play();
        SoundManager.Instance.Confetti();
        yield return new WaitForSeconds(0.1f);
        confetti2.Play();
        SoundManager.Instance.Confetti();
        yield return new WaitForSeconds(0.1f);
        confetti3.Play();
        SoundManager.Instance.Confetti();
    }
}