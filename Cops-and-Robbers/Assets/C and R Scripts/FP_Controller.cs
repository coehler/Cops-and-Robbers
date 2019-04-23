using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_Controller : MonoBehaviour {
    public float ForwardSpeed = 20;
    public float SidewaysSpeed = 15F;
    public float MouseSensitivity = 2F;

    Rigidbody PlayerRB;

    Vector3 MovementVector;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerRB = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        float mouseInput = Input.GetAxis("Mouse X") * MouseSensitivity;
        Vector3 lookhere = new Vector3(0, mouseInput, 0);
        transform.Rotate(lookhere);

        MovementVector = Input.GetAxis("Vertical") * ForwardSpeed * transform.forward;
        MovementVector += Input.GetAxis("Horizontal") * SidewaysSpeed * transform.right;

        PlayerRB.velocity.Set(MovementVector.x, PlayerRB.velocity.y, MovementVector.z);

        this.transform.position += MovementVector * Time.deltaTime;
    }

    // Time dependent code here
    private void FixedUpdate() {


    }
}
