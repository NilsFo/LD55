using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardLogic : MonoBehaviour
{
    [Header("World Hookup")] public PlayingCard playingCardBase;
    public PlayingCard playingCardFallback;
    public Vector3 handWorldPos;

    [Header("Parameters")] public float movementSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (playingCardBase == null)
        {
            playingCardBase = playingCardFallback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, handWorldPos, Time.deltaTime * movementSpeed);
    }
}