using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class scrClickOnObjectInteraction : MonoBehaviour
{
    public UnityEvent eventToTrigger;

    public void Interact()
    {
        print("Interact:" + gameObject.name);
        eventToTrigger.Invoke();
    }
}
