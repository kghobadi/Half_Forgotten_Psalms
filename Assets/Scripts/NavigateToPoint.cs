using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateToPoint : MonoBehaviour
{

    NPC.Animations npcAnimations;
    [Header("AI Movement Settings")]
    public LayerMask grounded;
    [HideInInspector]
    public NavMeshAgent myNavMesh;
    Vector3 origPosition;
    Vector3 targetPosition;

    public bool navOnStart;
    public Transform destination;

    public NavStates navState;
    public enum NavStates
    {
        NAVIGATING, IDLE,
    }

    public float lookSmooth = 1f;

    void Awake()
    {
        myNavMesh = GetComponent<NavMeshAgent>();
        npcAnimations = GetComponentInChildren<NPC.Animations>();
    }

    void Start()
    {
        origPosition = transform.position;

        if (navOnStart)
        {
            NavigateTo(destination.position);
        }
        else
        {
            SetIdle();
        }
    }
    
    void Update()
    {   
        //IDLE
        if (navState == NavStates.IDLE)
        {
            //looks at targetPos when not waving 
            LookAtObject(targetPosition, false);
        }

        //NAVIGATING
        if (navState == NavStates.NAVIGATING)
        {
            //looks at targetPos when not waving 
            LookAtObject(targetPosition, false);

            //stop running after we are close to position
            if (Vector3.Distance(transform.position, targetPosition) < myNavMesh.stoppingDistance + 3f)
            {
                Debug.Log("reach");
                SetIdle();
            }
        }
    }

    //stops movement
    public void SetIdle()
    {
        myNavMesh.isStopped = true;
        if(npcAnimations)
            npcAnimations.SetAnimator("idle");
        navState = NavStates.IDLE;
    }

    //base function for actually navigating to a point 
    public void NavigateTo(Vector3 castPoint)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(castPoint, Vector3.down, out hit, Mathf.Infinity, grounded))
        {
            targetPosition = hit.point;
        }
        else
        {
            if (Physics.Raycast(castPoint, Vector3.up, out hit, Mathf.Infinity, grounded))
            {
                targetPosition = hit.point;
            }
        }

        myNavMesh.SetDestination(targetPosition);

        myNavMesh.isStopped = false;
        
        if(npcAnimations)
            npcAnimations.SetAnimator("moving");
    }

    //looks at object
    void LookAtObject(Vector3 pos, bool useMyY)
    {
        //empty Vector 3
        Vector3 direction;

        //use my y Pos in Look pos
        if (useMyY)
        {
            //find direction from me to obj
            Vector3 posWithMyY = new Vector3(pos.x, transform.position.y, pos.z);
            direction = posWithMyY - transform.position;
        }
        //use obj y pos in Look pos
        else
        {
            //find direction from me to obj
            direction = pos - transform.position;
        }

        //find target look
        Quaternion targetLook = Quaternion.LookRotation(direction);
        //actually rotate the character 
        transform.rotation = Quaternion.Lerp(transform.rotation, targetLook, lookSmooth * Time.deltaTime);
    }
}
