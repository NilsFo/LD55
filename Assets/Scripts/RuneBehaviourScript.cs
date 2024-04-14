using System;
using UnityEngine;

public class RuneBehaviourScript : MonoBehaviour
{
    public SummoningCircleBehaviourScript summoningCircleBehaviourScript;
    public int runeId;
    public SelectorTarget mySelector;

    private GameState _gameState;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void Start()
    {
        summoningCircleBehaviourScript.onRuneActivation.AddListener(OnActivation);
    }

    public void OnActivation(int index)
    {
        if (runeId == index)
        {
            //TODO
            Debug.Log("TODO: Play Animation for Rune Activation!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayingCardBehaviour PlayingCardBehaviour = other.gameObject.GetComponent<PlayingCardBehaviour>();
        if (PlayingCardBehaviour != null)
        {
            if (runeId == 1)
            {
                summoningCircleBehaviourScript.SetRuneOne(PlayingCardBehaviour);
            }
            else if (runeId == 2)
            {
                summoningCircleBehaviourScript.SetRuneTwo(PlayingCardBehaviour);
            }
            else if (runeId == 3)
            {
                summoningCircleBehaviourScript.SetRuneThree(PlayingCardBehaviour);
            }
            else if (runeId == 4)
            {
                summoningCircleBehaviourScript.SetRuneFour(PlayingCardBehaviour);
            }
            else if (runeId == 5)
            {
                summoningCircleBehaviourScript.SetRuneFive(PlayingCardBehaviour);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayingCardBehaviour PlayingCardBehaviour = other.gameObject.GetComponent<PlayingCardBehaviour>();
        if (PlayingCardBehaviour != null)
        {
            if (runeId == 1)
            {
                summoningCircleBehaviourScript.ClearRuneOne(PlayingCardBehaviour);
            }
            else if (runeId == 2)
            {
                summoningCircleBehaviourScript.ClearRuneTwo(PlayingCardBehaviour);
            }
            else if (runeId == 3)
            {
                summoningCircleBehaviourScript.ClearRuneThree(PlayingCardBehaviour);
            }
            else if (runeId == 4)
            {
                summoningCircleBehaviourScript.ClearRuneFour(PlayingCardBehaviour);
            }
            else if (runeId == 5)
            {
                summoningCircleBehaviourScript.ClearRuneFive(PlayingCardBehaviour);
            }
        }
    }
    
}
