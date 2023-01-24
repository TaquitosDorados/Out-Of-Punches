using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScript : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public Text Decision;
    private AudioSource AudioSource;
    public AudioClip victoryClip;
    public AudioClip defeatClip;
    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {

        if(PlayerPrefs.GetInt("Result") == 0)
        {
            defeatScreen.SetActive(true);
            AudioSource.clip = defeatClip;
            AudioSource.Play();
        } else if(PlayerPrefs.GetInt("Result") == 1)
        {
            victoryScreen.SetActive(true);
            AudioSource.clip = victoryClip;
            AudioSource.Play();
        } else if(PlayerPrefs.GetInt("Result") == 2)
        {
            Decision.text = "Derrota por Decision";
            Decision.gameObject.SetActive(true);
            defeatScreen.SetActive(true);
            AudioSource.clip = defeatClip;
            AudioSource.Play();
        } else
        {
            Decision.text = "Victoria por Decision";
            Decision.gameObject.SetActive(true);
            victoryScreen.SetActive(true);
            AudioSource.clip = victoryClip;
            AudioSource.Play();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void NextFight()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
