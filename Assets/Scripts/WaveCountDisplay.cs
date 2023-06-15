using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class WaveCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI waveCountText;

    private void Start()
    {
        // Load the highest wave score from PlayerPrefs
        int highestWave = PlayerPrefs.GetInt("HighestWave", 0);

        waveCountText.text = "Highest score so far: " + highestWave;
    }
}
