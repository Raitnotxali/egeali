using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class eastereggscene : MonoBehaviour
{
    public GameObject buton;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public GameObject hikayepanel;
    public GameObject levelpanel;
    public bool delete1 = false;
    public float time=10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         if (PlayerPrefs.GetInt("isLevel2Open", 0) == 1)
        {
            level2Button.interactable = true;
        }
        else
        {
            level2Button.image.color = Color.gray;
        }
            

        if (PlayerPrefs.GetInt("isLevel3Open", 0) == 1)
        {
            level3Button.interactable = true;
        }
        else
        {
            level3Button.image.color = Color.gray;
        }
            

        if (PlayerPrefs.GetInt("isLevel4Open", 0) == 1)
        {
            level4Button.interactable = true;
        }
        else
        {
            level4Button.image.color = Color.gray;
        }
            

        if (PlayerPrefs.GetInt("isLevel5Open", 0) == 1)
        {
          level5Button.interactable = true;  
        }
        else
        {
            level5Button.image.color = Color.gray;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            Debug.Log(Mathf.Ceil(time));
        }

        else
        {
            buton.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            delete1 = true;
            Debug.Log("Delete 1/2");
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && delete1 == true)
        {
            Debug.Log("PLAYERPREFS DELETED");
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && delete1 == true)
            {
                PlayerPrefs.SetInt("isLevel2Open", 1);
                PlayerPrefs.SetInt("isLevel3Open", 1);
                PlayerPrefs.SetInt("isLevel4Open", 1);
                PlayerPrefs.SetInt("isLevel5Open", 1);
                PlayerPrefs.Save();
                SceneManager.LoadScene("SampleScene");
            }
        if(Input.GetKeyDown(KeyCode.Alpha3) && delete1 == true)
        {
            SceneManager.LoadScene("finalscene");
        }
       
    }
    public void butoncalisma()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void butoncalismahikaye()
    {
        hikayepanel.SetActive(false);
        levelpanel.SetActive(true);
    }
    public void level1buton()
    {
        SceneManager.LoadScene("0");
    }
    public void level2buton()
    {
        SceneManager.LoadScene("1");
    }
    public void level3buton()
    {
        SceneManager.LoadScene("2");
    }
    public void level4buton()
    {
        SceneManager.LoadScene("3");
    }
    public void level5buton()
    {
        SceneManager.LoadScene("4");
    }
    
}
