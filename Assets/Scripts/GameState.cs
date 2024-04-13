using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [Header("World Hookup")] public Camera camera;
    public PlayingCardHand handGameObject;
    public PlayingCardDeck deckGameObject;

    [Header("Game Rules")] public bool TEST;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}