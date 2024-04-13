using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum PlayingState
    {
        Unknown,
        Default,
        CardDrag
    }

    public enum LevelState
    {
        Unknown,
        Playing,
        Paused,
        GameOver
    }

    [Header("World Hookup")] public Camera camera;
    public PlayingCardHand handGameObject;
    public PlayingCardDeck deckGameObject;
    public GameObject mouseSelectTargetObj;
    public Vector3 mouseSelectTargetPos;

    [Header("States")] public PlayingState playingState = PlayingState.Default;
    private PlayingState _playingState;
    public LevelState levelState = LevelState.Playing;
    private LevelState _levelState;
    public GameObject draggingCard;
    private float _draggingDoubleClickTimer = 0;
    public bool AllowDropping => _draggingDoubleClickTimer <= 0;

    [Header("Gameplay config")] public Vector3 selectedCardOffset;

    [Header("Game Rules")] public bool TEST;

    private void Awake()
    {
        playingState = PlayingState.Default;
        levelState = LevelState.Playing;
        _playingState = playingState;
        _levelState = levelState;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _draggingDoubleClickTimer -= Time.deltaTime;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastResults = Physics.RaycastAll(ray);
        
        if (_levelState != levelState)
        {
            OnNewLevelState();
            _levelState = levelState;
        }

        if (_playingState != playingState)
        {
            OnNewPlayingState();
            _playingState = playingState;
        }

        mouseSelectTargetObj = null;
        mouseSelectTargetPos = Vector3.zero;
        switch (playingState)
        {
            case PlayingState.Unknown:
                Debug.LogError("Unknown playing state");
                return;
            case PlayingState.CardDrag:
                foreach (RaycastHit raycastHit in raycastResults)
                {
                    SelectorTarget selectable = raycastHit.transform.gameObject.GetComponent<SelectorTarget>();
                    if (selectable != null)
                    {
                        mouseSelectTargetObj = selectable.gameObject;
                        mouseSelectTargetPos = raycastHit.point;
                    }
                }
                break;
            case PlayingState.Default:
                draggingCard = null;

                PlayingCardBehaviour targetedCard=null;
                foreach (RaycastHit raycastHit in raycastResults)
                {
                    PlayingCardBehaviour cardBehaviour = raycastHit.transform.gameObject.GetComponent<PlayingCardBehaviour>();
                    if (cardBehaviour != null)
                    {
                        targetedCard = cardBehaviour;
                    }
                }
                
                if (targetedCard != null && Input.GetMouseButtonDown(0))
                {
                    _draggingDoubleClickTimer = 0.1f;
                    targetedCard.OnClick();
                }
                break;
        }

        switch (levelState)
        {
            case LevelState.Unknown:
                Debug.LogError("Unknown level state");
                return;
            case LevelState.Playing:
                Time.timeScale = 1;
                break;
            case LevelState.Paused:
                Time.timeScale = 0;
                break;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Debug.DrawLine(Vector3.zero, GetMouseWorldPosition(), Color.green);
        }
    }

#endif

    private void OnNewPlayingState()
    {
        Debug.Log("New playing state: " + _playingState + " -> " + playingState);
        _draggingDoubleClickTimer = 0.1f;

        switch (playingState)
        {
            case PlayingState.CardDrag:
                break;
            case PlayingState.Default:
                break;
        }
    }

    private void OnNewLevelState()
    {
        Debug.Log("New playing state: " + _levelState + " -> " + levelState);

        switch (levelState)
        {
            case LevelState.Playing:
                break;
            case LevelState.Paused:
                break;
        }
    }

    public void DragCard(PlayingCardBehaviour playingCardBehaviour)
    {
        playingState = PlayingState.CardDrag;
        draggingCard = playingCardBehaviour.gameObject;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }
}