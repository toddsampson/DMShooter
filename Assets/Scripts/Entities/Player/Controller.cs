using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Controller : MonoBehaviour
{
    private Movement movement;
    private PlayerCamera playerCam;

    public void Start()
    {
        movement = gameObject.GetComponent<Movement>();
        playerCam = gameObject.transform.GetComponentInChildren<PlayerCamera>();
    }

    public void Update()
    {
        ProcessInputs();
    }

    public void FixedUpdate()
    {
        ProcessMovement();
    }

    /// <summary>
    /// Check for key presses from 
    /// </summary>
    private void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();   
        }
    }

    private void ProcessMovement()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        movement.Rotate(mouseX, 0.0f);
        playerCam.Rotate(mouseX, mouseY);
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        movement.Move(moveX, moveY);
    }
}