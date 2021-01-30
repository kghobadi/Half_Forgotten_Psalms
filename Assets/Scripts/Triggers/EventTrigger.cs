using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour {

    public bool canTrigger = true;
    public bool hasTriggered;

    [Tooltip("Any object with one of these tags will activate the trigger.")]
    public string[] triggerTags;
    [Tooltip("Setting a specific object will make only that object Activate this.")]
    public GameObject specificObj;
    [Tooltip(("These events will fire when the trigger Activates"))]
    public UnityEvent[] events;
   

    [Header("Wait times")]
    public bool waits;
    public float waitTime = 5f;

    [Header("If trigger can activate repeatedly")]
    public bool repeats;
    public float resetTime = 5f;
    
    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && canTrigger)
        {
            
            if (specificObj != null)
            {
                if(other.gameObject == specificObj)
                {
                    SetTrigger();
                    return;
                }
            }
            
            for (int i = 0; i < triggerTags.Length; i++)
            {
                if (other.gameObject.tag == triggerTags[i])
                {
                    SetTrigger();
                    return;
                }
            }
        }
    }

    public void SetCanTrigger(bool state)
    {
        canTrigger = true;
    }

    public void SetTrigger()
    {
        if (waits)
        {
            StartCoroutine(WaitToTrigger());
        }
        else
        {
            Activate();
        }

        hasTriggered = true;
    }

    IEnumerator WaitToTrigger()
    {
        yield return new WaitForSeconds(waitTime);

        Activate();
    }

    void Activate()
    {
        //invoke the events
        for (int i = 0; i < events.Length; i++)
        {
            events[i].Invoke();
        }

        //call repeat if necessary 
        if (repeats)
            StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);

        hasTriggered = false;
    }
}
