
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.Audio;
public class FollowerPoints : MonoBehaviour
{
    public FollowerPoint[] allFollowerPoints;
    public AudioMixer mixer;
    public float workerAudioMin, workingAudioMax;
    private void Awake()
    {
        //fetch all follower points in children 
        if (allFollowerPoints.Length == 0)
        {
            allFollowerPoints = GetComponentsInChildren<FollowerPoint>();
        }
    }

    public FollowerPoint FindValidPoint()
    {
        //loop thru follower points looking for unoccupied point
        for (int i = 0; i < allFollowerPoints.Length; i++)
        {
            //found unoccupied point
            if (allFollowerPoints[i].occupied == false)
            {
                return allFollowerPoints[i];
            }
        }
        
        //didn't find a valid point, just return a random one...
        int randomPoint = UnityEngine.Random.Range(0, allFollowerPoints.Length);

        return allFollowerPoints[randomPoint];
    }

    public void IncreaseWorkerVolume()
    {
        float vol = 0;
        mixer.GetFloat("WorkerVol", out vol);
        if(vol < workingAudioMax)
            mixer.SetFloat("WorkerVol", vol + 1f);
    }
    
    public void DecreaseWorkerVolume()
    {
        float vol = 0;
        mixer.GetFloat("WorkerVol", out vol);
        if(vol > workerAudioMin)
            mixer.SetFloat("WorkerVol", vol - 1f);
    }
}
