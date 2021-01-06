using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 10f;
    [SerializeField] float rotationForce = 10f;
    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    enum State {Alive, Dying, Trascending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Rotate();
            Thrusting();
        }
        
    }

    
    private void OnCollisionEnter(Collision other)
    {
        if(state != State.Alive)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                state = State.Trascending;
                Invoke("LoadNextSccene", 1f);

                break;
            default:
                //dead
                state = State.Dying;
                Invoke("Death", 1f);

                break;
        }
    }

    private  void Death()
    {
        SceneManager.LoadScene(0);
    }

    private  void LoadNextSccene()
    {
        SceneManager.LoadScene(1);
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
