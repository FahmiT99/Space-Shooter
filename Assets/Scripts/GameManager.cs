using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public AudioSource musicSource;
    public float fadeDuration = 1f;

    private float currentVolume;
    private float startVolume;

    private void Start()
    {
 
        musicSource.Play();
        startVolume = musicSource.volume;
        currentVolume = startVolume;
    }
   
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutMusic());
    }

    private IEnumerator FadeOutMusic()
    {
        while (musicSource.volume > 0)
        {
            float fadeAmount = currentVolume * Time.deltaTime / fadeDuration;
            musicSource.volume -= fadeAmount;
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    public void StartMainMenuDelay()
    {
        StartCoroutine(LoadMainMenuDelayed());
    }

    private IEnumerator LoadMainMenuDelayed()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        SceneManager.LoadScene("Main Menu");
    }
}
