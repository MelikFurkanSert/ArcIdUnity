using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScaleYoyo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
