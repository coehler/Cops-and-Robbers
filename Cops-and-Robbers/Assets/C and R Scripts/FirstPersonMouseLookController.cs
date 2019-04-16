using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMouseLookController : MonoBehaviour
{

    Vector2 mouseLook;
    Vector2 smoothV;

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameObject character;

    // Initialize Controller.
    void Start(){
        character = this.transform.parent.gameObject;
    }

    // Update is called once per frame.
    void Update(){
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
