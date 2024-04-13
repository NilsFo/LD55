using UnityEngine;

[CreateAssetMenu(fileName = "New Playing Card", menuName = "Cards/New Playing Card", order = 1)]
public class PlayingCard : ScriptableObject
{
    public string cardName;
    public Vector2Int power;
    public Sprite sprite;

    public int weight = 10;
}