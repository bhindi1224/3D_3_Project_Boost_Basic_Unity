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
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Start":
                collisionMessage.text = collision.gameObject.tag.ToString();
                break;
            case "Goal":
                SceneManager.LoadScene(1);
                break;
            case "Friendly":
                // do nothing
                collisionMessage.text = collision.gameObject.tag.ToString();
                break;
            case "Deadly":
                collisionMessage.text = collision.gameObject.tag.ToString();
                SceneManager.LoadScene(0);
                break;
            case "Fuel":
                collisionMessage.text = collision.gameObject.tag.ToString();
                break;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionMessage.text = "";
    }
}
