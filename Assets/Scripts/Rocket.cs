﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 20f;
    [SerializeField] TextMeshProUGUI collisionMessage;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip goal;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

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
        if (state != State.Alive)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(mainEngine);
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
        if (state != State.Alive)
        {
            return;
        }
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
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {            
            case "Goal":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(goal);
                Invoke("LoadNextScene", 2f); // parameterise time
                break;
            case "Deadly":
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                collisionMessage.text = "should be playing death";
                state = State.Dying;
                Invoke("LoadFirstScene", 2f);
                break;
        }
    }

    private void LoadNextScene()
    {      
        SceneManager.LoadScene(1); // todo allow for more than 2 levels
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionMessage.text = "";
    }
}
