using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

[RequireComponent (typeof(NavMeshAgent))]
/// <summary>
/// State machine for the Worker
/// </summary>
public class Worker : RhythmProducer
{
    private FollowerPoints _followerPoints;
    NPC.Animations npcAnimations;
    [Header("AI Settings")]
    public WorkerStates workerState;
    public enum WorkerStates
    {
        IDLE, WORKING, FOLLOWING, RETURNING,
    }
    public LayerMask grounded;
    [HideInInspector]
    public NavMeshAgent myNavMesh;
    Vector3 origPosition;
    Vector3 targetPosition;
    public float lookSmooth = 1f;
    public bool navOnStart;
    public FollowerPoint followPoint;
    public GameObject workObject;
    public float followingRange = 25f;

    [Header("Voices & Singing")] 
    public AudioClip[] workingSounds;
    public AudioClip[] followingSounds;
    
    void Start()
    {
        _followerPoints = FindObjectOfType<FollowerPoints>();
        myNavMesh = GetComponent<NavMeshAgent>();
        npcAnimations = GetComponentInChildren<NPC.Animations>();
        
        origPosition = transform.position;
        SetWorking();
    }

    void Update()
    {   
        //WORKING -- starting state
        if (workerState == WorkerStates.WORKING)
        {
            //looks at targetPos when not waving 
            LookAtObject(workObject.transform.position, false);

            //when hit beat, play work tune
            if (showRhythm)
            {
                //play work sound
                PlayRandomSoundRandomPitch(workingSounds, 1f);
                
                //get dist from player
                float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        
                //follow the player!
                if (distFromPlayer < followingRange)
                {
                    SetFollowing();
                }
                
                showRhythm = false;
            }
        }
        
        //IDLE -- state where worker can return to work or keep following 
        if (workerState == WorkerStates.IDLE)
        {
            //looks at targetPos when not waving 
            LookAtObject(targetPosition, false);

            //when hit beat, return to work...
            if (showRhythm)
            {
                //get dist from player
                float distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        
                //time to return to work :'(
                if (distFromPlayer > followingRange)
                {
                    NavigateTo(origPosition, WorkerStates.RETURNING);
                    
                    //reset follow point 
                    followPoint.SetOccupied(false);
                    followPoint = null;
                }
                //keep following!
                else
                {
                    NavigateTo(followPoint.transform.position,  WorkerStates.FOLLOWING);
                }
                

                showRhythm = false;
            }
        }
        
        //RETURNING -- if you leave them behind, worker returns to work.
        if (workerState == WorkerStates.RETURNING)
        {
            //stop running after we are close to position
            if (Vector3.Distance(transform.position, targetPosition) < myNavMesh.stoppingDistance + 1f)
            {
                Debug.Log(gameObject.name + " returned to work");
                SetWorking();
            }
        }

        //FOLLOWING -- when following the player and 'singing'
        if (workerState == WorkerStates.FOLLOWING)
        {
            //looks at targetPos when not waving 
            LookAtObject(player.transform.position, false);

            //on beat, play sound & check our distance from target and what should be my next move
            if (showRhythm)
            {
                //play following sound
                PlayRandomSound(followingSounds, 1f);
                
                //get dist
                float distFromPoint = Vector3.Distance(transform.position, followPoint.transform.position);

                //set idle, i am too far from the music
                if (distFromPoint > followingRange)
                {
                    SetIdle();
                }
                //keep following my point
                else
                {
                    Debug.Log("still following!");
                    //nav
                    NavigateTo(followPoint.transform.position, WorkerStates.FOLLOWING);
                }

                showRhythm = false;
            }
        }
    }

    //called from within working to follow player
    public void SetFollowing()
    {
        //get follow point
        followPoint = _followerPoints.FindValidPoint();
        //navigate!
        NavigateTo(followPoint.transform.position, WorkerStates.FOLLOWING);
        //set occupied
        followPoint.SetOccupied(true);
    }

    //stops movement
    public void SetIdle()
    {
        //stop nav mesh
        myNavMesh.isStopped = true;
        //anim
        if(npcAnimations)
            npcAnimations.SetAnimator("idle");
        //set state
        workerState = WorkerStates.IDLE;
        Debug.Log(gameObject.name + " is Idling...");
    }

    //set back to work
    public void SetWorking()
    {
        //stop nav mesh
        myNavMesh.isStopped = true;
        //anim
        if(npcAnimations)
            npcAnimations.SetAnimator("working");
        //set state
        workerState = WorkerStates.WORKING;
    }

    //base function for actually navigating to a point 
    public void NavigateTo(Vector3 castPoint, WorkerStates newState)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(castPoint, Vector3.down, out hit, Mathf.Infinity, grounded))
        {
            targetPosition = hit.point;
        }
        else
        {
            //try up
            if (Physics.Raycast(castPoint, Vector3.up, out hit, Mathf.Infinity, grounded))
            {
                targetPosition = hit.point;
            }
        }

        //start moving nav mesh
        myNavMesh.SetDestination(targetPosition);
        myNavMesh.isStopped = false;
        
        //set animation
        if(npcAnimations)
            npcAnimations.SetAnimator("moving");
        
        //set state
        workerState = newState;
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
