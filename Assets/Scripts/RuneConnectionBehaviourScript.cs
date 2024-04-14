using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuneConnectionBehaviourScript : MonoBehaviour
{
    public GameObject runeOne;
    public GameObject runeTwo;
    
    public TMP_Text text;
    
    public float power;
    public float potencies;
    
    
    public void UpdateConnection(float newPower, float newPotencies)
    {
        potencies = newPotencies;
        power = newPower;
        
        text.text = power.ToString();
    }

    private void FixedUpdate()
    {
        Color color = Color.white;
        if(potencies > 0) color = Color.red;
        if(potencies < 0) color = Color.blue;
        Debug.DrawLine(
            runeOne.transform.position,
            runeTwo.transform.position, 
            color);
    }
}
