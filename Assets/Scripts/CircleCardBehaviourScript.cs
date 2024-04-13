using System;
using UnityEngine;

[Obsolete]
public class CircleCardBehaviourScript : MonoBehaviour
{
    public Vector2 power;

    public bool isBurned;
    
    public bool isBloodSoaked;

    public Vector2 CurrentPower
    {
        get { return power; }
    }
}
