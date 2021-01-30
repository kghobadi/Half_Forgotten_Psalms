using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charController;

    private Vector3 movement;
    public float walkSpeed;
    
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveInputs();
    }

    void MoveInputs()
    {
        float moveForwardBackward = Input.GetAxis("Vertical") * walkSpeed;
        float moveLeftRight = Input.GetAxis("Horizontal") * walkSpeed;

        movement = new Vector3(moveLeftRight, 0, moveForwardBackward);

        charController.Move(movement * Time.deltaTime);
    }
}
