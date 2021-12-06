using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Dreamteck.Splines;

public class GameManager : MonoBehaviour, IWinObserver, ILoseObserver, ILevelEndObserver
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
    [SerializeField] LevelDefinition[] levels;
    int levelIndex;
    [SerializeField] SplineFollower splineFollower;

    #region Game Start Functions
    void Start()
    {
        LevelOperations();
        PreparingPanels();
        AddObserver();

        StartCoroutine(CheckSwipe());
    }
    void LevelOperations()
    {
        PlayerPrefs.SetInt("level", 0);
        levelIndex = PlayerPrefs.GetInt("level");
        levelTMP.enabled = true;
        levelTMP.text = "LEVEL " + (levelIndex + 1).ToString();

        SettingLevelInfo();
    }
    void SettingLevelInfo()
    {
        int currentIndex = levelIndex % levels.Length;
        GameObject _level = Instantiate(levels[currentIndex].level, Vector3.zero, Quaternion.identity);
        splineFollower.spline = _level.transform.GetComponentInChildren<SplineComputer>();
        Player.Instance.speed = levels[currentIndex].speed;
        InputHandler.Instance.swipeSpeed = levels[currentIndex].swipeSpeed;

        LevelEndStairs stairs = _level.transform.GetComponentInChildren<LevelEndStairs>();
        stairs.stepCount = levels[currentIndex].stairsStepCount;
        stairs.ManuelStart();
    }
    void PreparingPanels()
    {
        startPanel.SetActive(true);
        succesPanel.SetActive(false);
        failPanel.SetActive(false);
    }
    void AddObserver()
    {
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);
    }

    #endregion

    #region Level Start Funtions
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

    public void StartPanel()
    {
        isSwiped = true;
        startPanel.SetActive(false);
        Observers.Instance.Notify_LevelStartObservers();
    }
    #endregion

    #region Buttons

    public void FailPanel()
    {
        SceneManager.LoadScene(0);
    }
    public void SuccessPanel()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    public void WinScenario()
    {

    }

    public void LoseScenario()
    {
        failPanel.SetActive(true);
    }

    public void LevelEnd()
    {
        levelIndex++;
        PlayerPrefs.SetInt("level", levelIndex);
        Invoke("SuccesPanelActivate", 1);
    }
    void SuccesPanelActivate()
    {
        succesPanel.SetActive(true);
    }
}
