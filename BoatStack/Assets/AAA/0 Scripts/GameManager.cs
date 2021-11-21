using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelTMP;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject succesPanel;
    [SerializeField] GameObject failPanel;

    // Swipe Control
    Vector2 firstPos;
    Vector2 secondPos;
    Vector2 currentSwipe;
    bool isSwiped;


    void Start()
    {
        levelTMP.enabled = true;
        startPanel.SetActive(true);
        succesPanel.SetActive(false);
        failPanel.SetActive(false);

        StartCoroutine(CheckSwipe());
    }

    public void StartPanel()
    {
        isSwiped = true;
        startPanel.SetActive(false);
        Observers.Instance.Notify_LevelStartObservers();
    }
    IEnumerator CheckSwipe()
    {
        while (isSwiped == false)
        {
            Swipe();
            yield return null;
        }
    }

    void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = (Vector2)Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPos = (Vector2)Input.mousePosition;
            if (Vector2.Distance(firstPos, secondPos) > 2)
            {
                StartPanel();
            }
        }
    }
}
