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
        // for (int i = boats.Count - 1; i >= 0; i--)
        // {
        //     StartCoroutine(BoingDelay(boats[i], d));
        //     d += 0.1f;
        // }
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
        for (int i = 0; i < 14; i++) // pos_s.Count = 14
        {
            pos_s[i].transform.localPosition += new Vector3(0, 0.1f * i, 0);
        }
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(pos_s[i].transform.localPosition, 1);
        }
    }

    // Fall&Land
    public void Fall()
    {
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(pos_sInit[i], 1);
        }
    }

    public void MakeCharacterHappy()
    {
        characterAnim.SetTrigger("happy");
    }

    public void Multiplier()
    {
        if (k < boats.Count - 1)
        {
            boats[k].transform.parent = null;
        }
        else if (k == boats.Count - 1)
        {
            boats[k].transform.parent = null;
            character.transform.parent = null;
            GetComponent<Rigidbody>().detectCollisions = false;

            Observers.Instance.Notify_LevelEndObservers();
        }
        else
        { Debug.Log("bok"); }
        k++;
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
        Debug.Log("LOSER");
    }

    public void WinScenario()
    {
        Debug.Log("Path End.");
    }

    public void LevelEnd()
    {
        characterAnim.SetTrigger("walk");
        character.transform.DOMove(character.transform.position + new Vector3(0, 0.6f, 0), 0.2f);
        character.transform.DOMove(character.transform.position + new Vector3(0, 0.6f, 0) + character.transform.forward * 3, 0.8f);
        DOVirtual.DelayedCall(0.8f, CharacterTurnDelay);
    }

    void CharacterTurnDelay()
    {
        characterAnim.SetTrigger("turn");
    }
}