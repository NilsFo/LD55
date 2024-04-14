using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigilIndicator : MonoBehaviour
{
    private SummoningCircleBehaviourScript _summoningCircle;
    private GameState _gameState;

    public RectTransform indicator;

    private Sigil _sigil;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _summoningCircle = FindObjectOfType<SummoningCircleBehaviourScript>();
        _gameState = FindObjectOfType<GameState>();
        _summoningCircle.onRuneChangeEvent.AddListener(UpdateSigilIndicator);
        _sigil = GetComponentInChildren<Sigil>();
        _gameState.onRoundStart.AddListener(UpdateSigilIndicator);
    }

    void UpdateSigilIndicator()
    {
        Vector2 v = _summoningCircle.resultRuneTotal;
        v = v.normalized * 20f;
        indicator.transform.localPosition = new Vector3(v.x, v.y, 0);

        _sigil.dir = _gameState.currentLevelSigil;
        _sigil.UpdateSigilSprite();
    }
}
