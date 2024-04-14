using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayingCardHand : MonoBehaviour
{
    [Header("Gameplay config")] public bool alignX;
    public bool alignY;
    public bool alignZ;
    public float drawDelay = 0.69f;
    private float _drawDelay = 0;
    public string currentDemonName;

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
        // todo only create when sigil matches?
        
        GameObject playingCardObj = Instantiate(cardPrefab, transform);
        playingCardObj.transform.position = daemonCreationOrigin.position;

        PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
        cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.DrawAnimation;

        cardBehaviour.isDaemon = true;
        cardBehaviour.isFoil = false;

        cardBehaviour.basePower = (int)Mathf.Ceil(_gameState.summoningCircle.Value * _gameState.daemonCardPowerMod);
        cardBehaviour.displayName = currentDemonName;
        cardBehaviour.sigilDirection = _gameState.currentLevelSigil;

        cardsInHand.Add(cardBehaviour);
    }

    public void OnNewRound()
    {
        NextDemonName();
    }

    public void NextDemonName()
    {
        List<string> daemonNames = new List<string>
        {
            "Baal the Bile",
            "The Crimson Carder",
            "Venomheart the Tormentor",
            "Blightwing, devourer of Souls",
            "The Ten of Tormends",
            "The Queen of Pain",
            "The Dealer of Doom",
            "Heart of Hellfire",
            "The Nine of Nightmares",
            "The Ace of the Abyss",
            "Sulfur's Shadow",
            "Globulus the Glutton",
            "The Folding Fiend",
            "The Jack of Judgement",
            "Beelzebub",
            "Abaddon",
            "Asmodeus",
            "Belphegor",
            "The Leviathan",
            "Bathory the Unjust",
            "Rhadamanthus",
            "Gorgoroth the Unrepented",
            "Coagula the Uncooth",
            "Abraxas",
            "Geryon"
        };

        int i = Random.Range(0, daemonNames.Count);
        currentDemonName = daemonNames[i];
    }
    
    
}