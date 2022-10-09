using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuyArea : MonoBehaviour
{
    [SerializeField] private Image progressImage;
    [SerializeField] private GameObject workDesk;
    private float currentMoney,progress;
    [SerializeField] private float cost;
    [SerializeField] private TextMeshProUGUI MoneyPriceCounter;
    bool inactive;

    void Start()
    {
        MoneyPriceCounter.text ="0 / " + cost.ToString();
    }
    void Buy()
    {
        currentMoney += 1;
        progress = currentMoney / cost;
        progressImage.fillAmount = progress;
        MoneyPriceCounter.text = currentMoney.ToString() + " / " + cost.ToString();
        if(progress >= 1)
        {
            GameObject NewDesk = Instantiate(workDesk, new Vector3(transform.position.x, -2.75f, transform.position.z-3),
            Quaternion.Euler(new Vector3(0, 270, 0)));

            NewDesk.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 1f).SetEase(Ease.OutElastic);
            inactive = true;
            Destroy(gameObject);
        }
    }
    public IEnumerator BuyWithDelay()
    {
            while (1 <= MoneyUI.instance.money && !inactive)
            {
            
                Debug.Log("aa");
                Buy();
                MoneyUI.instance.AddCount(-1);
                yield return new WaitForSeconds(0.1f);
            
            }
        
    }
}
