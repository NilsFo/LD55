using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHand : MonoBehaviour
{
    [Header("Gameplay config")] public bool alignX;
    public bool alignY;
    public bool alignZ;

    [Header("Cards in Hand")] public List<PlayingCardBehaviour> cardsInHand;
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
        if (_gameState.drawsRemaining > 0)
        {
            PlayingCardData cardDataData = _gameState.deckGameObject.NextCard();
            GameObject playingCardObj = Instantiate(cardPrefab, transform);
            playingCardObj.transform.position = _gameState.deckGameObject.transform.position;

            PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
            cardBehaviour.playingCardData = cardDataData;
            cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.InHand;

            cardsInHand.Add(cardBehaviour);
            _gameState.drawsRemaining--;
        }

        // Updating card Positions
        for (var i = 0; i < cardsInHand.Count; i++)
        {
            PlayingCardBehaviour card = cardsInHand[i];
            card.handWorldPos = GetDesiredCardPosition(i);
        }
    }

    public Vector3 GetDesiredCardPosition(int index)
    {
        int centerIndex = CardsInHandCount / 2;
        float oddOffset = 0;
        if (CardsInHandCount % 2 == 0)
        {
            oddOffset = cardOffset / 2;
        }

        Vector3 pos = transform.position;
        if (alignX)
        {
            pos.x += (index - centerIndex) * cardOffset + oddOffset;
        }

        if (alignY)
        {
            pos.y += (index - centerIndex) * cardOffset + oddOffset;
        }

        if (alignZ)
        {
            pos.z += (index - centerIndex) * cardOffset + oddOffset;
        }

        return pos;
    }

    public void OnEndOfRound()
    {
        cardsInHand.Clear();
    }
    
}