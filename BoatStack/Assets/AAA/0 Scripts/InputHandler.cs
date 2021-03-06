using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InputHandler : MonoBehaviour, ILevelStartObserver, IWinObserver, ILoseObserver, ILevelEndObserver
{
    public static InputHandler Instance;

    [Header("Player Info")]
    public GameObject player;

    [Header("Control")]
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    bool pressed = false;

    Transform playerHolderT;
    [SerializeField] float bound_Angle = 15f;
    public float swipeSpeed;

    bool isUpdating = true;
    bool goForward = true;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Observers.Instance.Add_LevelStartObserver(this);
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);

        playerHolderT = GameObject.FindWithTag("Player Holder").transform;
    }

    public void LevelStart()
    {
        StartCoroutine(MyUpdate());
    }

    public IEnumerator MyUpdate()
    {
        yield return null;

        while (isUpdating)
        {
            Control();

            yield return null;
        }
    }

    #region Control

    public void Control()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            firstPressPos = (Vector2)Input.mousePosition;
            pressed = false;
        }

        if (pressed == true)
        {
            secondPressPos = (Vector2)Input.mousePosition;
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, 0);
            currentSwipe.Normalize();

            AngleChange();
        }
    }

    void AngleChange()
    {
        float angle = playerHolderT.localEulerAngles.z;
        if (angle > 270)
            angle = angle - 360;

        if (secondPressPos.x - firstPressPos.x > 0)
            if (angle < bound_Angle)
            {
                playerHolderT.localEulerAngles += new Vector3(0, 0, swipeSpeed * 9f * Time.deltaTime);
            }

        if (secondPressPos.x - firstPressPos.x < 0)
            if (angle > -bound_Angle)
            {
                playerHolderT.localEulerAngles += new Vector3(0, 0, swipeSpeed * -9f * Time.deltaTime);
            }
    }

    #endregion

    void AngleReset()
    {
        playerHolderT.DOLocalRotate(Vector3.zero, 1, RotateMode.Fast);
    }

    public void WinScenario()
    {
        AngleReset();
        Stop();
        StartCoroutine(GoForward());
    }
    IEnumerator GoForward()
    {
        while (goForward)
        {
            // player.transform.position += new Vector3(player.transform.forward.x, 0, player.transform.forward.z) * 10 * Time.deltaTime;
            player.transform.position += Vector3.right * 10 * Time.deltaTime;
            yield return null;
        }
    }

    public void LoseScenario()
    {
        Stop();
    }

    public void LevelEnd()
    {
        goForward = false;
    }

    void Stop()
    {
        isUpdating = false;
    }
}