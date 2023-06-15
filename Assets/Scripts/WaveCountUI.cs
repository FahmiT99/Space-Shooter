using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCountUI : MonoBehaviour
{
    public TextMeshProUGUI waveCountText;
    public TextMeshProUGUI highestWaveText;

    private int currentWave = 0;
    private int highestWave = 0;
    private string waveCountKey = "WaveCount";
    private string highestWaveKey = "HighestWave";

    private void Start()
    {
        LoadWaveCount();
        LoadHighestWave();
        UpdateWaveCountText();
    //    UpdateHighestWaveText();
    }

    private void LoadWaveCount()
    {
        currentWave = PlayerPrefs.GetInt(waveCountKey, 0);
    }

    private void LoadHighestWave()
    {
        highestWave = PlayerPrefs.GetInt(highestWaveKey, 0);
    }

    private void SaveWaveCount()
    {
        PlayerPrefs.SetInt(waveCountKey, currentWave);
        PlayerPrefs.Save();
    }

    private void SaveHighestWave()
    {
        PlayerPrefs.SetInt(highestWaveKey, highestWave);
        PlayerPrefs.Save();
    }

    private void UpdateWaveCountText()
    {
        waveCountText.text = "Wave: " + currentWave;
    }

   /* private void UpdateHighestWaveText()
    {
        highestWaveText.text = "Highest Wave: " + highestWave;
    }*/

    public void IncreaseWaveCount()
    {
        currentWave++;
        UpdateWaveCountText();
        SaveWaveCount();

        if (currentWave > highestWave)
        {
            highestWave = currentWave;
     //       UpdateHighestWaveText();
            SaveHighestWave();
        }
    }

    public void ResetWaveCount()
    {
        currentWave = 0;
        UpdateWaveCountText();
        SaveWaveCount();
    }
    
    // Other methods...

    public void OnSceneEnding()
    {
        ResetWaveCount();
    }
}
