using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class RuneConnectionBehaviourScript : MonoBehaviour
{
    public GameObject runeOne;
    public GameObject runeTwo;
    public ConnectionParticleManager myConnectionParticleManager;
    
    public TMP_Text text;
    
    public Color color = Color.red;

    public Gradient colorGradient;
    
    public float power;
    public float potencie;

    private void Start()
    {
        Vector3 midPoint = (runeOne.transform.position + runeTwo.transform.position) / 2;
        transform.position = new Vector3(midPoint.x, midPoint.y, midPoint.z);;

        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(runeOne.transform.position.x, runeOne.transform.position.y - 0.5f, runeOne.transform.position.z);
        positions[1] = new Vector3(runeTwo.transform.position.x, runeTwo.transform.position.y - 0.5f, runeTwo.transform.position.z);

        UpdateColor();

        text.text = "";
    }

    private void Update()
    {
        myConnectionParticleManager.intensity = potencie / 2;

        UpdateColor();
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
        myConnectionParticleManager.emissionColor = GetColor();
    }

    public void SetHighlight(float deltaPower, Vector3 targetPos, Color color)
    {
        text.text = "+"+(float) Math.Round(deltaPower, 2);
        text.color = color;
        text.enabled = true;
        text.transform.position = transform.position;
        var upTween = text.transform.DOMove(transform.position + Vector3.up, .2f).From(transform.position).SetEase(Ease.OutCubic);
        upTween.OnComplete(() => {
            var toBookTween = text.transform.DOMove(targetPos, 0.4f).SetDelay(0.4f).SetEase(Ease.InOutCubic);
            toBookTween.OnComplete(() => text.enabled = false);
            toBookTween.Play();
        });
        upTween.Play();
    }

    public Color GetColor() {
        return colorGradient.Evaluate(potencie / 2);
    }

    public void ResetHighlight()
    {
        text.text = "";
    }
}
