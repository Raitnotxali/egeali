using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Button buton1;
    public int currentlevel = 1;
    public Button buton2;
    public GameObject buton1tick;
    public GameObject buton2tick;
    public GameObject winpanel;
    public bool isplaying=true;
    public GameObject losepanel;
    public GameObject phonescreen;
    public AudioSource losesound;
    public AudioSource winsound;
    public AudioSource winsound2;

    [Header("💰 Genel Ekonomi")]
    
    public int currentMoney = 0;
    public int moneygoal = 2000;
    public TextMeshProUGUI moneyText;

    [Header("⏳ Zaman Sistemi")]
    public float kalanSure = 600f; // 10 Dakika (Saniye cinsinden)
    public bool isTimeRunning = true;

    [Header("📝 Writing Game Ayarları")]
    public int writingCurrentCount = 0;
    public int writingDailyGoal = 5; // Telefonun aradığı hedef
    public int writingMoneyReward = 100;
    public float writingSpeedPenalty = 0f;

    [Header("📂 Matching Game Ayarları")]
    public int matchingCurrentCount = 0;
    public int matchingDailyGoal = 5; // Telefonun aradığı hedef
    public int matchingMoneyReward = 150;

    [Header("💻 Hack Game Ayarları")]
    public int hackCurrentCount = 0;
    public int hackMoneyReward = 200;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Geri Sayım Mantığı
        if (isTimeRunning && kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            if (kalanSure <= 0)
            {
                kalanSure = 0;
                Debug.Log("MESAİ BİTTİ!");
                // İstersen burada günü bitirebiliriz
            }
        }
        if (writingCurrentCount== writingDailyGoal && matchingCurrentCount==matchingDailyGoal && isplaying && currentMoney >= moneygoal)
        {
            winpanel.SetActive(true);
            phonescreen.SetActive(false);
            isplaying=false;
            winsound.Play();
            winsound2.Play();
            Debug.Log("Kazandinkanka");
            currentMoney=0;
            matchingCurrentCount=0;
            writingCurrentCount=0;
            UpdateUI();
            if (currentlevel == 1)
            {
                PlayerPrefs.SetInt("isLevel2Open", 1);
                PlayerPrefs.Save();
            }
            else if (currentlevel == 2)
            {
                PlayerPrefs.SetInt("isLevel3Open", 1);
                PlayerPrefs.Save();
            }
            else if (currentlevel == 3)
            {
                PlayerPrefs.SetInt("isLevel4Open", 1);
                PlayerPrefs.Save();
            }
            else if (currentlevel == 4)
            {
                PlayerPrefs.SetInt("isLevel5Open", 1);
                PlayerPrefs.Save();
            }
            else if (currentlevel == 5)
            {
                // Son seviye tamamlandı
                SceneManager.LoadScene("finalscene");
            }
        }
        if(kalanSure<=0 && isplaying)
        {
            losepanel.SetActive(true);
            phonescreen.SetActive(false);
            isplaying=false;
            losesound.Play();
            Debug.Log("Kaybettinkanka");
        }
        if(writingCurrentCount==writingDailyGoal)
        {
            buton1.interactable=false;
            buton1tick.SetActive(true);
        }
        if(matchingCurrentCount== matchingDailyGoal)
        {
            buton2.interactable=false;
            buton2tick.SetActive(true);
        }
    }

    // --- OYUN FONKSİYONLARI ---
    public void CompleteWritingTask()
    {
        currentMoney += writingMoneyReward;
        writingCurrentCount++;
        UpdateUI();
    }

    public void ApplyEasierMode()
    {
        writingSpeedPenalty += 40f;
    }

    public void CompleteMatchingTask()
    {
        currentMoney += matchingMoneyReward;
        matchingCurrentCount++;
        UpdateUI();
    }

    public void CompleteHackTask()
    {
        currentMoney += hackMoneyReward;
        hackCurrentCount++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$ " + currentMoney.ToString();
        }
    }
}