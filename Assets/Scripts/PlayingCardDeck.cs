using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayingCardDeck : MonoBehaviour
{
    public CardPool cardPool;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public PlayingCard NextCard()
    {
        Debug.Log("Request to draw next card.");
        PlayingCard card = cardPool.Next();

        Debug.Log("Card drawn: " + card.cardName);
        return card;
    }
}