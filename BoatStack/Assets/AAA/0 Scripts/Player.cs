using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, ILoseObserver, IWinObserver
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
        Rotation();
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
        boats[boats.Count - 1].transform.localPosition = transform.position;
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.localPosition = Vector3.MoveTowards(boats[i].transform.localPosition, pos_s[boats.Count - 1 - i].localPosition, 3 * Time.deltaTime);
        }
    }

    void Rotation()
    {
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.LookAt(transform.parent.transform.position);
            boats[i].transform.localEulerAngles += new Vector3(90, -180, 0);
        }
    }

    // Boat
    public void Boat(GameObject go)
    {
        for (int i = 0; i < boats.Count; i++)
        {
            boats[i].transform.DOLocalMove(pos_s[i + 1].localPosition, 0.3f);
        }

        boats.Insert(0, go);

        go.GetComponent<BoxCollider>().isTrigger = false;
        go.transform.parent = InputHandler.Instance.player.transform;
        go.transform.DOLocalMove(pos_s[0].localPosition, 0.3f);
        go.GetComponent<BoxCollider>().enabled = false;
    }

    // Obstacle
    public void Obstacle(int value)
    {
        while (value > 0)
        {
            BoatExplode(boats[0]);

            Sequence seq = DOTween.Sequence();
            for (int i = 1; i < boats.Count; i++)
            {
                seq.Join(boats[i].transform.DOLocalMove(pos_s[i - 1].localPosition, 0.3f));
            }
            boats.RemoveAt(0);

            if (value > 1)
                seq.Kill();

            value--;

            if (boats.Count <= 0)
                Lose();
        }
    }

    void BoatExplode(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject part = go.transform.GetChild(i).gameObject;
            Rigidbody rb = part.AddComponent<Rigidbody>();
            part.AddComponent<BoxCollider>();

            // Vector3 direction = (part.transform.position - go.transform.position).normalized;
            rb.AddExplosionForce(15, go.transform.position, 5, 1, ForceMode.Impulse);
        }
        go.transform.parent = null;
    }

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
        k++;

        if (boats.Count - k > 0)
        {
            boats[boats.Count - k].transform.parent = null;
        }
        if (boats.Count - k == 0)
        {
            boats[0].transform.parent = null;
            boats[0] = character;
            GetComponent<Rigidbody>().detectCollisions = false;
            Follower.Instance.speed = 0;
            characterAnim.SetTrigger("walk");
            StartCoroutine(CharacterDelay());
        }
    }

    IEnumerator CharacterDelay()
    {
        yield return new WaitForSeconds(1);
        characterAnim.SetTrigger("turn");

        yield return new WaitForSeconds(1);
        Win();
    }

    // Win
    public void Win()
    {
        Observers.Instance.Notify_WinObservers();
    }

    // Lose
    public void Lose()
    {
        // character.TODO: ragdoll
        Observers.Instance.Notify_LoseObservers();
    }

    public void LoseScenario()
    {
        // stickman ragdoll
        Debug.Log("LOSER");
    }

    public void WinScenario()
    {
        Debug.Log("WINNER");
    }
}