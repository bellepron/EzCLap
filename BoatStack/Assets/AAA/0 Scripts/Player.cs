using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, ILoseObserver, IWinObserver
{
    public static Player Instance;

    [SerializeField] List<GameObject> simits;
    [SerializeField] List<Transform> pos_s;
    List<Vector3> pos_sInit = new List<Vector3>();

    public GameObject stickman;
    [SerializeField] GameObject stickmanSpine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_WinObserver(this);

        stickman = InputHandler.Instance.player.transform.GetChild(0).GetChild(0).gameObject;

        simits.Add(InputHandler.Instance.player.transform.GetChild(0).gameObject);
        simits[0].transform.localPosition = pos_s[0].localPosition;

        for (int i = 0; i < pos_s.Count; i++)
        {
            pos_sInit.Add(pos_s[i].localPosition);
        }

        if (stickmanSpine != null)
        {
            stickmanSpine.AddComponent<CharacterBalance>();
        }
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

    void SetPosition()
    {
        simits[simits.Count - 1].transform.localPosition = transform.position;
        for (int i = 0; i < simits.Count; i++)
        {
            simits[i].transform.localPosition = Vector3.MoveTowards(simits[i].transform.localPosition, pos_s[simits.Count - 1 - i].localPosition, 3 * Time.deltaTime);
        }
    }

    void Rotation()
    {
        // simits[simits.Count - 1].transform.LookAt(transform.parent.transform.position + new Vector3(0, 5, 0));
        for (int i = 0; i < simits.Count; i++)
        {
            simits[i].transform.LookAt(transform.parent.transform.position + new Vector3(0, 5 + simits.Count - i, 0));

        }
    }

    // Simit
    public void Simit(GameObject go)
    {
        for (int i = 0; i < simits.Count; i++)
        {
            simits[i].transform.DOLocalMove(pos_s[i + 1].localPosition, 0.3f);
        }

        simits.Insert(0, go);

        go.GetComponent<BoxCollider>().isTrigger = false;
        go.transform.parent = InputHandler.Instance.player.transform;
        go.transform.DOLocalMove(pos_s[0].localPosition, 0.3f);
        go.GetComponent<BoxCollider>().enabled = false;
    }

    // Obstacle
    public void Obstacle(int value)
    {
        if (simits.Count - value <= 0)
        {
            Lose();

            return;
        }

        while (value > 0)
        {
            simits[0].transform.parent = null;
            simits[0].transform.position = simits[1].transform.localPosition + new Vector3(0, -1, 0); // What a pity!
            Sequence seq = DOTween.Sequence();
            for (int i = 1; i < simits.Count; i++)
            {
                seq.Join(simits[i].transform.DOLocalMove(pos_s[i - 1].localPosition, 0.3f));
            }
            // simits[0].GetComponent<Rigidbody>().isKinematic = true;
            simits.RemoveAt(0);

            if (value > 1)
                seq.Kill();

            value--;
        }
    }

    // Jump
    public void Jump()
    {
        for (int i = 0; i < pos_s.Count; i++)
        {
            pos_s[i].transform.localPosition += new Vector3(0, 0.1f * i, 0);
        }
        for (int i = 0; i < simits.Count; i++)
        {
            simits[i].transform.DOLocalMove(pos_s[i].transform.localPosition, 1);
        }
    }

    // Fall&Land
    public void Fall()
    {
        for (int i = 0; i < simits.Count; i++)
        {
            simits[i].transform.DOLocalMove(pos_sInit[i], 1);
        }
    }

    // Win
    public void Win()
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
        // stickman ragdoll
        Debug.Log("LOSER");

        // for (int i = 1; i < simits.Count; i++)
        // {
        //     simits[i].transform.parent = null;
        //     simits[i].GetComponent<Rigidbody>().useGravity = true;
        //     simits[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3, 3), Random.Range(1, 3), Random.Range(-1, 1)) * 15, ForceMode.Impulse);
        // }
        // simits[0].GetComponent<Rigidbody>().isKinematic = true;
        // simits.RemoveAt(0);

    }

    public void WinScenario()
    {
        Debug.Log("WINNER");
    }
}