using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a move towards component which can be attached to anything.
//useful for creating simple AIs which don't use pathfinding 

public class MoveTowards : MonoBehaviour {
    public float moveSpeed;
    [HideInInspector] public float origSpeed;

    public bool moving;
    public Vector3 destination;
    public float necessaryDist = 1f;

    void Start()
    {
        origSpeed = moveSpeed;
    }

    void Update () {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

            if(Vector3.Distance(transform.position, destination) < necessaryDist)
            {
                moving = false;
                moveSpeed = origSpeed;
            }
        }
	}

    public void MoveTo(Vector3 dest, float speed)
    {
        destination = dest;
        moveSpeed = speed;
        moving = true;
    }
}
