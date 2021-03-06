﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 20f;
    [SerializeField] TextMeshProUGUI statusTextField;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip goal;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Scene currentScene = SceneManager.GetActiveScene();
        statusTextField.text = currentScene.name.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
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
            mainEngineParticles.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
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

        rigidBody.angularVelocity = Vector3.zero;  // remove rotation due to physics
        if (Input.GetKey(KeyCode.A))
        {
            rotationDirection++;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotationDirection--;            
        }
        transform.Rotate(Vector3.forward * rotationDirection * rotationThisFrame);
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
                statusTextField.text = "Congratulations!!";
                audioSource.Stop();
                audioSource.PlayOneShot(goal);
                successParticles.Play();
                Invoke("LoadNextLevel", levelLoadDelay);
                break;
            case "Deadly":
                statusTextField.text = "You Crashed!";
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                state = State.Dying;
                Invoke("LoadFirstScene", levelLoadDelay);
                break;
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex;
        if (currentSceneIndex < totalSceneCount - 1 )
        {
            nextSceneIndex = currentSceneIndex + 1;
        }
        else
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
