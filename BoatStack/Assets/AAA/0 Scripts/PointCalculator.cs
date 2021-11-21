using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PointCalculator : MonoBehaviour
{
    public static PointCalculator Instance;

    float score;
    [SerializeField] TextMeshProUGUI scoreTMP;
    float multiplier;
    [SerializeField] TextMeshProUGUI multiplierTMP;

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
    }

    public void AddDiamond()
    {
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
}