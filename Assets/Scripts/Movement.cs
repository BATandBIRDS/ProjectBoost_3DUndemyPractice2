using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    
    bool isAlive;

    [SerializeField] float gas = 1000;
    [SerializeField] float rotateFactor = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem leftEngine;
    [SerializeField] ParticleSystem rightEngine;
    [SerializeField] ParticleSystem midEngine;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProccessThrust();
        ProccessRotation();
    }

    void ProccessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            midEngine.Stop();
            audioSource.Stop();
        }
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * gas * Time.deltaTime);

        if (!midEngine.isPlaying)
        {
            midEngine.Play();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    void RotateLeft()
    {
        ApplyRotation(rotateFactor);

        if (!rightEngine.isPlaying)
        {
            rightEngine.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-rotateFactor);

        if (!leftEngine.isPlaying)
        {
            leftEngine.Play();
        }
    }

    void ProccessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            leftEngine.Stop();
            rightEngine.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
