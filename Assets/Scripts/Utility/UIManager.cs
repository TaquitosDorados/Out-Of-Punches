using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject StartCanvas;

    public GameObject SettingsCanvas;
    
    public GameObject MenuCanvas;
    
    public Image BlockCanvas;
    
    private bool activar = true;
    
    public AudioMixer[] mixeador;
    public Slider[] mainSlider;
    private static float oldValueSound = 1.0f;
    private static float oldValueMusic = 1.0f;
    private void Start()
    {
        mainSlider[0].value = oldValueMusic;
        mainSlider[1].value = oldValueSound;
    }
    
    public void ChangeCanvas()
    {
        SettingsCanvas.SetActive(activar);
        activar = !activar;
        StartCanvas.SetActive(activar);
    }

    public void ChangeScene(string level)
    {
        SceneManager.LoadScene(level);
        Time.timeScale = 1;
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void Pausar()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            MenuCanvas.SetActive(true);
            BlockCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            BlockCanvas.enabled = false;
            MenuCanvas.SetActive(false);
        }
    }
    
    public void saveValueMusic()
    {
        oldValueMusic = mainSlider[0].value;
    }
    
    public void saveValueSound()
    {
        oldValueSound = mainSlider[0].value;
    }
    
    public void MusicSlider(  float sliderValue)
    { 
        mixeador[0].SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
    
    public void SoundSlider(  float sliderValue)
    {
        mixeador[1].SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);
    }
    
   
}
