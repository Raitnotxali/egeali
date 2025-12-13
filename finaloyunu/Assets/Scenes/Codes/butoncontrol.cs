using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class butoncontrol : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject mainmenupanel;
    public GameObject optionspanel;
    public AudioMixer audioMixer;

    void Start()
    {
    float music = PlayerPrefs.GetFloat("MusicVolume", 0);
    float sound = PlayerPrefs.GetFloat("SoundVolume", 0);

    audioMixer.SetFloat("MusicVolume", music);
    audioMixer.SetFloat("SoundVolume", sound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startbutton(){
        SceneManager.LoadScene("0");
    }
    public void optionsbutton(){
        mainmenupanel.SetActive(false);
        optionspanel.SetActive(true);
    }
    public void returnmainmenubutton()
    {
        optionspanel.SetActive(false);
        mainmenupanel.SetActive(true);
    }
    public void quitbutton(){
        Application.Quit();
    }
    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("MusicVolume", dB);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat("SoundVolume", dB);
        PlayerPrefs.SetFloat("SoundVolume", dB);
    }
}
