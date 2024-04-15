using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayingCardDeck : MonoBehaviour
{
    public CardPool cardPool;
    public CardPool cardPoolSpecial;

    public string lastDrawnCardName="";
    public string lastDrawnCardNameSpecial="";

    public Transform cardSpawnPoint;
    public Transform cardDrawAnimPoint;

    public AudioClip drawSoundClip;
    private GameState _gameState;

    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public PlayingCardData NextCard()
    {
        PlayingCardData cardData = cardPool.Next();
        if (cardData.cardName==lastDrawnCardName)
        {
            return NextCard();
        }

        _gameState.musicManager.CreateAudioClip(drawSoundClip,_gameState.transform.position,respectBinning:false);
        lastDrawnCardName = cardData.cardName;
        return cardData;
    }

    public PlayingCardData NextSpecialCard()
    {
        PlayingCardData cardData = cardPoolSpecial.Next();
        if (cardData.cardName==lastDrawnCardNameSpecial)
        {
            return NextSpecialCard();
        }

        _gameState.PlayCardSound();
        _gameState.musicManager.CreateAudioClip(drawSoundClip,_gameState.transform.position,respectBinning:false);
        return cardData;
    }
}