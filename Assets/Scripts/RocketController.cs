using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    [SerializeField] float thrustForce = 10f;
    [SerializeField] float rotationForce = 10f;
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelEnd;

    [SerializeField] ParticleSystem engineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    bool isCollisonDisabled;
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
            RespondToThrustInput();
            RespondToRotateInput();
           
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebbugKeys();
        }
        
    }

    
    private void OnCollisionEnter(Collision other)
    {
        if(state != State.Alive || isCollisonDisabled)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartSuccesSequence();

                break;
            default:
                //dead
                StartDeathSequence();

                break;
        }
    }

   
    private void StartSuccesSequence()
    {
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(levelEnd);
        successParticle.Play();
        state = State.Trascending;
        Invoke("LoadNextSccene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(deathSound);
        deathParticle.Play();
        state = State.Dying;
        Invoke("Death", levelLoadDelay);
    }


    private void LoadNextSccene()
    {
        
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextSceneToLoad = currentLevel + 1;
       
        if(nextSceneToLoad == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneToLoad = 0;
        }
        SceneManager.LoadScene(nextSceneToLoad);



    }

    private void Death()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);

    }

    private void RespondToThrustInput()
    {
        float thrustingForce = thrustForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustingForce);

        }
        else
        {
            myAudioSource.Stop();
            engineParticle.Stop();
        }

        
    }

    private void ApplyThrust(float thrustingForce)
    {
       
        myRigidbody.AddRelativeForce(Vector3.up * thrustingForce);
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngine);
            
        }
        if (!engineParticle.isPlaying)
        {
            engineParticle.Play();
        }


    }

    private void RespondToRotateInput()
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

   private void RespondToDebbugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int nextSceneToLoad = currentLevel + 1;

            if (nextSceneToLoad == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneToLoad = 0;
            }
            SceneManager.LoadScene(nextSceneToLoad);

        }
        else if (Input.GetKey(KeyCode.C))
        {
            isCollisonDisabled = !isCollisonDisabled;
        }
    }
}
