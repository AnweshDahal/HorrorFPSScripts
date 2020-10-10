using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // character controller object that handles the movement of the body
    private CharacterController cc;

    // vector to store movement on 3 dimensions forward, lateral, vertical
    private Vector3 movement;

    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private float gravity;

    // rotation of the head on z-axis
    private float pitch;
    [SerializeField]
    private float pitchSpeed = 100f;

    // rotation of the body on y-axis
    private float yaw;
    [SerializeField]
    private float yawSpeed = 100f;

    private float verticalVelocity = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // Automatically assigning the object to character controller
        cc = GetComponent<CharacterController>();

        // Hiding the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Time.deltaTime adjusts the yaw to the fps
        yaw = Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, yaw, 0)); // rotating the body on y axis

        pitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime; // Increasing the pitch of the camera
        // using '=' instead of '-=' produces a bug where your pitch resets on every frame
        pitch = Mathf.Clamp(pitch, -60f, 60f); // limiting the pitch to -60deg to +60deg
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0); // rotating the camera

        if (!cc.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // downward acceleration during fall
        } else if (cc.isGrounded)
        {
            verticalVelocity = 0; // stationary vertical velocity while grounded
        }

        float forwardMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // forward movement input
        float lateralMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // lateral movement input

        movement = new Vector3(lateralMove, verticalVelocity, forwardMove); // setting the movement constraints
        movement = transform.rotation * movement; // finding the resultant of rotation and movement

        cc.Move(movement); // moving the body
    }
}
