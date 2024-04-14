using System;
using TMPro;
using UnityEngine;

public class RuneConnectionBehaviourScript : MonoBehaviour
{
    public GameObject runeOne;
    public GameObject runeTwo;
    
    public TMP_Text text;

    public LineRenderer lineRenderer;
    
    public Color color = Color.red;
    
    public float power;
    public float potencie;

    private void Start()
    {
        Vector3 midPoint = (runeOne.transform.position + runeTwo.transform.position) / 2;
        transform.position = midPoint;

        Vector3[] positions = new Vector3[2];
        positions[0] = runeOne.transform.position;
        positions[1] = runeTwo.transform.position;

        lineRenderer.SetPositions(positions);
        UpdateColor();

        text.text = "";
    }

    public void UpdateConnection(float newPower, float newpotencie)
    {
        potencie = newpotencie;
        power = newPower;
        
        //text.text = GetPower().ToString();
    }

    public float GetPower()
    {
        return (float) Math.Round(power * potencie, 2);
    }

    public void UpdateColor()
    {
        // Blend color from red at 0% to blue at 100%
        var colors = new GradientColorKey[2];
        colors[0] = new GradientColorKey(color, 1.0f);
        colors[1] = new GradientColorKey(color, 1.0f);

        // Blend alpha from opaque at 0% to transparent at 100%
        var alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        lineRenderer.colorGradient.SetKeys(colors, alphas);
    }

    public void SetHighlight(float deltaPower)
    {
        text.text = "+"+deltaPower;
    }

    public void ResetHighlight()
    {
        text.text = "";
    }
}
