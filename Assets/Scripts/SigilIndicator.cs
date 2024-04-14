using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigilIndicator : MonoBehaviour
{
    private SummoningCircleBehaviourScript _summoningCircle;
    // Start is called before the first frame update
    void Start()
    {
        _summoningCircle = FindObjectOfType<SummoningCircleBehaviourScript>();
        _summoningCircle.onRuneChangeEvent.AddListener(UpdateSigilIndicator);
    }

    void UpdateSigilIndicator()
    {
        Vector2 v = _summoningCircle.resultRuneTotal;
        v = v.normalized * 20f;
        transform.localPosition = new Vector3(v.x, v.y, 0);
    }
}
