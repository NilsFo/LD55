using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardHand : MonoBehaviour
{
    [Header("Gameplay config")] public bool alignX;
    public bool alignY;
    public bool alignZ;
    public float drawDelay = 0.69f;
    private float _drawDelay = 0;

    [Header("Cards in Hand")] public float cardOffset = 1f;
    public List<PlayingCardBehaviour> cardsInHand;

    [Header("Hookup")] public GameObject cardPrefab;
    public Transform daemonCreationOrigin;
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
        _drawDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _drawDelay += Time.deltaTime;

        // Drawing next card?
        if (_gameState.drawsRemaining > 0 && _drawDelay > drawDelay)
        {
            PlayingCardData cardDataData = _gameState.deckGameObject.NextCard();
            GameObject playingCardObj = Instantiate(cardPrefab, transform);
            playingCardObj.transform.position = _gameState.deckGameObject.transform.position;

            PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
            cardBehaviour.playingCardData = cardDataData;
            cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.DrawAnimation;

            cardsInHand.Add(cardBehaviour);
            _gameState.drawsRemaining--;
            _drawDelay = 0;
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
        int futureCardsInHand = CardsInHandCount + _gameState.drawsRemaining;
        int centerIndex = futureCardsInHand / 2;
        float oddOffset = 0;
        if (futureCardsInHand % 2 == 0)
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
        // cardsInHand.Clear();
    }

    public void CreateDaemonCard()
    {
        GameObject playingCardObj = Instantiate(cardPrefab, transform);
        playingCardObj.transform.position = daemonCreationOrigin.position;
        
        PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
        cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.DrawAnimation;

        cardBehaviour.isDaemon = true;
        
        // TODO add name
        cardBehaviour.displayName = "New Daemon";
        cardBehaviour.basePower = (int)Mathf.Ceil(_gameState.summoningCircle.Value * _gameState.daemonCardPowerMod);

        cardBehaviour.sigilDirection = _gameState.currentLevelSigil;
        
        cardsInHand.Add(cardBehaviour);
    }

    
}