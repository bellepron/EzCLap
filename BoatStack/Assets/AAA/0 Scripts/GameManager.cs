using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IWinObserver, ILoseObserver
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

    // Level System
    int levelIndex;

    void Start()
    {
        levelIndex = PlayerPrefs.GetInt("level");

        levelTMP.enabled = true;
        startPanel.SetActive(true);
        succesPanel.SetActive(false);
        failPanel.SetActive(false);
        levelTMP.text = "LEVEL " + (levelIndex + 1).ToString();

        StartCoroutine(CheckSwipe());
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
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

    public void FailPanel()
    {
        SceneManager.LoadScene(0);
    }
    public void SuccessPanel()
    {
        SceneManager.LoadScene(0);
    }

    public void WinScenario()
    {
        succesPanel.SetActive(true);

        levelIndex++;
        PlayerPrefs.SetInt("level", levelIndex);
    }

    public void LoseScenario()
    {
        failPanel.SetActive(true);
    }
}
