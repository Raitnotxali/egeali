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
    float sfx = PlayerPrefs.GetFloat("SoundVolume", 0);

    audioMixer.SetFloat("MusicVolume", music);
    audioMixer.SetFloat("SoundVolume", sfx);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startbutton(){
        SceneManager.LoadScene("gamescene");
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
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
