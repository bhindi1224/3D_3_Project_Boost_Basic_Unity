using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 20f;
    [SerializeField] TextMeshProUGUI collisionMessage;
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
            audioSource.Stop();
            return;
        }
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
                Invoke("LoadNextScene", 1f); // parameterise time
                break;
            case "Deadly":
                state = State.Dying;
                collisionMessage.text = collision.gameObject.tag.ToString();
                Invoke("LoadFirstScene", 1f);
                break;
            case "Fuel":
                collisionMessage.text = collision.gameObject.tag.ToString();
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
