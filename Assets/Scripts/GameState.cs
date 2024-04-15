using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

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
        EndOfRound,
        Calculating,
        GameOver
    }

    [Header("World Hookup")] public Camera camera;
    public PlayingCardHand handGameObject;
    public PlayingCardDeck deckGameObject;
    public SummoningCircleBehaviourScript summoningCircle;
    public bool mouseSelectHasTarget;
    public SelectorTarget mouseSelectTargetObj;
    public Vector3 mouseSelectTargetPos;
    public Vector3 mouseCardPlaneTargetPos;

    [Header("States")] public PlayingState playingState = PlayingState.Default;
    private PlayingState _playingState;
    public LevelState levelState = LevelState.Playing;
    private LevelState _levelState;
    public PlayingCardBehaviour draggingCard;
    private float _draggingDoubleClickTimer = 0;
    public bool AllowDropping => _draggingDoubleClickTimer <= 0;

    [Header("Current Level")] public Vector2 currentLevelSigil;
    public int levelCurrent = 0;
    public int levelMax = 6;
    public bool giveDoubleDaemon = false;
    public int score = 0;

    [Header("Gameplay config")] public Vector3 selectedCardOffset;
    public float daemonCardPowerMod = 1 / 10f;

    [Header("Listeners")] public UnityEvent onRoundEnd;
    public UnityEvent onRoundStart;
    public UnityEvent onRoundCalculation;

    [Header("Gameplay Rules")] public int handSize = 5;
    public int drawsRemaining;
    public bool allowCardPickUp = true;
    [Range(0, 1)] public float cardFoilChance = 0.1f;
    [Range(0, 2)] public float cardFoilMult = 1.25f;
    [Range(0, 2)] public float cardBurningMult = 1.25f;
    [Range(0, 1)] public float specialCardDrawChance = 0.1f;
    [Range(0, 10)] public int specialCardMax = 2;

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
        levelCurrent = 1;
        if (onRoundEnd == null)
        {
            onRoundEnd = new UnityEvent();
        }

        if (onRoundStart == null)
        {
            onRoundStart = new UnityEvent();
        }

        if (onRoundCalculation == null)
        {
            onRoundCalculation = new UnityEvent();
        }

        OnRoundBegin();
    }

    // Update is called once per frame
    void Update()
    {
        drawsRemaining = Mathf.Max(drawsRemaining, 0);
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

        mouseSelectHasTarget = false;
        mouseSelectTargetObj = null;
        mouseCardPlaneTargetPos = Vector3.zero;
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
                    CardHoverPlane hoverTarget = raycastHit.transform.gameObject.GetComponent<CardHoverPlane>();
                    if (selectable != null && selectable.active && selectable.enabled)
                    {
                        mouseSelectHasTarget = true;
                        mouseSelectTargetObj = selectable;
                        mouseSelectTargetPos = selectable.cardPlacement.position;
                    }

                    if (hoverTarget != null)
                    {
                        // print("hover hit");
                        mouseCardPlaneTargetPos = raycastHit.point;
                    }
                }

                if (Input.GetMouseButtonUp(0) && AllowDropping)
                {
                    if (mouseSelectTargetObj != null)
                    {
                        draggingCard.PlaceOnTable();
                    }
                    else
                    {
                        draggingCard.ReturnToHand();
                    }
                }

                break;
            case PlayingState.Default:
                draggingCard = null;

                PlayingCardBehaviour targetedCard = null;
                foreach (RaycastHit raycastHit in raycastResults)
                {
                    PlayingCardBehaviour cardBehaviour =
                        raycastHit.transform.gameObject.GetComponent<PlayingCardBehaviour>();
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

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(mouseSelectTargetPos, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mouseCardPlaneTargetPos, 0.1f);
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
            default:
                playingState = PlayingState.Unknown;
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
            case LevelState.Calculating:
                onRoundCalculation?.Invoke();
                break;
            case LevelState.EndOfRound:
                OnRoundEnd();
                break;
            default:
                levelState = LevelState.Unknown;
                break;
        }

        if (_levelState == LevelState.EndOfRound)
        {
            OnRoundBegin();
        }
    }

    public void DragCard(PlayingCardBehaviour playingCardBehaviour)
    {
        playingState = PlayingState.CardDrag;
        draggingCard = playingCardBehaviour;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }

    private void EndRound()
    {
        levelState = LevelState.EndOfRound;
    }

    public void StartCalculateScores()
    {
        Debug.Log("Calculating scores...");
        levelState = LevelState.Calculating;
    }

    private void OnRoundBegin()
    {
        OnRoundStart();
    }

    private void OnRoundEnd()
    {
        // Listeners
        onRoundEnd.Invoke();
        handGameObject.OnEndOfRound();

        handGameObject.CreateDaemonCard();

        // Cleanup
        PlayingCardBehaviour[] cards = FindObjectsOfType<PlayingCardBehaviour>();
        foreach (PlayingCardBehaviour card in cards)
        {
            card.OnRoundEnd();
        }

        levelCurrent++;
        levelState = LevelState.Playing;
        OnRoundStart();
        print("Level: " + levelCurrent + "/" + levelMax);
    }

    private void OnRoundStart()
    {
        Random r = new Random();
        int si = r.Next(8);
        Vector2[] svec =
        {
            new(1, 1),
            new(0, 1),
            new(-1, 1),
            new(-1, 0),
            new(-1, -1),
            new(0, -1),
            new(1, -1),
            new(1, 0),
        };
        currentLevelSigil = svec[si];

        // Init new Round
        drawsRemaining = handSize - handGameObject.CardsInHandCount;
        levelState = LevelState.Playing;

        onRoundStart.Invoke();
    }
}