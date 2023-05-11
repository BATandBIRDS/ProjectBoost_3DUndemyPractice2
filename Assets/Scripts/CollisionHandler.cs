using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2.0f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem finishParticle;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisable = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLvl();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable) {return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            //case "Fuel":
            //    GetFuel();
            //    break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        crashParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        Invoke("Die", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        finishParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLvl", levelLoadDelay);
    }

    private void LoadNextLvl()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if(nextScene == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    //private void GetFuel()
    //{
    //    throw new NotImplementedException();
    //}

    private void Die()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
