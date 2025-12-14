using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class easteregg : MonoBehaviour
{
    public AudioSource tablo1ses;
    public AudioSource tablo2ses;
    public AudioSource tablo3ses;
    public AudioSource tablo4ses;
    public AudioSource tablo5ses;
    public Button tablo1buton;
    public Button tablo2buton;
    public Button tablo3buton;
    public Button tablo4buton;
    public Button tablo5buton;
    public bool a = false;
    public bool b = false;
    public bool c = false;
    public bool d = false;
    public bool e = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void tablo1()
    {
        tablo1ses.Play();
        a=true;
        b=false;
        c=false;
        d=false;
        e=false;
        print($"{a},{b},{c},{d},{e}");
    }
    public void tablo2()
    {
        tablo2ses.Play();
        if(a==true && b==false && c==false && d==false && e==false)
        {
            b=true;
            print($"{a},{b},{c},{d},{e}");
        }
        else
        {
            a=false;
            b=false;
            c=false;
            d=false;
            e=false;
            print($"{a},{b},{c},{d},{e}");
        }
    }
    public void tablo3()
    {
        tablo3ses.Play();
        if(a==true && b==true && c==false && d==false && e==false)
        {
            c=true;
            print($"{a},{b},{c},{d},{e}");
        }
        else
        {
            a=false;
            b=false;
            c=false;
            d=false;
            e=false;
            print($"{a},{b},{c},{d},{e}");
        }
    }
    public void tablo4()
    {
        tablo4ses.Play();
        if(a==true && b==true && c==true && d==false && e==false)
        {
            d=true;
            print($"{a},{b},{c},{d},{e}");
        }
        else
        {
            a=false;
            b=false;
            c=false;
            d=false;
            e=false;
            print($"{a},{b},{c},{d},{e}");
        }
    }
    public void tablo5()
    {
        tablo5ses.Play();
        if(a==true && b==true && c==true && d==true && e==false)
        {
            e=true;
            print($"{a},{b},{c},{d},{e}");
            SceneManager.LoadScene("ees");
        }
        else
        {
            a=false;
            b=false;
            c=false;
            d=false;
            e=false;
            print($"{a},{b},{c},{d},{e}");
        }
    }
}
