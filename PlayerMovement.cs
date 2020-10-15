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

    // 2D vector to store input value
    private Vector2 inputVector;

    [SerializeField]
    private float moveSpeed = 10f; // The speed at which you would move horizontally on the ground

    [SerializeField]
    private float gravity = -9.85f; // The acceleration due to gravity

    [SerializeField]
    private float jumpHeight = 1.5f; // Your Jump Height

    // rotation of the head on z-axis
    private float pitch;
    [SerializeField]
    private float mouseSensitivity = 100f;

    // rotation of the body on y-axis
    private float yaw;

    [SerializeField]
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
        // CAMERA MOVEMENT
        // ---------------
        // Time.deltaTime adjusts the yaw to the fps
        yaw = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(new Vector3(0, yaw, 0)); // rotating the body on y axis

        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Increasing the pitch of the camera
        // using '=' instead of '-=' produces a bug where your pitch resets on every frame
        pitch = Mathf.Clamp(pitch, -60f, 60f); // limiting the pitch to -60deg to +60deg
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0); // rotating the camera

        // JUMPING
        // --------
        

        if (!cc.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // downward acceleration during fall
        } else if (cc.isGrounded){
            verticalVelocity = -2f; // stationary vertical velocity while grounded
        }

        if (Input.GetButton("Jump") && cc.isGrounded)
        {
            verticalVelocity = 15f * Time.deltaTime;
        }

        inputVector.x = Input.GetAxis("Vertical"); // forward movement input
        inputVector.y = Input.GetAxis("Horizontal"); // lateral movement input

        // Normalizing the vector to adjust the diagonal movement speed
        inputVector = inputVector.normalized;

        movement = new Vector3(inputVector.y * moveSpeed * Time.deltaTime, verticalVelocity, inputVector.x * moveSpeed * Time.deltaTime); // setting the movement constraints
        movement = transform.rotation * movement; // finding the resultant of rotation and movement

        cc.Move(movement); // moving the body
    }
}
