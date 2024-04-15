using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ESBehaviourScript : MonoBehaviour
{
    
    public string prefix = "Score:";
    
    private GameState _gameState;
    private SummoningCircleBehaviourScript _summonCircle;

    private TMP_Text _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
        _text = GetComponent<TMP_Text>();
        _summonCircle = FindObjectOfType<SummoningCircleBehaviourScript>();
        
        _text.text = prefix + " 0";
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = prefix + " " + _summonCircle.animationResult;
    }
}
