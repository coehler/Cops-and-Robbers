﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMouseLookController : MonoBehaviour
{

    private Vector2 smoothV;
    private GameObject character;
    private Vector3 parentLastPos;
    private float headbobSpeed = 1.0f;
    private float yRotationV = 0.0f;
    private float xRotationV = 0.0f;

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    [HideInInspector]
    public float headbobStepCounter = 0.0f;
    [HideInInspector]
    public Vector2 mouseLook;
    
    // Initialize Controller.
    void Start(){

        character = transform.parent.gameObject;
        parentLastPos = transform.parent.position;

    }

    // Update is called once per frame.
    void Update(){

        headbobStepCounter += Vector3.Distance(parentLastPos, transform.parent.position) * headbobSpeed;

        parentLastPos = transform.parent.position;

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

    }
}
