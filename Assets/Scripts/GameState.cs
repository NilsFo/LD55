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

    [Header("States")] public PlayingState playingState = PlayingState.Default;
    private PlayingState _playingState;
    public LevelState levelState = LevelState.Playing;
    private LevelState _levelState;
    public GameObject draggingCard;

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


        switch (playingState)
        {
            case PlayingState.Unknown:
                Debug.LogError("Unknown playing state");
                return;
            case PlayingState.CardDrag:
                break;
            case PlayingState.Default:
                draggingCard = null;
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

    private void OnNewPlayingState()
    {
        Debug.Log("New playing state: " + _playingState + " -> " + playingState);

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
}