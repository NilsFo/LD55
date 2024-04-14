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
        transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);;

        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(runeOne.transform.position.x, runeOne.transform.position.y - 0.5f, runeOne.transform.position.z);
        positions[1] = new Vector3(runeTwo.transform.position.x, runeTwo.transform.position.y - 0.5f, runeTwo.transform.position.z);

        lineRenderer.SetPositions(positions);
        UpdateColor();

        text.text = "";
    }

    public void UpdateConnection(float newPower, float newpotencie)
    {
        potencie = newpotencie;
        power = newPower;
        
        text.text = GetPower().ToString();
        text.color = Color.white;
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
        text.text = "+"+(float) Math.Round(deltaPower, 2);
        text.color = Color.green;
    }

    public void ResetHighlight()
    {
        text.text = "";
    }
}
