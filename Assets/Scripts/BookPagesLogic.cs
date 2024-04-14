using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookPagesLogic : MonoBehaviour
{

    public TMP_Text roundTF;
    public TMP_Text demonNameTF;

    private GameState _gameState;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        roundTF.text = "Page: "+_gameState.levelCurrent+"/"+_gameState.levelMax;
        demonNameTF.text = _gameState.handGameObject.currentDemonName;
    }
}
