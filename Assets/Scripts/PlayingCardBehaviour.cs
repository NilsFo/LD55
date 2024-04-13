using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayingCardBehaviour : MonoBehaviour
{
    [Header("World Hookup")] public PlayingCardData playingCardDataBase;
    public PlayingCardData playingCardDataFallback;
    public Vector3 handWorldPos;
    private GameState _gameState;
    
    [Header("Gameplay Modifiers")]
    public bool isBurned;
    public bool isBloodSoaked;
    
    [Header("Parameters")] public float movementSpeed = 5;

    [Header("Readonly")] public bool inTransition;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }
    
    // Power
    public Vector2 CurrentPower => GetPower();

    // Start is called before the first frame update
    void Start()
    {
        if (playingCardDataBase == null)
        {
            playingCardDataBase = playingCardDataFallback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, handWorldPos, Time.deltaTime * movementSpeed);

        inTransition = !(Vector3.Distance(transform.position, handWorldPos) <= 0.01f);
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse down on card: " + name);

        if (_gameState.playingState==GameState.PlayingState.Default)
        {
            _gameState.DragCard(this);
        }
    }

    private Vector2 GetPower()
    {
        return playingCardDataBase.power;
    }

    private void OnMouseUp()
    {
    }
}