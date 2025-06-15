using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{
    public string sceneToLoad = "GameScene"; 

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

