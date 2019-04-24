using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovementController : MonoBehaviour {

    private float forwardInput;
    private float sideInput;
    private Vector3 movementTarget;
    private Rigidbody rb;

    public float standingMoveSpeed = 4.5f;
    public float crouchMoveSpeed = 3.0f;
    public float sprintForwardsMult = 2.0f;
    public float sprintSidewaysMult = 0.5f;
    public KeyCode sprintKeyCode = KeyCode.LeftShift;
    public KeyCode crouchKeyCode = KeyCode.LeftControl;
    public KeyCode jumpKeyCode = KeyCode.Space;

    // Start is called before the first frame update.
    void Start() {

        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame.
    void Update() {

        forwardInput = Input.GetAxis("Vertical");
        sideInput = Input.GetAxis("Horizontal");

        // Get player state from Unity's Input API.

        if (Input.GetKey(crouchKeyCode)) { // The crouch key is being pressed.

            MoveToTarget(forwardInput, sideInput, crouchMoveSpeed); // Move the player character crouching.

        } else if (Input.GetKey(sprintKeyCode)) { // The sprint key is being pressed.

            MoveToTarget(forwardInput * sprintForwardsMult, sideInput * sprintSidewaysMult, standingMoveSpeed); // Move the player character while sprinting.

        } else { // No state key is being pressed, so the player must be standing.

            MoveToTarget(forwardInput, sideInput, standingMoveSpeed); // Move the player character while standing.

        }

    }

    /*
     * Moves the player character based to a target calculated from Input.
     * 
     * @param fi a float value representing forwards motion.
     * @param si a float value representing sideways motion.
     * @author Christopher Oehler.
     */
    void MoveToTarget(float fi, float si, float speed) {
        
        movementTarget = (transform.forward * fi * speed) + (transform.right * si * speed); // Calculate a movement target vector for given forwards and sideways inputs.

        //transform.position += movementTarget * Time.deltaTime;

        rb.MovePosition(rb.transform.position + movementTarget * Time.deltaTime);
    }

}