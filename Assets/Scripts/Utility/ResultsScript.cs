using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScript : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject defeatScreen;
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
        } else
        {
            victoryScreen.SetActive(true);
            AudioSource.clip = victoryClip;
            AudioSource.Play();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
