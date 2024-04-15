using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    /** Blatantly stolen from:
    https://gamedevbeginner.com/how-to-make-a-light-flicker-in-unity/
    **/

    public Light myLight;
    public float maxInterval = 1f;

    float targetIntensity;
    float lastIntensity;
    float initIntensity;
    float interval;
    float timer;

    public float maxDisplacement = 0.25f;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 origin;

    private void Start()
    {
        origin = transform.position;
        lastPosition = origin;
        initIntensity = myLight.intensity;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            lastIntensity = myLight.intensity;
            targetIntensity = Random.Range(0.8f * initIntensity, 1f * initIntensity);
            timer = 0;
            interval = Random.Range(0, maxInterval);

            targetPosition = origin + Random.insideUnitSphere * maxDisplacement;
            lastPosition = myLight.transform.position;
        }

        myLight.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / interval);
        myLight.transform.position = Vector3.Lerp(lastPosition, targetPosition, timer / interval);
    }
}
