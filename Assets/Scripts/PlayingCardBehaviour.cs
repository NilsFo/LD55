using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayingCardBehaviour : MonoBehaviour
{
    public enum PlayingCardState
    {
        DrawAnimation,
        Drawing,
        InHand,
        Selected,
        Played
    }

    [Header("World Hookup")] public PlayingCardData playingCardData;
    public Vector3 handWorldPos;
    private GameState _gameState;
    public Vector3 playedWorldPos;

    [Header("Card visual design")] public TMP_Text nameTF;
    public TMP_Text powerTF;
    public UnityEngine.UI.Image artworkImg;
    public Sigil sigil;

    [Header("Gameplay Modifiers")] public PlayingCardState playingCardState, _playingCardState;
    public bool isBurned;
    public bool isBloodSoaked;

    [Header("Parameters")] public float movementSpeed = 5;

    [Header("Readonly")] public bool inTransition;
    
    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playingCardState = playingCardState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playingCardState != playingCardState)
        {
            _playingCardState = playingCardState;
            Debug.Log("New card state: " + playingCardState);
        }

        switch (playingCardState)
        {
            case PlayingCardState.Drawing:
                transform.position =
                    Vector3.MoveTowards(transform.position, handWorldPos, Time.deltaTime * movementSpeed);
                inTransition = !(Vector3.Distance(transform.position, handWorldPos) <= 0.01f);
                break;
            case PlayingCardState.DrawAnimation:
                playingCardState = PlayingCardState.Drawing;
                break;
            case PlayingCardState.Played:
                transform.position =
                    Vector3.MoveTowards(transform.position, playedWorldPos, Time.deltaTime * movementSpeed);
                inTransition = !(Vector3.Distance(transform.position, playedWorldPos) <= 0.01f);
                break;
            case PlayingCardState.Selected:
                // Updating mouse pos if selected
                // Vector3 mousePos = _gameState.GetMouseWorldPosition();
                // Vector3 offset = _gameState.selectedCardOffset;
                // Vector3 pos = mousePos + offset;
                // transform.position = pos;
                break;
            case PlayingCardState.InHand:
                transform.position =
                    Vector3.MoveTowards(transform.position, handWorldPos, Time.deltaTime * movementSpeed);
                inTransition = !(Vector3.Distance(transform.position, handWorldPos) <= 0.01f);
                break;
            default:
                break;
        }

        // Draw placement arrow
        if (_gameState.playingState == GameState.PlayingState.CardDrag && playingCardState == PlayingCardState.Selected)
        {
            Debug.DrawLine(transform.position, _gameState.mouseSelectTargetPos, Color.red);

            // Placing the card
            if (Input.GetMouseButtonDown(0) && _gameState.AllowDropping)
            {
                PlaceOnTable();
            }
        }

        // Updating card art
        if (playingCardData != null)
        {
            nameTF.text = playingCardData.cardName;
            powerTF.text = playingCardData.PowerScala().ToString();
            artworkImg.sprite = playingCardData.sprite;
            sigil.dir = GetSigilDirection();
            sigil.UpdateSigilSprite();
        }
    }

    public void GetSigilSprite(Vector2 dir) {

    }

    public void OnClick()
    {
        print("card click");
        if (_gameState.playingState == GameState.PlayingState.Default)
        {
            // Placing card on board
            if (playingCardState == PlayingCardState.InHand)
            {
                DragCard();
            }

            // Returning card to hand
            // if (playingCardState == PlayingCardState.Played)
            // {
            // ReturnToHand();
            // }
        }
    }

    public void ReturnToHand()
    {
        Debug.Log("Returning to hand: " + name);
        playingCardState = PlayingCardState.InHand;
        _gameState.playingState = GameState.PlayingState.Default;

        if (!_gameState.handGameObject.cardsInHand.Contains(this))
        {
            _gameState.handGameObject.cardsInHand.Add(this);
        }
    }

    public void DragCard()
    {
        Debug.Log("card selected: " + name);
        _gameState.DragCard(this);
        playingCardState = PlayingCardState.Selected;
    }

    public void PlaceOnTable()
    {
        playedWorldPos = _gameState.mouseSelectTargetPos;
        _gameState.playingState = GameState.PlayingState.Default;
        playingCardState = PlayingCardState.Played;
        _gameState.handGameObject.cardsInHand.Remove(this);
    }

    public Vector2 GetSigilDirection()
    {
        if (playingCardData == null)
        {
            return Vector2.zero;
        }

        Vector2 v = playingCardData.sigilDirection;
        return v.normalized;
    }

    public float GetPower()
    {
        return playingCardData.power;
    }

    private void OnMouseUp()
    {
    }
}