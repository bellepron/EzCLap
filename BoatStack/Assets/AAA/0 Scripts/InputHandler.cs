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
    public float speed = 20;
    [SerializeField] float bound_X = 1.5f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {

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

            // Vector3 direction = player.transform.forward + new Vector3(currentSwipe.x, 0f, 0f);

            // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);
            // player.transform.rotation = newRot;

            // anim.SetBool("isWalking", true);
            // player.transform.position += player.transform.forward * speed * Time.deltaTime;


            // if (secondPressPos.x - firstPressPos.x > 0)
            //     if (player.transform.localPosition.x <= bound_X)
            //         player.transform.position += player.transform.right * speed * Time.deltaTime;

            // if (secondPressPos.x - firstPressPos.x < 0)
            //     if (player.transform.localPosition.x >= -bound_X)
            //         player.transform.position -= player.transform.right * speed * Time.deltaTime;

            if (secondPressPos.x - firstPressPos.x > 0)
                if (player.transform.parent.transform.localEulerAngles.z < 90 + 30)
                    player.transform.parent.transform.localEulerAngles += new Vector3(0, 0, 90 * Time.deltaTime);

            if (secondPressPos.x - firstPressPos.x < 0)
                if (player.transform.parent.transform.localEulerAngles.z > 90 - 30)
                    player.transform.parent.transform.localEulerAngles -= new Vector3(0, 0, 90 * Time.deltaTime);
        }
        else
        {
            // anim.SetBool("isWalking", false);
        }
    }

    #endregion
}