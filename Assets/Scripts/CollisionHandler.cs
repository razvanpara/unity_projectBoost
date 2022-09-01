using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float crashDelay = 2F;
    [SerializeField] float nextLevelDelay = 2F;
    private void OnCollisionEnter(Collision other)
    {
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
        // add SFX for crash
        // add particle effect for crash
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delay);
    }
    void StartSuccessSequence(float delay)
    {
        // add SFX for crash
        // add particle effect for crash
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
