using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class finalcode : MonoBehaviour
{
    public AudioSource ses1;
    public AudioSource ses2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ses1.Play();
        ses2.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onclick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
