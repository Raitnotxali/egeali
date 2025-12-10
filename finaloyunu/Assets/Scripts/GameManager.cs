using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("💰 Genel Ekonomi")]
    public int currentMoney = 0;
    public TextMeshProUGUI moneyText;

    [Header("⏳ Zaman Sistemi")]
    public float kalanSure = 600f; // 10 Dakika (Saniye cinsinden)
    public bool isTimeRunning = true;

    [Header("📝 Writing Game Ayarları")]
    public int writingCurrentCount = 0;
    public int writingDailyGoal = 10; // Telefonun aradığı hedef
    public int writingMoneyReward = 100;
    public float writingSpeedPenalty = 0f;

    [Header("📂 Matching Game Ayarları")]
    public int matchingCurrentCount = 0;
    public int matchingDailyGoal = 10; // Telefonun aradığı hedef
    public int matchingMoneyReward = 150;

    [Header("💻 Hack Game Ayarları")]
    public int hackCurrentCount = 0;
    public int hackMoneyReward = 200;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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