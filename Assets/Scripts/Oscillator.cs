using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);  // todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved
    [SerializeField] float period = 2f;

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;		
	}
	
	// Update is called once per frame
	void Update () {
        // setMovementFactor

        float cycles = 0;
        if (period != 0)        
        {
            cycles = Time.time / period;  // Time.time grows continually from 0
        }
        const float tau = Mathf.PI * 2;  // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to 1
        print(rawSinWave);
        movementFactor = rawSinWave / 2f + .5f;
        transform.position = startingPos + (movementVector * movementFactor);
	}
}
