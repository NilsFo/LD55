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

    public List<GameObject> attachedObjects;

    public Light mainLight;
    private float _mainLightInte;

    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
        originalPos = transform.position;
        _mainLightInte = mainLight.intensity;

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
        // float TOLERANCE = 0.01f;
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
        moveTween.OnComplete(() => { OnArriveShow(); });
        moveTween.Play();
        mainLight.DOIntensity(0, .5f).Play();
    }

    public void OnArriveShow()
    {
        foreach (var attachedObject in attachedObjects)
        {
            attachedObject.SetActive(true);
            attachedObject.SetActive(true);
        }
    }

    public void OnHideShow()
    {
        foreach (var attachedObject in attachedObjects)
        {
            attachedObject.SetActive(false);
            attachedObject.SetActive(false);
        }
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
        moveTween.OnComplete(() => { OnHideShow(); });
        moveTween.Play();

        mainLight.DOIntensity(_mainLightInte, .5f).Play();
    }
}