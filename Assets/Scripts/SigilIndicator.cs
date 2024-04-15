using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SigilIndicator : MonoBehaviour
{
    private SummoningCircleBehaviourScript _summoningCircle;
    private GameState _gameState;

    public Image indicator;

    private Sigil _sigil;

    private Vector2 sigilTarget;

    private float _colorLerpVal;
    public Color highlightColor, defaultColor;
    
    
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
        sigilTarget = _summoningCircle.resultRuneTotal;
        sigilTarget = sigilTarget.normalized * 20f;

        _sigil.dir = _gameState.currentLevelSigil;
        _sigil.UpdateSigilSprite();
        _colorLerpVal = 0;
        DOTween.To(() => _colorLerpVal, (val) => _colorLerpVal = val, 1, .4f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
    }

    void Update() {
        indicator.transform.localPosition = Vector2.Lerp(indicator.transform.localPosition, new Vector3(sigilTarget.x, sigilTarget.y, 0), 2f * Time.deltaTime);
        indicator.color = Color.Lerp(defaultColor, highlightColor, _colorLerpVal);
    }
}
