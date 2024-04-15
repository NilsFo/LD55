using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummoningCircleBehaviourScript : MonoBehaviour
{
    public UnityEvent onRuneChangeEvent;
    public UnityEvent onRuneConnectionChangeEvent;
    public UnityEvent onRuneLineActivation;
    public UnityEvent onRuneLineActivationEnding;
    
    [Header("Hookup")] 
    public GameState gameState;
    
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

    public RuneBehaviourScript runeBehaviourOne;
    public RuneBehaviourScript runeBehaviourTwo;
    public RuneBehaviourScript runeBehaviourThree;
    public RuneBehaviourScript runeBehaviourFour;
    public RuneBehaviourScript runeBehaviourFive;
    
    [Header("Card Live Pointer")]
    public PlayingCardBehaviour runeOne;
    public PlayingCardBehaviour runeTwo;
    public PlayingCardBehaviour runeThree;
    public PlayingCardBehaviour runeFour;
    public PlayingCardBehaviour runeFive;

    [Header("Cardseffect")] 
    public float powerModDaemon = 2f;
    public float powerModSigil = 2f;
    
    [Header("PowerUp Animation")] 
    public float stepTimer = 3f;
    public bool isPlaying = false;
    public float _currentTimer = 0f;
    public int _currentAnimationState = 0;

    public float animationResult = 0f;
    
    [Header("Stats")]
    public Vector2 resultRuneOne;
    public Vector2 resultRuneTwo;
    public Vector2 resultRuneThree;
    public Vector2 resultRuneFour;
    public Vector2 resultRuneFive;

    public Vector2 resultRuneTotal;

    public float resultTotalPower = 0f;

    public float resultMod = 0f;

    public float Value => (float) Math.Round(resultMod * resultTotalPower, 0);

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
        
        if (onRuneChangeEvent == null)
            onRuneChangeEvent = new UnityEvent();
        if (onRuneConnectionChangeEvent == null)
            onRuneConnectionChangeEvent = new UnityEvent();
        if (onRuneLineActivationEnding == null)
            onRuneLineActivationEnding = new UnityEvent();
        if (onRuneLineActivation == null)
            onRuneLineActivation = new UnityEvent();
    }

    private void Start()
    {
        gameState.onRoundCalculation.AddListener(PlayAnimation);
        gameState.onRoundEnd.AddListener(ResetAnimation);
        
        onRuneLineActivationEnding.AddListener(() =>
        {
            Invoke("StartNextRound", 7f);
        });
    }

    void StartNextRound() {
        gameState.levelState = GameState.LevelState.EndOfRound;
    }

    void Update()
    {
        if (isPlaying && _currentAnimationState <= 10)
        {
            _currentTimer += Time.deltaTime;
            if (_currentTimer >= stepTimer)
            {
                _currentTimer -= stepTimer;
                _currentAnimationState++;
                if (_currentAnimationState <= 10)
                {
                    UpdateAnimationResult(_currentAnimationState);
                    onRuneLineActivation?.Invoke();
                }
                else
                {
                    UpdateAnimationResult(_currentAnimationState);
                    onRuneLineActivationEnding?.Invoke();
                }
            }
        }
    }

    private void UpdateAnimationResult(int index)
    {
        float deltaPower = animationResult;
        if (index == 1)
        {
            animationResult += connectionR1R2.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR1R2.SetHighlight(deltaPower);
        }
        else if(index == 2)
        {
            animationResult += connectionR2R3.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR2R3.SetHighlight(deltaPower);
        }
        else if(index == 3)
        {
            animationResult += connectionR3R4.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR3R4.SetHighlight(deltaPower);
        }
        else if(index == 4)
        {
            animationResult += connectionR4R5.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR4R5.SetHighlight(deltaPower);
        }
        else if(index == 5)
        {
            animationResult += connectionR1R5.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR1R5.SetHighlight(deltaPower);
        }
        else if(index == 6)
        {
            animationResult += connectionR1R3.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR1R3.SetHighlight(deltaPower);
        }
        else if(index == 7)
        {
            animationResult += connectionR3R5.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR3R5.SetHighlight(deltaPower);
        }
        else if(index == 8)
        {
            animationResult += connectionR2R5.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR2R5.SetHighlight(deltaPower);
        }
        else if(index == 9)
        {
            animationResult += connectionR2R4.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR2R4.SetHighlight(deltaPower);
        }
        else if(index == 10)
        {
            animationResult += connectionR1R4.GetPower();
            deltaPower = animationResult - deltaPower;
            connectionR1R4.SetHighlight(deltaPower);
        }
        else if(index == 11)
        {
            animationResult = Value;
        }
    }
    
    public void PlayAnimation()
    {
        isPlaying = true;
        _currentTimer = 0f;
        _currentAnimationState = 0;
        animationResult = 0f;
        onRuneLineActivation?.Invoke();
    }
    
    public void ResetAnimation()
    {
        isPlaying = false;
        _currentTimer = 0f;
        _currentAnimationState = 0;
        animationResult = 0f;
        
        connectionR1R2.ResetHighlight();
        connectionR1R3.ResetHighlight();
        connectionR1R4.ResetHighlight();
        connectionR1R5.ResetHighlight();
        connectionR2R3.ResetHighlight();
        connectionR2R4.ResetHighlight();
        connectionR2R5.ResetHighlight();
        connectionR3R4.ResetHighlight();
        connectionR3R5.ResetHighlight();
        connectionR4R5.ResetHighlight();
    }
    
    void UpdateStats()
    {
        gameState.giveDoubleDaemon = true;
        if (runeOne != null) ResetRuneEffect(runeOne);
        if (runeTwo != null) ResetRuneEffect(runeTwo);
        if (runeThree != null) ResetRuneEffect(runeThree);
        if (runeFour != null) ResetRuneEffect(runeFour);
        if (runeFive != null) ResetRuneEffect(runeFive);

        if (runeOne != null && runeTwo != null) UpdateRuneEffect(runeOne, runeTwo);
        if (runeOne != null && runeThree != null) UpdateRuneEffect(runeOne, runeThree);
        if (runeOne != null && runeFour != null) UpdateRuneEffect(runeOne, runeFour);
        if (runeOne != null && runeFive != null) UpdateRuneEffect(runeOne, runeFive);
        if (runeTwo != null && runeThree != null) UpdateRuneEffect(runeTwo, runeThree);
        if (runeTwo != null && runeFour != null) UpdateRuneEffect(runeTwo, runeFour);
        if (runeTwo != null && runeFive != null) UpdateRuneEffect(runeTwo, runeFive);
        if (runeThree != null && runeFour != null) UpdateRuneEffect(runeThree, runeFour);
        if (runeThree != null && runeFive != null) UpdateRuneEffect(runeThree, runeFive);
        if (runeFour != null && runeFive != null) UpdateRuneEffect(runeFour, runeFive);
        
        resultRuneOne = Vector2.zero;
        resultRuneTwo = Vector2.zero;
        resultRuneThree = Vector2.zero;
        resultRuneFour = Vector2.zero;
        resultRuneFive = Vector2.zero;
        
        if (runeOne != null)
            resultRuneOne = runeOne.GetSigilDirection() * runeOne.GetPower();
        
        if (runeTwo != null)
            resultRuneTwo = runeTwo.GetSigilDirection() * runeTwo.GetPower();

        if (runeThree != null)
            resultRuneThree = runeThree.GetSigilDirection() * runeThree.GetPower();
        
        if (runeFour != null)
            resultRuneFour = runeFour.GetSigilDirection() * runeFour.GetPower();

        if (runeFive != null) 
            resultRuneFive = runeFive.GetSigilDirection() * runeFive.GetPower();

        
        resultRuneTotal = resultRuneOne + resultRuneTwo + resultRuneThree + resultRuneFour + resultRuneFive;
        resultRuneTotal = resultRuneTotal.normalized;

        resultMod = (Vector2.Dot(gameState.currentLevelSigil.normalized, resultRuneTotal)+1);
        resultMod *= 0.5f; //0->2 => 0-1
        resultMod += 1; //1-2;
        resultMod = (float)(Math.Round(resultMod * 4, MidpointRounding.ToEven) / 4);

        #region Connections

        resultTotalPower = 0f;
        if (runeOne != null && runeTwo != null)
        {
            float newPower = (runeOne.GetPower() + runeTwo.GetPower());
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeTwo.GetSigilDirection());
            connectionR1R2.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR1R2.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeThree != null)
        {
            float newPower = (runeOne.GetPower() + runeThree.GetPower());
            float newPotencies = -Vector2.Dot(runeOne.GetSigilDirection(), runeThree.GetSigilDirection());
            connectionR1R3.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR1R3.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeFour != null)
        {
            float newPower = (runeOne.GetPower() + runeFour.GetPower());
            float newPotencies = -Vector2.Dot(runeOne.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR1R4.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR1R4.UpdateConnection(0,0);
        }
        
        if (runeOne != null && runeFive != null)
        {
            float newPower = (runeOne.GetPower() + runeFive.GetPower());
            float newPotencies = Vector2.Dot(runeOne.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR1R5.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR1R5.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeThree != null)
        {
            float newPower = (runeTwo.GetPower() + runeThree.GetPower());
            float newPotencies = Vector2.Dot(runeTwo.GetSigilDirection(), runeThree.GetSigilDirection());
            connectionR2R3.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR2R3.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeFour != null)
        {
            float newPower = (runeTwo.GetPower() + runeFour.GetPower());
            float newPotencies = -Vector2.Dot(runeTwo.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR2R4.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR2R4.UpdateConnection(0,0);
        }
        
        if (runeTwo != null && runeFive != null)
        {
            float newPower = (runeTwo.GetPower() + runeFive.GetPower());
            float newPotencies = -Vector2.Dot(runeTwo.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR2R5.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR2R5.UpdateConnection(0,0);
        }
        
        if (runeThree != null && runeFour != null)
        {
            float newPower = (runeThree.GetPower() + runeFour.GetPower());
            float newPotencies = Vector2.Dot(runeThree.GetSigilDirection(), runeFour.GetSigilDirection());
            connectionR3R4.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR3R4.UpdateConnection(0,0);
        }
        
        if (runeThree != null && runeFive != null)
        {
            float newPower = (runeThree.GetPower() + runeFive.GetPower());
            float newPotencies = -Vector2.Dot(runeThree.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR3R5.UpdateConnection(newPower, newPotencies + 1);
        }
        else
        {
            connectionR3R5.UpdateConnection(0,0);
        }
        
        if (runeFour != null && runeFive != null)
        {
            float newPower = (runeFour.GetPower() + runeFive.GetPower());
            float newPotencies = Vector2.Dot(runeFour.GetSigilDirection(), runeFive.GetSigilDirection());
            connectionR4R5.UpdateConnection(newPower, newPotencies + 1);
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

        runeBehaviourOne.mySelector.active = runeOne == null;
        runeBehaviourTwo.mySelector.active = runeTwo == null;
        runeBehaviourThree.mySelector.active = runeThree == null;
        runeBehaviourFour.mySelector.active = runeFour == null;
        runeBehaviourFive.mySelector.active = runeFive == null;
        
        onRuneChangeEvent?.Invoke();
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
            ResetRuneEffect(oldCard);
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
            ResetRuneEffect(oldCard);
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
            ResetRuneEffect(oldCard);
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
            ResetRuneEffect(oldCard);
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
            ResetRuneEffect(oldCard);
            runeFive = null;
            if (listRuneFive.Count > 0)
            {
                runeFive = listRuneFive[0];
                listRuneFive.RemoveAt(0);
            }

            UpdateStats();
        }
    }

    public void UpdateRuneEffect(PlayingCardBehaviour r1, PlayingCardBehaviour r2)
    {
        if (r1.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesDoubleDemon)
        {
            gameState.giveDoubleDaemon = true;
        }
        if (r2.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesDoubleDemon)
        {
            gameState.giveDoubleDaemon = true;
        }
        
        if (r1.cardEffect == PlayingCardBehaviour.PlayingCardEffect.ReturnToHand)
        {
            r2.returnToHand = true;
        }
        if (r2.cardEffect == PlayingCardBehaviour.PlayingCardEffect.ReturnToHand)
        {
            r1.returnToHand = true;
        }
        
        if (r1.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesAdjacentDemonDoublePoints && r2.isDaemon)
        {
            r2.powerMod = powerModDaemon;
        }
        if (r2.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesAdjacentDemonDoublePoints && r1.isDaemon)
        {
            r1.powerMod = powerModDaemon;
        }
        
        float potency = Vector2.Dot(r1.GetSigilDirection(), r2.GetSigilDirection());
        if (potency >= 0 && r1.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesAdjacentSigilDoublePoints)
        {
            r2.powerMod = powerModSigil;
        }
        if (potency >= 0 && r2.cardEffect == PlayingCardBehaviour.PlayingCardEffect.GivesAdjacentSigilDoublePoints)
        {
            r1.powerMod = powerModSigil;
        }
    }

    public void ResetRuneEffect(PlayingCardBehaviour r1)
    {
        r1.powerMod = 1f;
        r1.returnToHand = false;
    }
}
