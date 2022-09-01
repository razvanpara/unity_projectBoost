using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float crashDelay = 2F;
    [SerializeField] float nextLevelDelay = 2F;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip levelCompletedSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem levelCompletedParticles;
    private AudioSource aSrc;

    private bool isTransitioning;

    void Start()
    {
        aSrc = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly collision");
                break;
            case "Finish":
                StartSuccessSequence(nextLevelDelay);
                break;
            default:
                StartCrashSequence(crashDelay);
                break;
        }
    }

    void StartCrashSequence(float delay)
    {
        crashParticles.Play();
        isTransitioning = true;
        aSrc.Stop();
        aSrc.PlayOneShot(crashSound);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delay);
    }
    void StartSuccessSequence(float delay)
    {
        levelCompletedParticles.Play();
        isTransitioning = true;
        aSrc.Stop();
        aSrc.PlayOneShot(levelCompletedSound);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delay);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log($"{SceneManager.sceneCountInBuildSettings} {nextSceneIndex}");
        if (SceneManager.sceneCountInBuildSettings <= nextSceneIndex)
            nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }
}
