using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class HackManager : MonoBehaviour
{
    public AudioSource wronganswer;
    public AudioSource trueanswer;

    [Header("UI Referansları")]
    public List<HackSlot> topSlots;
    public List<HackSlot> bottomSlots;
    public Button hackButton;

    // 🔥 Kapanacak olan ana pencere
    public GameObject hackWindow;

    [Header("Sonuç Görselleri (Üst Kısım)")]
    public Sprite spriteGreen;
    public Sprite spriteYellow;
    public Sprite spriteRed;
    public Sprite spriteTopDefault;

    [Header("Giriş Görselleri (Alt Kısım)")]
    public Sprite spriteBottomDefault;

    private int[] secretCode = new int[4];

    // ✅ YENİ EKLENEN KISIM: Pencere her açıldığında burası çalışır
    void OnEnable()
    {
        // UI listeleri dolu mu diye kontrol et (Hata vermemesi için)
        if (topSlots != null && topSlots.Count > 0)
        {
            StartNewHack();
        }
    }

    void Start()
    {
        // Buton bağlantıları sadece 1 kere yapılsa yeterlidir
        if (hackButton != null)
        {
            hackButton.onClick.RemoveAllListeners();
            hackButton.onClick.AddListener(CheckPassword);
        }

        if (hackWindow == null)
        {
            hackWindow = gameObject;
        }

        // StartNewHack()'i buradan sildik, OnEnable'a taşıdık.
    }

    public void StartNewHack()
    {
        GenerateSecretCode();

        // Slotları sıfırla
        foreach (var slot in topSlots) slot.ResetSlot(false, spriteTopDefault);
        foreach (var slot in bottomSlots) slot.ResetSlot(true, spriteBottomDefault);

        Debug.Log($"YENİ ŞİFRE OLUŞTURULDU: {secretCode[0]}{secretCode[1]}{secretCode[2]}{secretCode[3]}");
    }

    void GenerateSecretCode()
    {
        List<int> numbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            secretCode[i] = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);
        }
    }

    void CheckPassword()
    {
        if (topSlots == null || topSlots.Count < 4 || bottomSlots == null || bottomSlots.Count < 4) return;

        int[] userGuess = new int[4];

        for (int i = 0; i < 4; i++)
        {
            int digit = bottomSlots[i].GetDigit();
            if (digit == -1)
            {
                Debug.Log("Eksik sayı var!");
                return;
            }
            userGuess[i] = digit;
        }

        foreach (var slot in topSlots) slot.SetSprite(spriteTopDefault);

        bool[] secretUsed = new bool[4];
        bool[] guessUsed = new bool[4];

        int correctCount = 0;

        // 1. YEŞİLLERİ BUL
        for (int i = 0; i < 4; i++)
        {
            if (userGuess[i] == secretCode[i])
            {
                topSlots[i].SetSprite(spriteGreen);
                secretUsed[i] = true;
                guessUsed[i] = true;
                correctCount++;
                trueanswer.Play();
            }
        }

        // --- KAZANMA DURUMU ---
        if (correctCount == 4)
        {
            Debug.Log("SİSTEM HACKLENDİ! PENCERE KAPANIYOR...");

            if (GameManager.Instance != null) GameManager.Instance.CompleteHackTask();

            // Pencereyi Kapat
            if (hackWindow != null)
            {
                hackWindow.SetActive(false);
            }
            return;
        }

        // 2. SARILARI BUL
        for (int i = 0; i < 4; i++)
        {
            if (guessUsed[i]) continue;

            for (int j = 0; j < 4; j++)
            {
                if (!secretUsed[j] && userGuess[i] == secretCode[j])
                {
                    topSlots[i].SetSprite(spriteYellow);
                    secretUsed[j] = true;
                    guessUsed[i] = true;
                    break;
                }
            }
        }

        // 3. KIRMIZILARI BUL
        for (int i = 0; i < 4; i++)
        {
            if (!guessUsed[i])
            {
                topSlots[i].SetSprite(spriteRed);
                wronganswer.Play();
            }
        }
    }
}