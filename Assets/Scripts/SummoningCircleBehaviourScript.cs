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
    
    public RuneConnectionBehaviourScript connectionR1R2;
    public RuneConnectionBehaviourScript connectionR1R3;
    public RuneConnectionBehaviourScript connectionR1R4;
    public RuneConnectionBehaviourScript connectionR1R5;
    public RuneConnectionBehaviourScript connectionR2R3;
    public RuneConnectionBehaviourScript connectionR2R4;
    public RuneConnectionBehaviourScript connectionR2R5;
    public RuneConnectionBehaviourScript connectionR3R4;
    public RuneConnectionBehaviourScript connectionR3R5;
    public RuneConnectionBehaviourScript connectionR4R5;

    public float resultTotalPower = 0f;
    
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
            resultRuneOne += runeOne.GetSigilDirection();
        }
        if (runeTwo != null)
        {
            resultRuneOne += runeTwo.GetSigilDirection();
        }
        if (runeThree != null)
        {
            resultRuneOne -= runeThree.GetSigilDirection();
        }
        if (runeFour != null)
        {
            resultRuneOne -= runeFour.GetSigilDirection();
        }
        if (runeFive != null)
        {
            resultRuneOne += runeFive.GetSigilDirection();
        }
        
        resultRuneOne = resultRuneOne.normalized;
        #endregion
        
        #region resultRuneTwo
        //V2 = + r2 + r3 + r1 - r4 - r5
        resultRuneTwo = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneTwo += runeOne.GetSigilDirection();
        }
        if (runeTwo != null)
        {
            resultRuneTwo += runeTwo.GetSigilDirection();
        }
        if (runeThree != null)
        {
            resultRuneTwo += runeThree.GetSigilDirection();
        }
        if (runeFour != null)
        {
            resultRuneTwo -= runeFour.GetSigilDirection();
        }
        if (runeFive != null)
        {
            resultRuneTwo -= runeFive.GetSigilDirection();
        }
        
        resultRuneTwo = resultRuneTwo.normalized;
        #endregion
        
        #region resultRuneThree
        //V3 = + r3 + r4 + r2 - r5 - r1
        resultRuneThree = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneThree -= runeOne.GetSigilDirection();
        }
        if (runeTwo != null)
        {
            resultRuneThree += runeTwo.GetSigilDirection();
        }
        if (runeThree != null)
        {
            resultRuneThree += runeThree.GetSigilDirection();
        }
        if (runeFour != null)
        {
            resultRuneThree += runeFour.GetSigilDirection();
        }
        if (runeFive != null)
        {
            resultRuneThree -= runeFive.GetSigilDirection();
        }

        resultRuneThree = resultRuneThree.normalized;
        #endregion
        
        #region resultRuneFour
        //V4 = + r4 + r5 + r3 - r2 - r1
        resultRuneFour = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneFour -= runeOne.GetSigilDirection();
        }
        if (runeTwo != null)
        {
            resultRuneFour -= runeTwo.GetSigilDirection();
        }
        if (runeThree != null)
        {
            resultRuneFour += runeThree.GetSigilDirection();
        }
        if (runeFour != null)
        {
            resultRuneFour += runeFour.GetSigilDirection();
        }
        if (runeFive != null)
        {
            resultRuneFour += runeFive.GetSigilDirection();
        }

        resultRuneFour = resultRuneFour.normalized;
        #endregion
        
        #region resultRuneFive
        //V5 = + r5 + r4 + r1 - r2 - r3
        resultRuneFive = Vector2.zero;
        if (runeOne != null)
        {
            resultRuneFive += runeOne.GetSigilDirection();
        }
        if (runeTwo != null)
        {
            resultRuneFive -= runeTwo.GetSigilDirection();
        }
        if (runeThree != null)
        {
            resultRuneFive -= runeThree.GetSigilDirection();
        }
        if (runeFour != null)
        {
            resultRuneFive += runeFour.GetSigilDirection();
        }
        if (runeFive != null)
        {
            resultRuneFive += runeFive.GetSigilDirection();
        }

        resultRuneFive = resultRuneFive.normalized;
        #endregion

        resultRuneTotal = resultRuneOne + resultRuneTwo + resultRuneThree + resultRuneFour + resultRuneFive;
        resultRuneTotal = resultRuneTotal.normalized;
        
        onRuneChangeEvent?.Invoke();

        #region Connections

        resultTotalPower = 0f;
        if (runeOne != null && runeTwo != null)
        {
            float newPower = (runeOne.GetPower() + runeTwo.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeTwo.GetSigilDirection());
            connectionR1R2.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR1R2.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeThree != null)
        {
            float newPower = (runeOne.GetPower() + runeThree.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeThree.GetSigilDirection());
            connectionR1R3.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR1R3.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeFour != null)
        {
            float newPower = (runeOne.GetPower() + runeFour.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR1R4.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR1R4.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeFive != null)
        {
            float newPower = (runeOne.GetPower() + runeFive.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR1R5.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR1R5.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeThree != null)
        {
            float newPower = (runeTwo.GetPower() + runeThree.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeTwo.GetSigilDirection(), runeThree.GetSigilDirection());
            connectionR2R3.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR2R3.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeFour != null)
        {
            float newPower = (runeTwo.GetPower() + runeFour.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeTwo.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR2R4.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR2R4.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeFive != null)
        {
            float newPower = (runeTwo.GetPower() + runeFive.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeTwo.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR2R5.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR2R5.UpdateConnection(0,0);
        }
        
        if (runeThree != null && runeFour != null)
        {
            float newPower = (runeThree.GetPower() + runeFour.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeThree.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR3R4.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR3R4.UpdateConnection(0,0);
        }
        
        if (runeThree != null && runeFive != null)
        {
            float newPower = (runeThree.GetPower() + runeFive.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeThree.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR3R5.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR3R5.UpdateConnection(0,0);
        }
        
        if (runeFour != null && runeFive != null)
        {
            float newPower = (runeFour.GetPower() + runeFive.GetPower()) / 2;
            float newPotencies = Vector2.Dot(runeFour.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR4R5.UpdateConnection(newPower, newPotencies);
        }
        else
        {
            connectionR4R5.UpdateConnection(0,0);
        }
        
        resultTotalPower += connectionR1R2.GetPower();
        resultTotalPower += connectionR1R3.GetPower();
        resultTotalPower += connectionR1R4.GetPower();
        resultTotalPower += connectionR1R5.GetPower();
        resultTotalPower += connectionR2R3.GetPower();
        resultTotalPower += connectionR2R4.GetPower();
        resultTotalPower += connectionR2R5.GetPower();
        resultTotalPower += connectionR3R4.GetPower();
        resultTotalPower += connectionR3R5.GetPower();
        resultTotalPower += connectionR4R5.GetPower();
        
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
