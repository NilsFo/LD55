using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorTarget : MonoBehaviour
{
    public bool active=true;
    public bool placeable=true;
    
    public UnityEvent<PlayingCardBehaviour> onSelected;

    // Start is called before the first frame update
    void Start()
    {
        if (onSelected == null)
        {
            onSelected = new UnityEvent<PlayingCardBehaviour>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Select(PlayingCardBehaviour card)
    {
        OnSelected(card);
    }

    private void OnSelected(PlayingCardBehaviour card)
    {
        onSelected.Invoke(card);
    }
    
    
}