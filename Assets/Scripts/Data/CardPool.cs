using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Playing Card", menuName = "Cards/New Card Pool", order = 1)]
public class CardPool : ScriptableObject
{
    public List<PlayingCardData> knownCards;

    private void OnValidate()
    {
        for (var i = 0; i < knownCards.Count; i++)
        {
            var card = knownCards[i];
            if (card == null)
            {
                Debug.LogError("Empty card slot!");
            }
        }
    }

    public PlayingCardData Next()
    {
        List<PlayingCardData> cardPool = new List<PlayingCardData>();

        foreach (PlayingCardData card in knownCards)
        {
            for (int i = 0; i < card.rarity; i++)
            {
                cardPool.Add(card);
            }
        }

        return cardPool[Random.Range(0, cardPool.Count)];
    }
}