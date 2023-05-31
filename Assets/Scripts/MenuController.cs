using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    private int highestScore;
    private void Start()
    {
        // Load the highest score from PlayerPrefs
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        highScoreText.text = "HighScore: " + highestScore;
    }
     
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
