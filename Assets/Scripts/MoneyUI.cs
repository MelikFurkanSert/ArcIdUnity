using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI instance;

    public TextMeshProUGUI MoneyCounter;
    public Transform iconTransform;
    private Camera mainCamera;
    public int money;
    [SerializeField] private Vector3 MoneyPunchScale;
    private bool doOnce;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        mainCamera = Camera.main;
    }
    
    public void AddCount(int amount)
    {
        money += amount;
        MoneyCounter.text = "$"+ money.ToString();
    }
    public Vector3 GetIconPosition(Vector3 target)
    {
        Vector3 uiPos = iconTransform.position;
        uiPos.z = (target - mainCamera.transform.position).z;

        Vector3 result = mainCamera.ScreenToWorldPoint(uiPos);
        return result;
    }
    public void ScaleElastic()
    {
        if (doOnce == false)
        {
            doOnce= true;
            this.transform.DOPunchScale(new Vector3(MoneyPunchScale.x, MoneyPunchScale.y, MoneyPunchScale.z), 0.5f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
            {
                this.transform.localScale = new Vector3(2, 1, 1);
                doOnce = false;
            });
        }
    }
}
