using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private GameState _gameState;
    public int usesRemaining = 4;
    public bool infiniteUses = true;
    public bool canDestroyCards = false;
    public bool canBuffCards = false;
    private int _usesOriginal;
    public List<GameObject> attachedObjects;

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
        foreach (GameObject obj in attachedObjects)
        {
            obj.SetActive(usesRemaining != 0);
        }
    }

    public void OnBurn(PlayingCardBehaviour playingCardBehaviour)
    {
        if (usesRemaining == 0)
        {
            print("YOU NO USE CANDLE!");
            return;
        }

        print("Burning Card: " + playingCardBehaviour.name);
        if (!infiniteUses) usesRemaining--;

        if (playingCardBehaviour.isBurned)
        {
            if (canDestroyCards)
            {
                _gameState.drawsRemaining += 1;
                playingCardBehaviour.DestroyCard();
            }
        }
        else
        {
            if (canBuffCards)
            {
                playingCardBehaviour.isBurned = true;
            }
        }
    }

    public void Reset()
    {
        //print("Resetting candle.");
        usesRemaining = _usesOriginal;
    }
}