using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Controls the behavior of the Boss 
/// </summary>
public class Boss : RhythmProducer
{
    private CameraManager _cameraManager;
    private NPC.Animations _animations;
    
    [Header("Boss AI behavior")]
    public List<Worker> nearbyWorkers = new List<Worker>();
    public int workersNecessary = 20;
    public int conflictBeats = 0;
    public int beatsNec = 8;
    public GameCamera endingShot;
    public FadeUI victory;
    public bool hasEnded;
    
    [Header("Boss Audio")] 
    public AudioClip []bossSoundsIdle;
    public AudioClip []bossSoundsConflict;
    public ParticleSystem moneyBags;
    public ParticleSystem conflictSymbols;

    private void Start()
    {
        _animations = GetComponentInChildren<NPC.Animations>();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        if (showRhythm && !hasEnded)
        {
            //conflict
            if (nearbyWorkers.Count > 0)
            {
                //conflict sounds
                PlayRandomSound(bossSoundsConflict, 1f);
                //conflict symbols
                conflictSymbols.Play();
                
                //look at player
                transform.LookAt(player.transform.position, Vector3.up);

                //not enough workers
                if (nearbyWorkers.Count < workersNecessary)
                {
                    //random amount 
                    int randomMice = UnityEngine.Random.Range(0, 3);
                    //make random # of mice return to work!
                    for (int i = 0; i < randomMice; i++)
                    {
                        nearbyWorkers[i].SetReturning();
                    }
                }
                //enough workers, countdown til victory!!!
                else
                {
                    //inc beats
                    conflictBeats++;
                    //enough to trigger ending?
                    if (conflictBeats > beatsNec)
                    {
                        TriggerEnding();
                    }
                }
                
                Debug.Log("Conflict");
            }
            //idle
            else
            {
                PlayRandomSound(bossSoundsIdle, 1f);

                conflictBeats = 0;
                
                //making da money boss
                moneyBags.Play();
            }

            showRhythm = false;
        }
    }

    //add workers as they enter boss trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            Worker worker = other.gameObject.GetComponent<Worker>();

            if (nearbyWorkers.Contains(worker) == false)
            {
                nearbyWorkers.Add(worker);
                Debug.Log("cat sensed " + worker);
            }
        }
    }

    //remove workers as they leave boss trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Worker")
        {
            Worker worker = other.gameObject.GetComponent<Worker>();

            if (nearbyWorkers.Contains(worker))
            {
                nearbyWorkers.Remove(worker);
            }
        }
    }

    public void TriggerEnding()
    {
        //camera transition
        _cameraManager.Set(endingShot);
        //roll that cat!
        _animations.SetAnimator("victory");
        //victory text
        victory.FadeIn();

        //make the workers dance!!!
        for (int i = 0; i < nearbyWorkers.Count; i++)
        {
            nearbyWorkers[i].SetDancing();
        }

        hasEnded = true;
        Debug.Log("Ending!");
    }
}
