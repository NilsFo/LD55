using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class TutorialBook : MonoBehaviour
{

    public float distX = 0;
    public float distY = 0;
    public float distZ = 0;
    private GameState _gameState;

    private Vector3 originalPos;

    private Vector3 hidePos;
    private TweenerCore<Vector3, Vector3, VectorOptions> _moveTween;

    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
        originalPos = transform.position;
        
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        // var pos = transform.localPosition;
        // pos.x = _desiredX;
        // transform.localPosition = pos;

        if (Input.GetMouseButtonDown(0))
        {
            RequestHide();
        }
    }

    public void RequestHide()
    {
        print("request hide");
        float TOLERANCE = 0.01f;
        // if (Math.Abs(transform.localPosition.x - showX) < TOLERANCE)
        // {
        //     Hide();
        // }
        Hide();
    }
    
    [ContextMenu("Show")]
    public void Show()
    {
        print("show");
        var pos = originalPos;
        pos.x += distX;
        pos.y += distY;
        pos.z += distZ;
        
        TweenerCore<Vector3, Vector3, VectorOptions> moveTween = transform.DOMove(pos, .5f)
            .SetEase(Ease.OutQuad);
        // moveTween.OnComplete(() => { cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.Drawing; });
        moveTween.Play();
    }
    
    [ContextMenu("Hide")]
    public void Hide()
    {
        print("hide");
        if (_moveTween != null)
        {
            _moveTween.Pause();
        }

        var pos = originalPos;
        pos.x += distX;
        pos.y += distY;
        pos.z += distZ;
        
        TweenerCore<Vector3, Vector3, VectorOptions> moveTween = transform.DOMove(originalPos, .5f)
            .SetEase(Ease.OutQuad);
        // moveTween.OnComplete(() => { cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.Drawing; });
        moveTween.Play();
    }
}