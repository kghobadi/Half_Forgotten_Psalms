using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character Movement")]
    private CharacterController charController;
    private Vector3 movement;
    public float walkSpeed;

    [Header("Camera Movement")] 
    public CinemachineVirtualCameraBase cvCamera;

    private Camera mainCam;
    
    void Start()
    {
        charController = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    void Update()
    {
        MoveInputs();
    }

    void MoveInputs()
    {
        //set character rotation according to camera
        transform.rotation = Quaternion.LookRotation(mainCam.transform.forward, Vector3.up);
        
        //get input * walk speed
        float moveForwardBackward = Input.GetAxis("Vertical") * walkSpeed;
        float moveLeftRight = Input.GetAxis("Horizontal") * walkSpeed;

        //multiply input/speed * direction
        Vector3 fwd = transform.forward * moveForwardBackward;
        Vector3 side = transform.right * moveLeftRight;
        Vector3 grav = Vector3.up * -9.81f;
        //add the vectors together
        movement = fwd + side + grav;
        
        //actually move char 
        charController.Move(movement * Time.deltaTime );
    }

    void CameraInputs()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");
    }
}
