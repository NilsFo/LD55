using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public Sprite daemonSprite;

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
            DrawNextCard();
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

    public void DrawNextCard()
    {
        PlayingCardData cardDataData;
        float roll = Random.Range(0f, 1f);

        // roll for special card
        if (cardsInHand.Count < _gameState.specialCardMax && roll <= _gameState.specialCardDrawChance)
        {
            cardDataData = _gameState.deckGameObject.NextSpecialCard();
        }
        else
        {
            cardDataData = _gameState.deckGameObject.NextCard();
        }

        GameObject playingCardObj = Instantiate(cardPrefab, transform);
        playingCardObj.transform.position = _gameState.deckGameObject.cardSpawnPoint.position;
        playingCardObj.transform.rotation = _gameState.deckGameObject.cardSpawnPoint.rotation;

        PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
        cardBehaviour.playingCardData = cardDataData;
        cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.DrawAnimation;
        var moveTween = playingCardObj.transform.DOMove(_gameState.deckGameObject.cardDrawAnimPoint.position, .5f)
            .SetEase(Ease.OutQuad);
        moveTween.OnComplete(() => { cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.Drawing; });
        moveTween.Play();
        playingCardObj.transform.DORotateQuaternion(_gameState.deckGameObject.cardDrawAnimPoint.rotation, .5f)
            .SetEase(Ease.OutQuad).Play();

        cardsInHand.Add(cardBehaviour);
    }

    public bool HasSpecialCardInHand()
    {
        foreach (PlayingCardBehaviour card in cardsInHand)
        {
            if (card.IsEffectCard)
            {
                return true;
            }
        }

        return false;
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

    public void CreateDaemonCard(float offset = 0)
    {
        GameObject playingCardObj = Instantiate(cardPrefab, transform);
        playingCardObj.transform.position = daemonCreationOrigin.position + new Vector3(0, 0, -offset);

        PlayingCardBehaviour cardBehaviour = playingCardObj.GetComponent<PlayingCardBehaviour>();
        cardBehaviour.playingCardState = PlayingCardBehaviour.PlayingCardState.DrawAnimation;

        cardBehaviour.isDaemon = true;
        cardBehaviour.isFoil = false;
        cardBehaviour.sprite = daemonSprite;

        cardBehaviour.basePower = (int)Mathf.Ceil(_gameState.summoningCircle.Value * _gameState.daemonCardPowerMod);
        cardBehaviour.displayName = currentDemonName;
        cardBehaviour.sigilDirection = _gameState.currentLevelSigil;

        _gameState.demomCreationCount += 1;
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
