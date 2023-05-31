using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    

  
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            StartGameScene();
        }
         
    }

    public void StartGameScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // If the next scene index is greater than the number of scenes, loop back to the first scene
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
