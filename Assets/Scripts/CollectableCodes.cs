using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCodes : MonoBehaviour
{
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
        {
            Vector3 targetPos = MoneyUI.instance.GetIconPosition(transform.position);

            if (Vector2.Distance(transform.position, targetPos) > 0.75f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 8f);
            }
            else
            {
                MoneyUI.instance.AddCount(5);
                MoneyUI.instance.ScaleElastic();
                Destroy(gameObject);
            }
        }
    }

    public void SetCollected()
    {
        collected = true;
    }
}
