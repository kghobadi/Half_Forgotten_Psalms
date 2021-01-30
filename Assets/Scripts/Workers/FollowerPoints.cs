
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FollowerPoints : MonoBehaviour
{
    public FollowerPoint[] allFollowerPoints;

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
}
