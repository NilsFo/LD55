using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
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

    public void OnBurn(PlayingCardBehaviour playingCardBehaviour)
    {
        print("Burning Card: " + playingCardBehaviour.name);

        if (playingCardBehaviour.isBurned)
        {
            playingCardBehaviour.DestroyCard();
            _gameState.drawsRemaining += 1;
        }
        else
        {
            playingCardBehaviour.isBurned = true;
        }
    }
}