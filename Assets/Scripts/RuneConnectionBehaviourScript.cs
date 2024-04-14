using TMPro;
using UnityEngine;

public class RuneConnectionBehaviourScript : MonoBehaviour
{
    public GameObject runeOne;
    public GameObject runeTwo;
    
    public TMP_Text text;
    
    public float power;
    public float potencie;

    private void Start()
    {
        Vector3 midPoint = (runeOne.transform.position + runeTwo.transform.position) / 2;
        transform.position = midPoint; 
    }

    public void UpdateConnection(float newPower, float newpotencie)
    {
        potencie = newpotencie;
        power = newPower;
        
        text.text = power.ToString();
    }

    public float GetPower()
    {
        return power + potencie;
    }
    
    private void FixedUpdate()
    {
        Color color = Color.white;
        if(potencie > 0) color = Color.red;
        if(potencie < 0) color = Color.blue;
        Debug.DrawLine(
            runeOne.transform.position,
            runeTwo.transform.position, 
            color);
    }
}
