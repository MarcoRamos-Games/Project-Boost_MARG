using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 10f;
    [SerializeField] float rotationForce = 10f;
    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Thrusting();
    }

    private void Thrusting()
    {
        float thrustingForce = thrustForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            myRigidbody.AddRelativeForce(Vector3.up * thrustingForce);
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play();
            }
        }
        else
        {
            myAudioSource.Stop();
        }
    }

    private void Rotate()
    {
        myRigidbody.freezeRotation = true; //Take manual control of rotation
        float rotationSpeed = rotationForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        myRigidbody.freezeRotation = false; //Resume physics control of rotation
    }

   
}
