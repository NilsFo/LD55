using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookPagesLogic : MonoBehaviour
{

    public TMP_Text roundTF;
    public TMP_Text demonNameTF;
    public TMP_Text scoreTF;
    public TMP_Text highscoreTF;
    
    public TMP_Text endScreenScoreTF;
    public TMP_Text endScreenHighscoreTF;
    public TMP_Text endScreenDemons;
    
    public GameObject scoringLayoutHolder;

    private GameState _gameState;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        roundTF.text = "Page: "+_gameState.levelCurrent+"/"+_gameState.levelMax+
                       "\n"+
                       "Captured: "+_gameState.demonCaptureCount;
        demonNameTF.text = _gameState.handGameObject.currentDemonName;
        scoreTF.text = "Score:\n" + _gameState.score;

        endScreenDemons.text = "Demons captured: " + _gameState.demonCaptureCount;
        endScreenHighscoreTF.text = "Highscore: " + _gameState.highScore;
        endScreenScoreTF.text = "Your Score: " + _gameState.score;
        
        highscoreTF.text = "Highscore: " + _gameState.highScore;

        if (_gameState.levelState==GameState.LevelState.Calculating)
        {
            scoringLayoutHolder.SetActive(true);
        }
        else
        {
            scoringLayoutHolder.SetActive(false);
        }
        
    }
}
