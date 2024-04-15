using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Flamme : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOShakeRotation(5f, strength: 20, vibrato: 5, fadeOut:false).SetLoops(-1).Play();
        transform.DOShakeScale(5f, strength: 0.1f,  vibrato: 5, fadeOut: false).SetLoops(-1).Play();
    }

}
