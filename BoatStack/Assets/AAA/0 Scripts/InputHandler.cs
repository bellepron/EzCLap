using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    [Header("Player Info")]
    public GameObject player;
    // public Animator anim;
    // public Rigidbody rb;

    [Header("Control")]
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    bool pressed = false;

    Transform playerHolderT;
    public float speed = 20;
    [SerializeField] float bound_Angle = 15f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerHolderT = GameObject.FindWithTag("Player Holder").transform;
    }

    private void Update()
    {
        Control();
    }

    // public IEnumerator MyUpdate()
    // {
    //     while (true)
    //     {
    //         Control();

    //         transform.position = player.transform.position;

    //         yield return null;
    //     }
    // }

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
        else
        {

        }
    }

    void AngleChange()
    {
        float angle = playerHolderT.localEulerAngles.z;
        if (angle > 270)
            angle = angle - 360;

        if (secondPressPos.x - firstPressPos.x > 0)
            if (angle < bound_Angle)
                playerHolderT.localEulerAngles += new Vector3(0, 0, 90 * Time.deltaTime);

        if (secondPressPos.x - firstPressPos.x < 0)
            if (angle > -bound_Angle)
                playerHolderT.localEulerAngles += new Vector3(0, 0, -90 * Time.deltaTime);
    }

    #endregion
}