using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Playing Card", menuName = "Cards/New Playing Card", order = 1)]
public class PlayingCardData : ScriptableObject
{
    public string cardName;
    public Vector2Int sigilDirection;
    public int power;
    public Sprite sprite;

    public int rarity = 10;

    public int PowerScala()
    {
        return power;
    }

    private void OnValidate()
    {
        // if (power == 0)
        // {
        //     Debug.LogError("Power of playing card " + cardName + " is 0!");
        // }
// 
        // if (sigilDirection.x == 0 && sigilDirection.y == 0)
        // {
        //     Debug.LogError("Direction of playing card " + cardName + " is 0!");
        // }
// 
        // if (sigilDirection.magnitude != 1)
        // {
        //     Debug.LogError("Magnitude of playing card " + cardName + " is not 1!");
        // }
    }
}