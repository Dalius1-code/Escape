using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{
    public string sceneToLoad = "GameScene"; // Set this to the name of your game scene

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

