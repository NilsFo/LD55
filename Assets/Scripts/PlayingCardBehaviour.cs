using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public UnityEngine.UI.Image backgroundImg;
    public Sigil sigil;
    public float drawAnimationDuration = .5f;

    [Header("Card visual values")] public string displayName;
    public int basePower;
    public Vector2 sigilDirection;
    public Sprite sprite;

    [Header("Card visual defaults")] 
    public Color titleTextColorDefault = new Color(60/255f,51/255f,76/255f);
    public Color titleTextColorFoil = new Color(42/255f,73/255f,41/255f);
    public Color titleTextColorDemonic = new Color(114/255f,54/255f,39/255f);
    public Sprite cardBackgroundDefault;
    public Sprite cardBackgroundSinged;

    [Header("Gameplay Modifiers")] public PlayingCardState playingCardState;
    private PlayingCardState _playingCardState;
    public bool isBurned = false;
    public bool isBloodSoaked = false;
    public bool isFoil = false;
    public bool isDaemon = false;

    [Header("Parameters")] public float movementSpeed = 7;
    public float rotationSpeed = 10;
    public float selectionHoverDistance = 0.6f;

    [Header("Readonly")] public bool inTransition;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playingCardState = playingCardState;
        _gameState = FindObjectOfType<GameState>();

        if (playingCardData != null)
        {
            displayName = playingCardData.cardName;
            basePower = playingCardData.PowerScala();
            sprite = playingCardData.sprite;
            sigilDirection = playingCardData.sigilDirection;
        }

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
            if (_playingCardState == PlayingCardState.Drawing)
            {
                var tween = transform.DOMove(handWorldPos, drawAnimationDuration).SetEase(Ease.OutCubic);
                tween.OnUpdate(() => { tween.SetTarget(handWorldPos); });
                tween.OnComplete(() =>
                {
                    Debug.Log("Tween complete");
                    playingCardState = PlayingCardState.InHand;
                });
                tween.Play();

                transform.DORotateQuaternion(
                    Quaternion.LookRotation(
                        _gameState.camera.transform.forward,
                        Vector3.right),
                    drawAnimationDuration).Play();
            }
            else if (_playingCardState == PlayingCardState.Selected)
            {

                transform.DORotateQuaternion(
                    Quaternion.LookRotation(Vector3.down, Vector3.right),
                    drawAnimationDuration).Play();
            }
        }

        transform.DOPlay();
        var rot = transform.rotation.eulerAngles;
        switch (playingCardState)
        {
            case PlayingCardState.Drawing:
                /*transform.position =
                    Vector3.MoveTowards(transform.position, handWorldPos, Time.deltaTime * movementSpeed);*/
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
                var f = transform.forward;
                var r = -Vector3.up;
                var l = Vector3.RotateTowards(f, r, Time.deltaTime * movementSpeed, 0.0f);
                transform.rotation = Quaternion.LookRotation(l, Vector3.right);

                break;
            case PlayingCardState.Selected:
                // Updating mouse pos if selected

                Vector3 mousePos = _gameState.mouseCardPlaneTargetPos;
                if (_gameState.mouseSelectHasTarget)
                {
                    mousePos += new Vector3(0, -selectionHoverDistance, 0);
                }
                transform.position += (mousePos - transform.position) * (10f * Time.deltaTime);
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
        UpdateVisuals();

        gameObject.name = GetName() + ": " + GetPower() + " -> " + GetSigilDirection();
    }

    private void UpdateVisuals()
    {
        nameTF.text = GetName();
        powerTF.text = GetPower().ToString();
        artworkImg.sprite = sprite;
        sigil.dir = GetSigilDirection();
        sigil.UpdateSigilSprite();
        if(isBurned)
            backgroundImg.sprite = cardBackgroundSinged;
        else 
            backgroundImg.sprite = cardBackgroundDefault;

        if(isFoil)
            nameTF.color = titleTextColorFoil;
        else if(isDaemon)
            nameTF.color = titleTextColorDemonic;
        else 
            nameTF.color = titleTextColorDefault;
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
                DragCard();
            }
        }
    }

    public void ReturnToHand()
    {
        Debug.Log("Returning to hand: " + name);
        playingCardState = PlayingCardState.Drawing;
        
        _gameState.playingState = GameState.PlayingState.Default;
        if (!_gameState.handGameObject.cardsInHand.Contains(this))
        {
            _gameState.handGameObject.cardsInHand.Add(this);
            handWorldPos = _gameState.handGameObject.GetDesiredCardPosition(_gameState.handGameObject.cardsInHand.Count - 1);
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
        playedWorldPos = _gameState.mouseSelectTargetObj.transform.position;
        SelectorTarget targetObj = _gameState.mouseSelectTargetObj;

        if (targetObj.placeable)
        {
            _gameState.playingState = GameState.PlayingState.Default;
            playingCardState = PlayingCardState.Played;
            _gameState.handGameObject.cardsInHand.Remove(this);
        }
        else
        {
            ReturnToHand();
        }
    }

    public void OnRoundEnd()
    {
        if (playingCardState == PlayingCardState.Played)
        {
            DestroyCard();
        }
    }

    public void DestroyCard()
    {
        if (playingCardState == PlayingCardState.Selected)
        {
            _gameState.playingState = GameState.PlayingState.Default;
        }
        _gameState.handGameObject.cardsInHand.Remove(this);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _gameState.handGameObject.cardsInHand.Remove(this);
    }

    public float GetPower()
    {
        float power = basePower;

        if (isFoil)
        {
            power += Mathf.CeilToInt(((float)power * _gameState.cardFoilMult));
        }

        if (isBurned)
        {
            power += Mathf.CeilToInt(((float)power * _gameState.cardBurningMult));
        }

        return power;
    }

    public Vector2 GetSigilDirection()
    {
        Vector2 v = sigilDirection;
        return v.normalized;
    }

    public string GetName()
    {
        string name = displayName;
        if (isBurned)
        {
            name = "Singed " + name;
        }

        if (isFoil)
        {
            name = name + "+";
        }

        return name;
    }
}
