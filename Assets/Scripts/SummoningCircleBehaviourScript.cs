using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCircleBehaviourScript : MonoBehaviour
{
    
    public UnityEvent onRuneChangeEvent;
    public UnityEvent onRuneConnectionChangeEvent;

    public PlayingCardBehaviour runeOne;
    public PlayingCardBehaviour runeTwo;
    public PlayingCardBehaviour runeThree;
    public PlayingCardBehaviour runeFour;
    public PlayingCardBehaviour runeFive;
    
    public RuneBehaviourScript runeBehaviourOne;
    public RuneBehaviourScript runeBehaviourTwo;
    public RuneBehaviourScript runeBehaviourThree;
    public RuneBehaviourScript runeBehaviourFour;
    public RuneBehaviourScript runeBehaviourFive;
    
    //Stats
    public Vector2 resultRuneOne;
    public Vector2 resultRuneTwo;
    public Vector2 resultRuneThree;
    public Vector2 resultRuneFour;
    public Vector2 resultRuneFive;

    public Vector2 resultRuneTotal;

    //Dot Product 1 same direction <-> -1 opposite directions
    public float connectionR1R2;
    public float connectionR1R3;
    public float connectionR1R4;
    public float connectionR1R5;
    public float connectionR2R3;
    public float connectionR2R4;
    public float connectionR2R5;
    public float connectionR3R4;
    public float connectionR3R5;
    public float connectionR4R5;
    
    //Queue
    List<PlayingCardBehaviour> listRuneOne = new List<PlayingCardBehaviour>();
    List<PlayingCardBehaviour> listRuneTwo = new List<PlayingCardBehaviour>();
    List<PlayingCardBehaviour> listRuneThree = new List<PlayingCardBehaviour>();
    List<PlayingCardBehaviour> listRuneFour = new List<PlayingCardBehaviour>();
    List<PlayingCardBehaviour> listRuneFive = new List<PlayingCardBehaviour>();

    private GameState _gameState;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void Start()
    {
        if (onRuneChangeEvent == null)
            onRuneChangeEvent = new UnityEvent();
        if (onRuneConnectionChangeEvent == null)
            onRuneConnectionChangeEvent = new UnityEvent();
    }

    void UpdateStats()
    {
        #region resultRuneOne
        //V1 = + r1 + r2 + r5 - r4 - r3
        resultRuneOne = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneOne += runeOne.CurrentPower;
        }
        if (runeTwo != null)
        {
            resultRuneOne += runeTwo.CurrentPower;
        }
        if (runeThree != null)
        {
            resultRuneOne -= runeThree.CurrentPower;
        }
        if (runeFour != null)
        {
            resultRuneOne -= runeFour.CurrentPower;
        }
        if (runeFive != null)
        {
            resultRuneOne += runeFive.CurrentPower;
        }
        #endregion
        
        #region resultRuneTwo
        //V2 = + r2 + r3 + r1 - r4 - r5
        resultRuneTwo = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneTwo += runeOne.CurrentPower;
        }
        if (runeTwo != null)
        {
            resultRuneTwo += runeTwo.CurrentPower;
        }
        if (runeThree != null)
        {
            resultRuneTwo += runeThree.CurrentPower;
        }
        if (runeFour != null)
        {
            resultRuneTwo -= runeFour.CurrentPower;
        }
        if (runeFive != null)
        {
            resultRuneTwo -= runeFive.CurrentPower;
        }
        #endregion
        
        #region resultRuneThree
        //V3 = + r3 + r4 + r2 - r5 - r1
        resultRuneThree = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneThree -= runeOne.CurrentPower;
        }
        if (runeTwo != null)
        {
            resultRuneThree += runeTwo.CurrentPower;
        }
        if (runeThree != null)
        {
            resultRuneThree += runeThree.CurrentPower;
        }
        if (runeFour != null)
        {
            resultRuneThree += runeFour.CurrentPower;
        }
        if (runeFive != null)
        {
            resultRuneThree -= runeFive.CurrentPower;
        }
        #endregion
        
        #region resultRuneFour
        //V4 = + r4 + r5 + r3 - r2 - r1
        resultRuneFour = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneFour -= runeOne.CurrentPower;
        }
        if (runeTwo != null)
        {
            resultRuneFour -= runeTwo.CurrentPower;
        }
        if (runeThree != null)
        {
            resultRuneFour += runeThree.CurrentPower;
        }
        if (runeFour != null)
        {
            resultRuneFour += runeFour.CurrentPower;
        }
        if (runeFive != null)
        {
            resultRuneFour += runeFive.CurrentPower;
        }
        #endregion
        
        #region resultRuneFive
        //V5 = + r5 + r4 + r1 - r2 - r3
        resultRuneFive = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneFive += runeOne.CurrentPower;
        }
        if (runeTwo != null)
        {
            resultRuneFive -= runeTwo.CurrentPower;
        }
        if (runeThree != null)
        {
            resultRuneFive -= runeThree.CurrentPower;
        }
        if (runeFour != null)
        {
            resultRuneFive += runeFour.CurrentPower;
        }
        if (runeFive != null)
        {
            resultRuneFive += runeFive.CurrentPower;
        }
        #endregion

        resultRuneTotal = resultRuneOne + resultRuneTwo + resultRuneThree + resultRuneFour + resultRuneFive;
        onRuneChangeEvent?.Invoke();

        #region Connections

        if (runeOne != null)
        {
            connectionR1R2 = runeTwo != null ? Vector2.Dot(runeOne.CurrentPower, runeTwo.CurrentPower) : 0f;
            connectionR1R3 = runeThree != null ? Vector2.Dot(runeOne.CurrentPower, runeThree.CurrentPower) : 0f;
            connectionR1R4 = runeFour != null ? Vector2.Dot(runeOne.CurrentPower, runeFour.CurrentPower) : 0f;
            connectionR1R5 = runeFive != null ? Vector2.Dot(runeOne.CurrentPower, runeFive.CurrentPower) : 0f;
        }
        else
        {
            connectionR1R2 = 0f;
            connectionR1R3 = 0f;
            connectionR1R4 = 0f;
            connectionR1R5 = 0f;
        }

        if (runeTwo != null)
        {
            connectionR2R3 = runeThree != null ? Vector2.Dot(runeTwo.CurrentPower, runeThree.CurrentPower) : 0f;
            connectionR2R4 = runeFour != null ? Vector2.Dot(runeTwo.CurrentPower, runeFour.CurrentPower) : 0f;
            connectionR2R5 = runeFive != null ? Vector2.Dot(runeTwo.CurrentPower, runeFive.CurrentPower) : 0f;
        }
        else
        {
            connectionR2R3 = 0f;
            connectionR2R4 = 0f;
            connectionR2R5 = 0f;
        }

        if (runeThree != null)
        {
            connectionR3R4 = runeFour != null ? Vector2.Dot(runeThree.CurrentPower, runeFour.CurrentPower) : 0f;
            connectionR3R5 = runeFive != null ? Vector2.Dot(runeThree.CurrentPower, runeFive.CurrentPower) : 0f;
        }
        else
        {
            connectionR3R4 = 0f;
            connectionR3R5 = 0f;
        }

        if (runeFour != null)
        {
            connectionR4R5 = runeFive != null ? Vector2.Dot(runeFour.CurrentPower, runeFive.CurrentPower) : 0f;
        }
        else
        {
            connectionR4R5 = 0f;
        }
        
        onRuneConnectionChangeEvent?.Invoke();
        
        #endregion
    }

    public void SetRuneOne(PlayingCardBehaviour newCard)
    {
        if (runeOne != null)
        {
            listRuneOne.Add(newCard);
            return;
        }
        runeOne = newCard;
        UpdateStats();
    }
    
    public void SetRuneTwo(PlayingCardBehaviour newCard)
    {
        if (runeTwo != null)
        {
            listRuneTwo.Add(newCard);
            return;
        }
        runeTwo = newCard;
        UpdateStats();
    }
    
    public void SetRuneThree(PlayingCardBehaviour newCard)
    {
        if (runeThree != null)
        {
            listRuneThree.Add(newCard);
            return;
        }
        runeThree = newCard;
        UpdateStats();
    }
    
    public void SetRuneFour(PlayingCardBehaviour newCard)
    {
        if (runeFour != null)
        {
            listRuneFour.Add(newCard);
            return;
        }
        runeFour = newCard;
        UpdateStats();
    }
    
    public void SetRuneFive(PlayingCardBehaviour newCard)
    {
        if (runeFive != null)
        {
            listRuneFive.Add(newCard);
            return;
        }
        runeFive = newCard;
        UpdateStats();
    }
    
    public void ClearRuneOne(PlayingCardBehaviour oldCard)
    {
        if (listRuneOne.Contains(oldCard))
        {
            listRuneOne.Remove(oldCard);
        }
        else if (runeOne == oldCard)
        {
            runeOne = null;
            if (listRuneOne.Count > 0)
            {
                runeOne = listRuneOne[0];
                listRuneOne.RemoveAt(0);
            }
            UpdateStats();
        }
    }
    
    public void ClearRuneTwo(PlayingCardBehaviour oldCard)
    {
        if (listRuneTwo.Contains(oldCard))
        {
            listRuneTwo.Remove(oldCard);
        }
        else if (runeTwo == oldCard)
        {
            runeTwo = null;
            if (listRuneTwo.Count > 0)
            {
                runeTwo = listRuneTwo[0];
                listRuneTwo.RemoveAt(0);
            }
            UpdateStats();
        }
    }
    
    public void ClearRuneThree(PlayingCardBehaviour oldCard)
    {
        if (listRuneThree.Contains(oldCard))
        {
            listRuneThree.Remove(oldCard);
        }
        else if (runeThree == oldCard)
        {
            runeThree = null;
            if (listRuneThree.Count > 0)
            {
                runeThree = listRuneThree[0];
                listRuneThree.RemoveAt(0);
            }
            UpdateStats();
        }
    }
    
    public void ClearRuneFour(PlayingCardBehaviour oldCard)
    {
        if (listRuneFour.Contains(oldCard))
        {
            listRuneFour.Remove(oldCard);
        }
        else if (runeFour == oldCard)
        {
            runeFour = null;
            if (listRuneFour.Count > 0)
            {
                runeFour = listRuneFour[0];
                listRuneFour.RemoveAt(0);
            }
            UpdateStats();
        }
    }
    
    public void ClearRuneFive(PlayingCardBehaviour oldCard)
    {
        if (listRuneFive.Contains(oldCard))
        {
            listRuneFive.Remove(oldCard);
        }
        else if (runeFive == oldCard)
        {
            runeFive = null;
            if (listRuneFive.Count > 0)
            {
                runeFive = listRuneFive[0];
                listRuneFive.RemoveAt(0);
            }
            UpdateStats();
        }
    }

     void Update()
    {
        runeBehaviourOne.mySelector.active=runeOne==null;
        runeBehaviourTwo.mySelector.active=runeTwo==null;
        runeBehaviourThree.mySelector.active=runeThree==null;
        runeBehaviourFour.mySelector.active=runeFour==null;
        runeBehaviourFive.mySelector.active=runeFive==null;
    }
}
