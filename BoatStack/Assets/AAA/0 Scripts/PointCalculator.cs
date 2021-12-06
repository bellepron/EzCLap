using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PointCalculator : MonoBehaviour, ILevelEndObserver
{
    public static PointCalculator Instance;

    float score;
    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI multiplierTMP;
    int multiplier;
    [SerializeField] GameObject pointBar;
    [SerializeField] TextMeshProUGUI totalPointTMP;
    float totalPoint = 0;
    [SerializeField] Button nextLevelButton;

    [Header("Diamond Collect")]
    float diamondQuantity;
    [SerializeField] TextMeshProUGUI diamondTMP;
    [SerializeField] GameObject diamondImagePrefab;
    [SerializeField] Transform diamondInstT;
    [SerializeField] Transform canvasDiamondT;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        diamondTMP.text = diamondQuantity.ToString();
        multiplierTMP.text = "";

        Observers.Instance.Add_LevelEndObserver(this);
    }

    public void AddDiamond()
    {
        SoundManager.Instance.Diamond();

        GameObject diamond = Instantiate(diamondImagePrefab, diamondInstT.position, Quaternion.identity, canvas.transform) as GameObject;
        DiamondImageMove(diamond);
        StartCoroutine(IncreaseDiamond(diamond));
    }
    void DiamondImageMove(GameObject diamond)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(diamond.transform.DOMove(diamond.transform.position + new Vector3(0, 100, 0), 0.3f));
        seq.Append(diamond.transform.DOMove(canvasDiamondT.position, 1));
        seq.Join(diamond.transform.DOScale(Vector3.one * 1f, 1.3f));
    }
    IEnumerator IncreaseDiamond(GameObject diamond)
    {
        yield return new WaitForSeconds(1.3f);

        diamondQuantity += 10;
        diamondTMP.text = diamondQuantity.ToString();
        diamond.SetActive(false);
    }

    public void MultiplierText()
    {
        multiplier++;
        multiplierTMP.text = "X" + multiplier.ToString();
        multiplierTMP.transform.DOScale(Vector3.one * 1.3f, 0.2f);
    }

    public void LevelEnd()
    {
        Invoke("SuccessPanelDelay", 1);
    }
    void SuccessPanelDelay()
    {
        pointBar.transform.localScale = Vector3.one * 0.1f;
        pointBar.transform.DOScale(Vector3.one, 2);
        Invoke("NextLevelButtonActivate", 2);

        DOTween.To(() => totalPoint, x => totalPoint = x, diamondQuantity * multiplier, 2);
        StartCoroutine(UpdateTotalPointText());
    }
    void NextLevelButtonActivate()
    {
        nextLevelButton.interactable = true;
    }
    IEnumerator UpdateTotalPointText()
    {
        bool a = true;
        float t = 0;
        while (a)
        {
            totalPointTMP.text = ((int)totalPoint).ToString();

            t += Time.deltaTime;
            if (t > 2.1f)
            {
                a = false;
            }
            yield return null;
        }
    }
}