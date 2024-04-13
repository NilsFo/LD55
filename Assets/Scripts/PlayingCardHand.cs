using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHand : MonoBehaviour
{
    [Header("Gameplay config")] public int handSize = 5;

    [Header("Cards in Hand")] public List<PlayingCardLogic> cardsInHand;
    public float cardOffset = 1f;

    [Header("Hookup")] public GameObject cardPrefab;

    private GameState _gameState;

    // Properties
    public int CardsInHandCount => cardsInHand.Count;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Drawing next card?
        if (CardsInHandCount < handSize)
        {
            PlayingCard cardData = _gameState.deckGameObject.NextCard();
            GameObject playingCardObj = Instantiate(cardPrefab, transform);
            playingCardObj.transform.position = _gameState.deckGameObject.transform.position;

            PlayingCardLogic cardLogic = playingCardObj.GetComponent<PlayingCardLogic>();
            cardLogic.playingCardBase = cardData;

            cardsInHand.Add(cardLogic);
        }

        // Updating card Positions
        for (var i = 0; i < cardsInHand.Count; i++)
        {
            PlayingCardLogic card = cardsInHand[i];
            card.handWorldPos = GetDesiredCardPosition(i);
        }
    }

    public Vector3 GetDesiredCardPosition(int index)
    {
        Vector3 pos = transform.position;
        pos.x += index * cardOffset;

        return pos;
    }
}