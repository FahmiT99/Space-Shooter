using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

 

public class WaveManager : MonoBehaviour
{
    public int currentWave;
    public int highestScore;
    public TextMeshProUGUI WaveText;
    private GameObject[] Enemies;

    private void Start()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
    }
    private void Update()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        WaveText.text = "Wave " + currentWave;
        UpdateWave();
    }

    public void UpdateWave()
    {    
        // Logic to determine if all enemies in the wave are destroyed
        if(Enemies.Length == 0) 
        {
            Debug.Log("Wave completed");           
            //start next wave

        }

        if (currentWave > highestScore)
        {
            highestScore = currentWave;
            PlayerPrefs.SetInt("HighestScore", highestScore);
            PlayerPrefs.Save();
        }
        
    }
 
}

