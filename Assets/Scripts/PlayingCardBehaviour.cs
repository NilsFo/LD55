using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayingCardBehaviour : MonoBehaviour
{
    public enum PlayingCardState
    {
        Unknown,
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

    [Header("Gameplay Modifiers")] public PlayingCardState playingCardState;
    private PlayingCardState _playingCardState;
    public bool isBurned = false;
    public bool isBloodSoaked = false;
    public bool isFoil = false;

    [Header("Parameters")] public float movementSpeed = 7;
    public float rotationSpeed = 10;

    [Header("Readonly")] public bool inTransition;

    // Power
    public Vector2 CurrentPower => GetSigilDirection();

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playingCardState = playingCardState;
        _gameState = FindObjectOfType<GameState>();

        if (Random.Range(0f, 1f) <= _gameState.cardFoilChance)
        {
            isFoil = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playingCardState == PlayingCardState.Unknown)
        {
            Debug.LogError("Unknown card state");
            return;
        }

        if (_playingCardState != playingCardState)
        {
            _playingCardState = playingCardState;
            Debug.Log("New card state: " + playingCardState);
        }

        var rot = transform.rotation.eulerAngles;
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
                    Vector3.MoveTowards(transform.position, playedWorldPos, Time.deltaTime * rotationSpeed);
                inTransition = !(Vector3.Distance(transform.position, playedWorldPos) <= 0.01f);

                // rotation
                var f = transform.forward * -1;
                var r = Vector3.up;
                var l = Vector3.RotateTowards(f, r, Time.deltaTime * movementSpeed, 0.0f);
                transform.rotation = Quaternion.LookRotation(l, Vector3.up);

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

                // rotation
                transform.rotation = Quaternion.Euler(Vector3.MoveTowards(rot,
                    _gameState.camera.transform.rotation.eulerAngles, Time.deltaTime * rotationSpeed));

                break;
            default:
                playingCardState = PlayingCardState.Unknown;
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
            powerTF.text = GetPower().ToString();
            artworkImg.sprite = playingCardData.sprite;
            sigil.dir = GetSigilDirection();
            sigil.UpdateSigilSprite();
        }
    }

    public void GetSigilSprite(Vector2 dir)
    {
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
            if (playingCardState == PlayingCardState.Played && _gameState.allowCardPickUp)
            {
                ReturnToHand();
            }
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
        if (_gameState.mouseSelectTargetObj == null)
        {
            Debug.LogWarning("Cannot place. Nothing selected.");
            return;
        }
        
        // Notifying listeners
        _gameState.mouseSelectTargetObj.Select(this);

        // Updating stuff
        playedWorldPos = _gameState.mouseSelectTargetPos;
        SelectorTarget targetObj = _gameState.mouseSelectTargetObj;

        if (targetObj.placeable)
        {
            _gameState.playingState = GameState.PlayingState.Default;
            playingCardState = PlayingCardState.Played;
            _gameState.handGameObject.cardsInHand.Remove(this);
        }
    }

    private Vector2 GetSigilDirection()
    {
        if (playingCardData == null)
        {
            return Vector2.zero;
        }

        Vector2 v = playingCardData.sigilDirection;
        return v.normalized;
    }

    private void OnMouseUp()
    {
    }

    public void OnRoundEnd()
    {
        DestroyCard();
    }

    public void DestroyCard()
    {
        if (playingCardState == PlayingCardState.Selected)
        {
            _gameState.playingState = GameState.PlayingState.Default;
        }

        if (_gameState.handGameObject.cardsInHand.Contains(this))
        {
            _gameState.handGameObject.cardsInHand.Remove(this);
        }

        Destroy(gameObject);
    }

    public int GetPower()
    {
        int basePower = playingCardData.PowerScala();

        if (isFoil)
        {
            basePower += Mathf.CeilToInt(((float)basePower * _gameState.cardFoilMult));
        }

        if (isBurned)
        {
            basePower += Mathf.CeilToInt(((float)basePower * _gameState.cardBurningMult));
        }

        return basePower;
    }
}