using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private GameState _gameState;
    public int usesRemaining = 4;
    private int _usesOriginal;
    public GameObject myLight;

    // Start is called before the first frame update
    void Start()
    {
        _usesOriginal = usesRemaining;
        _gameState = FindObjectOfType<GameState>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        myLight.SetActive(usesRemaining != 0);
    }

    public void OnBurn(PlayingCardBehaviour playingCardBehaviour)
    {
        if (usesRemaining == 0)
        {
            print("YOU NO USE CANDLE!");
            return;
        }

        print("Burning Card: " + playingCardBehaviour.name);
        usesRemaining--;

        if (playingCardBehaviour.isBurned)
        {
            _gameState.drawsRemaining += 1;
            playingCardBehaviour.DestroyCard();
        }
        else
        {
            playingCardBehaviour.isBurned = true;
        }
    }

    public void Reset()
    {
        print("Resetting candle.");
        usesRemaining = _usesOriginal;
    }
}