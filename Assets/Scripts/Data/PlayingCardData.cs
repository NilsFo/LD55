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
}