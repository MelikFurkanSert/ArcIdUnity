using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class workDesk : MonoBehaviour
{
    public Animator female_anim;
    [SerializeField] private Transform DollarPlace,DollarsParent;
    [SerializeField] private GameObject Dollar;
    private float YAxis = -3.8f;
    private IEnumerator makeMoneyIE;
    [SerializeField] private Vector3 dollarScale;
    // Start is called before the first frame update
    void Start()
    {
        makeMoneyIE = MakeMoney();
    }

    public void Work()
    {
        female_anim.SetBool("work", true);

        InvokeRepeating("DOSubmitPapers", 2f, 1f);

        StartCoroutine(makeMoneyIE);
    }
    private IEnumerator MakeMoney()
    {
        var counter = 0;
        var DollarPlaceIndex = 0;
        
        yield return new WaitForSecondsRealtime(2);

        while (counter < transform.childCount)
        {
            if(DollarsParent.childCount == 0)
            {
                YAxis = -3.8f;
                DollarPlaceIndex = 0;
            }
            GameObject NewDollar = Instantiate(Dollar, new Vector3(DollarPlace.GetChild(DollarPlaceIndex).position.x,
                    YAxis, DollarPlace.GetChild(DollarPlaceIndex).position.z),
                DollarPlace.GetChild(DollarPlaceIndex).rotation);

            NewDollar.transform.DOScale(new Vector3(dollarScale.x, dollarScale.y, dollarScale.z), 0.5f).SetEase(Ease.OutElastic);
            NewDollar.transform.parent = DollarsParent;
 
            if (DollarPlaceIndex < DollarPlace.childCount - 1)
            {
                DollarPlaceIndex++;
            }
            else
            {
                DollarPlaceIndex = 0;
                YAxis += 0.25f;
            }

            yield return new WaitForSecondsRealtime(3f);
        }
    }

    void DOSubmitPapers()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject, 1f);
        }
        else
        {
            female_anim.SetBool("work", false);

            var Desk = transform.parent;

            Desk.GetChild(Desk.childCount - 2).gameObject.SetActive(true);

            StopCoroutine(makeMoneyIE);

          //  YAxis = 0f;
        }
    }
}
