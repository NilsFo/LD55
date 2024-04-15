using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SigilIndicator : MonoBehaviour
{
    private SummoningCircleBehaviourScript _summoningCircle;
    private GameState _gameState;

    public UnityEngine.UI.Image indicator;

    private Sigil _sigil;
    public UnityEngine.UI.Image sigilImg;

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
        var v = new Vector2(indicator.transform.localPosition.x, indicator.transform.localPosition.y) / 20f;
        if(Sigil.GetIndex(v) == Sigil.GetIndex(_gameState.currentLevelSigil)) {
            sigilImg.color = Color.Lerp(sigilImg.color, highlightColor, 4f * Time.deltaTime);
        } else {
            sigilImg.color = Color.Lerp(sigilImg.color, defaultColor, 4f * Time.deltaTime);
        }
    }
}
