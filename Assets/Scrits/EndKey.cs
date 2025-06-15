using UnityEngine;
using UnityEngine.SceneManagement;

public class EndKey : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load. You're at the last scene.");
        }
    }
}