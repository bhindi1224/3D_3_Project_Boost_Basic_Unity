﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 15f;
    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}


    private void Thrust()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }
    }
    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        float rotationDirection = 0f;
        rigidBody.freezeRotation = true; // take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            rotationDirection++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotationDirection--;
        }

        transform.Rotate(Vector3.forward * rotationDirection * rotationThisFrame);
        rigidBody.freezeRotation = false; // resume physics (return control)
    }
}
