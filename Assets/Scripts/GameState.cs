using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
        MainMenu,
        Playing,
        Paused,
        EndOfRound,
        Calculating,
        Summoning,
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
    public MusicManager musicManager;
    public TutorialBook tutorialBook;

    [Header("UI Hookup")] public GameObject mainMenuPL;
    public GameObject helpPL;
    public GameObject endScreenPL;

    [Header("UI Elements")] public Slider volumeSlider;
    public GameObject inGameBookPages;

    [Header("States")] public PlayingState playingState = PlayingState.Default;
    private PlayingState _playingState;
    public LevelState levelState = LevelState.Playing;
    private LevelState _levelState;
    public bool demonCaptureCorrect;
    public PlayingCardBehaviour draggingCard;
    private float _draggingDoubleClickTimer = 0;
    public bool AllowDropping => _draggingDoubleClickTimer <= 0;

    [Header("Current Level")] public Vector2 currentLevelSigil;
    public int levelCurrent = 0;
    public int levelMax = 6;
    public int demonCreationCount = 1;
    public int score = 0;
    public int highScore = 0;
    public int demomCreationCount = 0;

    [Header("Gameplay config")] public Vector3 selectedCardOffset;
    public float daemonCardPowerMod = 1 / 10f;

    [Header("Listeners")] public UnityEvent onRoundEnd;
    public UnityEvent onRoundStart;
    public UnityEvent onRoundCalculation;

    [Header("Gameplay Rules")] public int handSize = 5;
    public int drawsRemaining;
    public bool firstTimePlaying = true;
    public bool allowCardPickUp = true;
    [Range(0, 1)] public float cardFoilChance = 0.1f;
    [Range(0, 2)] public float cardFoilMult = 1.25f;
    [Range(0, 2)] public float cardBurningMult = 1.25f;
    [Range(0, 1)] public float specialCardDrawChance = 0.1f;
    [Range(0, 10)] public int specialCardMax = 2;

    [Header("Audio")] public List<AudioClip> cardSounds;

    private void Awake()
    {
        playingState = PlayingState.Default;
        levelState = LevelState.MainMenu;
        _playingState = playingState;
        _levelState = levelState;

        volumeSlider.value = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelCurrent = 1;
        volumeSlider.value = MusicManager.userDesiredMasterVolume;

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

        ResetMetrics();
    }

    public void StartGame()
    {
        ResetMetrics();
        OnRoundBegin();
    }

    // Update is called once per frame
    void Update()
    {
        drawsRemaining = Mathf.Max(drawsRemaining, 0);
        _draggingDoubleClickTimer -= Time.deltaTime;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastResults = Physics.RaycastAll(ray);

        // Updating music
        musicManager.Play(0);
        MusicManager.userDesiredMasterVolume = volumeSlider.value;

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

                if (Input.GetMouseButtonUp(0) && AllowDropping &&
                    levelState == LevelState.Playing
                   )
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

                if (targetedCard != null && Input.GetMouseButtonDown(0) &&
                    (levelState == LevelState.Playing || levelState == LevelState.Summoning)
                   )
                {
                    _draggingDoubleClickTimer = 0.1f;
                    targetedCard.OnClick();
                }

                break;
        }

        switch (levelState)
        {
            case LevelState.GameOver:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                inGameBookPages.SetActive(false);
                endScreenPL.SetActive(true);
                break;
            case LevelState.MainMenu:
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                mainMenuPL.SetActive(true);
                inGameBookPages.SetActive(false);
                break;
            case LevelState.Unknown:
                Debug.LogError("Unknown level state");
                return;
            case LevelState.Playing:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                inGameBookPages.SetActive(true);
                Time.timeScale = 1;
                break;
            case LevelState.Paused:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                inGameBookPages.SetActive(true);
                Time.timeScale = 0;
                break;
            case LevelState.Calculating:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                inGameBookPages.SetActive(true);
                break;
            case LevelState.Summoning:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                inGameBookPages.SetActive(true);
                break;
            case LevelState.EndOfRound:
                mainMenuPL.SetActive(false);
                helpPL.SetActive(false);
                endScreenPL.SetActive(false);
                inGameBookPages.SetActive(true);
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
                tutorialBook.Hide();
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
                Invoke(nameof(RequestFirstTimeTutorial),5);
                //RequestFirstTimeTutorial();
                break;
            case LevelState.Paused:
                break;
            case LevelState.Calculating:
                onRoundCalculation?.Invoke();
                demonCaptureCorrect = IsSigilMatching();
                tutorialBook.Hide();
                break;
            case LevelState.EndOfRound:
                demonCaptureCorrect = false;
                tutorialBook.Hide();
                OnRoundEnd();
                break;
            case LevelState.GameOver:
                tutorialBook.Hide();
                OnGameEnd();
                break;
            case LevelState.MainMenu:
                tutorialBook.Hide();
                break;
            case LevelState.Unknown:
                break;
            case LevelState.Summoning:
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

    private void RequestFirstTimeTutorial()
    {
        if (firstTimePlaying)
        {
            RequestTutorial();
        }
    }

    public void RequestTutorial()
    {
        if (playingState == PlayingState.Default && levelState == LevelState.Playing)
        {
            tutorialBook.Show();
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

    public void RequestEndRound()
    {
        EndRound();
    }

    private void EndRound()
    {
        levelState = LevelState.Calculating;
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

        // Cleanup
        levelCurrent++;
        if (levelCurrent >= levelMax)
        {
            highScore = (int)MathF.Max(highScore, score);
            levelState = LevelState.GameOver;
        }
        else
        {
            levelState = LevelState.Playing;
            OnRoundStart();
            print("Level: " + levelCurrent + "/" + levelMax);
        }
    }

    public bool IsSigilMatching()
    {
        int resultRuneTotalIndex = Sigil.GetIndex(summoningCircle.resultRuneTotal);
        int currentLevelSigilIndex = Sigil.GetIndex(currentLevelSigil);
        print("result rune: " + resultRuneTotalIndex + ". current level: " + currentLevelSigilIndex);

        return resultRuneTotalIndex == currentLevelSigilIndex;
    }

    public void CreateDemonCards()
    {
        PlayingCardBehaviour[] cards = FindObjectsOfType<PlayingCardBehaviour>();
        foreach (PlayingCardBehaviour card in cards)
        {
            card.OnRoundEnd();
        }

        float offset = 0;
        for (var i = 0; i < demonCreationCount; i++)
        {
            handGameObject.CreateDaemonCard(offset);
            offset += 0.3f;
        }
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
        demonCreationCount = 1;
        demonCaptureCorrect = false;

        // Init new Round
        drawsRemaining = handSize - handGameObject.CardsInHandCount;
        levelState = LevelState.Playing;

        onRoundStart.Invoke();
    }

    public void RequestStartNewGame()
    {
        print("Request new game.");
        StartNewGame();
    }

    private void StartNewGame()
    {
        levelState = LevelState.Playing;
        OnRoundStart();
        ResetMetrics();
    }

    public void ResetMetrics()
    {
        var cards = FindObjectsOfType<PlayingCardBehaviour>();
        foreach (var playingCardBehaviour in cards)
        {
            playingCardBehaviour.DestroyCard();
        }

        demonCaptureCorrect = false;
        score = 0;
        levelCurrent = 1;
        demomCreationCount = 0;
    }

    [ContextMenu("Back to menu")]
    public void BackToMenu()
    {
        firstTimePlaying = false;
        OnRoundEnd();

        var cards = FindObjectsOfType<PlayingCardBehaviour>();
        foreach (var playingCardBehaviour in cards)
        {
            playingCardBehaviour.DestroyCard();
        }

        levelState = LevelState.MainMenu;
    }

    public void OnGameEnd()
    {
        highScore = (int)MathF.Max(highScore, score);
    }

    public void PlayCardSound()
    {
        int i = UnityEngine.Random.Range(0, cardSounds.Count);
        musicManager.CreateAudioClip(cardSounds[i], camera.transform.position, respectBinning: false);
    }
}