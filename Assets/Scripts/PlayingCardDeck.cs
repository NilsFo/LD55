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

    // Start is called before the first frame update
    void Start()
    {
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

        lastDrawnCardNameSpecial = cardData.cardName;
        return cardData;
    }
}