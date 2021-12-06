using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, ILoseObserver, IWinObserver, ILevelEndObserver
{
    public static Player Instance;

    [SerializeField] List<GameObject> boats;
    [SerializeField] List<Transform> pos_s;
    List<Vector3> pos_sInit = new List<Vector3>();

    public GameObject character;
    [HideInInspector] public Animator characterAnim;

    int k;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);

        boats.Add(InputHandler.Instance.player.transform.GetChild(0).gameObject);
        boats[0].transform.localPosition = pos_s[0].localPosition;

        for (int i = 0; i < pos_s.Count; i++)
        {
            pos_sInit.Add(pos_s[i].localPosition);
        }

        characterAnim = character.GetComponent<Animator>();
    }

    void Update()
    {
        // Rotation();
        // SetPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() == null) return;
        if (other.GetComponent<IInteractable>() != null)
        {
            other.GetComponent<IInteractable>().Interact();
        }
    }

    // void SetPosition()
    // {
    //     boats[boats.Count - 1].transform.localPosition = transform.position;
    //     for (int i = 0; i < boats.Count; i++)
    //     {
    //         boats[i].transform.localPosition = Vector3.MoveTowards(boats[i].transform.localPosition, pos_s[boats.Count - 1 - i].localPosition, 3 * Time.deltaTime);
    //     }
    // }
    Transform playerHolderT;
    void Rotation()
    {
        playerHolderT = GameObject.FindWithTag("Player Holder").transform;
        boats[0].transform.LookAt(transform.parent.transform.position);
        boats[0].transform.localEulerAngles += new Vector3(90, -180, 0);
        for (int i = 1; i < pos_s.Count; i++)
        {
            pos_s[i].transform.parent.transform.localEulerAngles = playerHolderT.transform.localEulerAngles;
        }
    }

    // Boat
    public void Boat(GameObject go)
    {
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(pos_s[i + 1].localPosition, 0.3f);
            boats[i].transform.parent = pos_s[i + 1].transform;
        }

        SoundManager.Instance.BoatStack();

        boats.Insert(0, go);
        BoingEffect();

        go.GetComponent<BoxCollider>().isTrigger = false;
        go.transform.parent = InputHandler.Instance.player.transform;
        go.transform.localEulerAngles = boats[1].transform.localEulerAngles;
        go.transform.DOLocalMove(pos_s[0].localPosition, 0.3f);
        go.GetComponent<BoxCollider>().enabled = false;
    }
    void BoingEffect()
    {
        float d = 0.0f;

        for (int i = 0; i < boats.Count; i++)
        {
            StartCoroutine(BoingDelay(boats[i], d));
            d += 0.05f;
        }
    }
    IEnumerator BoingDelay(GameObject boat, float delay)
    {
        yield return new WaitForSeconds(delay);
        boat.transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(() => boat.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce));
    }

    #region Obstacle Interact
    public void Obstacle(int value)
    {
        while (value > 0)
        {
            BoatExplode(boats[0], BoatChildCount());

            Sequence seq = DOTween.Sequence();
            for (int i = 1; i < boats.Count; i++)
            {
                seq.Join(boats[i].transform.DOLocalMove(pos_s[i - 1].localPosition, 0.3f));
                boats[i].transform.parent = pos_s[i - 1].transform;
            }
            boats.RemoveAt(0);

            if (value > 1)
                seq.Kill();

            value--;

            if (boats.Count <= 0)
            {
                // value = 0;
                // seq.Kill();
                Lose();
                return;
            }
        }
    }
    int BoatChildCount()
    {
        if (boats.Count > 1)
            return boats[0].transform.childCount;
        else
            return boats[0].transform.childCount - 1;
    }

    void BoatExplode(GameObject go, int childCount)
    {
        SoundManager.Instance.BoatExplosion();

        for (int i = 0; i < childCount; i++)
        {
            GameObject part = go.transform.GetChild(i).gameObject;
            Rigidbody rb = part.AddComponent<Rigidbody>();
            part.AddComponent<BoxCollider>();

            rb.AddExplosionForce(15, go.transform.position - new Vector3(0, 2f, 0), 3, 1, ForceMode.Impulse);
        }
        go.transform.parent = null;
    }

    #endregion


    // Jump
    public void Jump()
    {
        SoundManager.Instance.Jump();

        for (int i = 0; i < pos_s.Count; i++)
        {
            pos_s[i].transform.localPosition += new Vector3(0, 0.1f * i, 0);
        }
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(pos_s[i].transform.localPosition, 0.2f);
        }
    }

    // Fall&Land
    public void Fall()
    {
        SoundManager.Instance.Land();

        for (int i = 0; i < pos_s.Count; i++)
        {
            pos_s[i].transform.localPosition = pos_sInit[i];
        }
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(Vector3.zero, 0.3f);
        }
    }

    public void MakeCharacterHappy()
    {
        characterAnim.SetTrigger("happy");
    }

    public void Multiplier()
    {
        PointCalculator.Instance.MultiplierText();

        if (k < boats.Count - 1)
        {
            Leave(boats[k]);

            SoundManager.Instance.BoatDrop();
        }
        else if (k == boats.Count - 1)
        {
            Leave(boats[k]);

            character.transform.parent = null;
            character.transform.eulerAngles = new Vector3(0, 90, 0);
            GetComponent<Rigidbody>().detectCollisions = false;
            DOVirtual.DelayedCall(0.1f, StayOnGround);

            SoundManager.Instance.BoatExplosion();

            Observers.Instance.Notify_LevelEndObservers();
        }
        k++;
    }
    void Leave(GameObject boatsK)
    {
        boatsK.transform.parent = null;
        float posY = (PointCalculator.Instance.multiplier - 2) * 0.4f + 0.5f + FindObjectOfType<LevelEndStairs>().transform.position.y;
        float posX = (PointCalculator.Instance.multiplier - 2) * 3 + 0.43f + FindObjectOfType<LevelEndStairs>().transform.position.x;
        boatsK.transform.position = new Vector3(posX, posY, boatsK.transform.position.z);
        boatsK.transform.eulerAngles = new Vector3(0, boatsK.transform.eulerAngles.y, 0);
    }
    void StayOnGround()
    {
        Rigidbody charRb = character.GetComponent<Rigidbody>();
        charRb.useGravity = true;
        charRb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        character.GetComponent<CapsuleCollider>().isTrigger = false;
    }

    public void LastStep()
    {
        InputHandler.Instance.LevelEnd();

        StartCoroutine(WinWithExtraBoat());
    }
    IEnumerator WinWithExtraBoat()
    {
        bool a = true;
        float repeatTime = 0.2f;
        CameraManager.Instance.Cam2_Cam3();
        while (a)
        {
            if (k < boats.Count)
            {
                PointCalculator.Instance.MultiplierText();
                BoatExplode(boats[k], BoatChildCount());

                for (int i = k; i < boats.Count; i++)
                {
                    // boats[i].transform.DOLocalMove(pos_s[i - 1].localPosition + new Vector3(0, -1.5f, 0), 0.1f);
                    boats[i].transform.DOMove(boats[i].transform.position + new Vector3(0, -0.4f, 0), 0.2f);
                }

                // if (k < boats.Count - 1)
                //     seq.Kill();


                if (k == boats.Count - 1)
                {
                    boats[k].transform.parent = null;
                    character.transform.parent = null;
                    character.transform.eulerAngles = new Vector3(0, 90, 0);
                    GetComponent<Rigidbody>().detectCollisions = false;
                    DOVirtual.DelayedCall(0.1f, StayOnGround);

                    Observers.Instance.Notify_LevelEndObservers();

                    a = false;
                }
                k++;
            }

            yield return new WaitForSeconds(repeatTime);
        }
    }

    // Win
    public void PathEnd()
    {
        Observers.Instance.Notify_WinObservers();
    }

    // Lose
    public void Lose()
    {
        Observers.Instance.Notify_LoseObservers();
    }

    public void LoseScenario()
    {
        character.GetComponent<RagdollToggle>().RagdollActivate(true);
        SoundManager.Instance.Pain();
        Debug.Log("LOSER");
    }

    public void WinScenario()
    {

    }

    public void LevelEnd()
    {
        characterAnim.SetTrigger("walk");
        character.transform.DOMove(character.transform.position + new Vector3(0, 0.6f, 0), 0.2f);
        character.transform.DOMove(character.transform.position + new Vector3(0, 0.6f, 0) + new Vector3(1, 0, 0) * 3, 0.8f);
        DOVirtual.DelayedCall(0.8f, CharacterTurnDelay);
    }

    void CharacterTurnDelay()
    {
        characterAnim.SetTrigger("turn");
    }
}