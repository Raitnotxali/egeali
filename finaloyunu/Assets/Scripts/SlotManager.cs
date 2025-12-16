using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public Image[] slotImages;
    public TMP_InputField betInput;
    public Button spinButton;

    [Header("Sonuç Ekranları")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Miktar Yazıları")]
    public TextMeshProUGUI winAmountText;
    public TextMeshProUGUI loseAmountText;

    [Header("Görseller")]
    public Sprite[] slotIcons;

    [Header("Ayarlar")]
    public int minBet = 10;
    public int maxBet = 500;
    public float spinDuration = 1.5f;

    [Header("Şans Ayarı 🎲")]
    [Range(0, 100)]
    [Tooltip("0 ile 100 arası bir değer gir. Örn: 30 yaparsan %30 ihtimalle kazanır.")]
    public int winChancePercentage = 30; // Varsayılan %30

    private int currentBet = 10;
    private bool isSpinning = false;

    void OnEnable()
    {
        if (slotIcons != null && slotIcons.Length > 0)
            ShuffleSlotsInstant();

        UpdateBetUI();
        ClosePanels();
    }

    void Start()
    {
        if (betInput != null)
            betInput.onEndEdit.AddListener(OnBetInputChanged);
    }

    public void OnBetInputChanged(string value)
    {
        if (int.TryParse(value, out int result))
        {
            currentBet = Mathf.Clamp(result, minBet, maxBet);
        }
        else
        {
            currentBet = minBet;
        }
        UpdateBetUI();
    }

    void UpdateBetUI()
    {
        if (betInput != null)
            betInput.text = currentBet.ToString();
    }

    public void Spin()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentMoney < currentBet)
        {
            Debug.Log("Para yetersiz!");
            return;
        }

        if (isSpinning) return;

        ClosePanels();
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        isSpinning = true;
        spinButton.interactable = false;

        GameManager.Instance.currentMoney -= currentBet;
        GameManager.Instance.UpdateUI();

        float timer = 0f;
        float animationSpeed = 0.05f;

        while (timer < spinDuration)
        {
            ShuffleSlotsInstant();
            yield return new WaitForSecondsRealtime(animationSpeed);
            timer += animationSpeed;
        }

        // --- KAZANMA BELİRLEME KISMI (YENİ SİSTEM) ---
        int[] results = new int[9];

        // 1. Zar Atalım (0 ile 100 arası sayı tut)
        int randomChance = Random.Range(0, 100);

        // 2. Eğer tuttuğumuz sayı senin belirlediğin yüzdenin içindeyse => ZORLA KAZANDIR
        if (randomChance < winChancePercentage)
        {
            Debug.Log("Şanslı Gün! Sistem kazandırıyor. (Zar: " + randomChance + ")");
            ForceWin(results);
        }
        else
        {
            Debug.Log("Şans tutmadı, tamamen rastgele... (Zar: " + randomChance + ")");
            // Şans tutmadıysa tamamen rastgele doldur (Kazanma ihtimali çok düşük olur)
            for (int i = 0; i < 9; i++)
            {
                results[i] = Random.Range(0, slotIcons.Length);
            }
        }

        // Ekrana yansıt
        for (int i = 0; i < 9; i++)
        {
            slotImages[i].sprite = slotIcons[results[i]];
        }

        CheckWin(results);

        isSpinning = false;
        spinButton.interactable = true;
    }

    // KAZANDIRMA FONKSİYONU
    void ForceWin(int[] results)
    {
        // Önce hepsini rastgele yap (Doğallık bozulmasın)
        for (int i = 0; i < 9; i++)
        {
            results[i] = Random.Range(0, slotIcons.Length);
        }

        // Hangi kombinasyonla kazansın? (Düz, Çapraz vb.)
        int winType = Random.Range(0, 3); // 0: Üst, 1: Orta, 2: Alt
        int winningIcon = Random.Range(0, slotIcons.Length); // Hangi meyve?

        if (winType == 0) // Üst Satır
        {
            results[0] = winningIcon; results[1] = winningIcon; results[2] = winningIcon;
        }
        else if (winType == 1) // Orta Satır
        {
            results[3] = winningIcon; results[4] = winningIcon; results[5] = winningIcon;
        }
        else // Alt Satır
        {
            results[6] = winningIcon; results[7] = winningIcon; results[8] = winningIcon;
        }
    }

    void ShuffleSlotsInstant()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] != null)
            {
                int rnd = Random.Range(0, slotIcons.Length);
                slotImages[i].sprite = slotIcons[rnd];
            }
        }
    }

    void CheckWin(int[] r)
    {
        int totalWin = 0;

        totalWin += CheckLine(r[0], r[1], r[2]);
        totalWin += CheckLine(r[3], r[4], r[5]);
        totalWin += CheckLine(r[6], r[7], r[8]);
        totalWin += CheckLine(r[0], r[4], r[8]);
        totalWin += CheckLine(r[2], r[4], r[6]);

        if (totalWin > 0)
        {
            // KAZANDI
            GameManager.Instance.currentMoney += totalWin;
            GameManager.Instance.UpdateUI();

            if (winPanel != null)
            {
                winPanel.SetActive(true);
                if (winAmountText != null)
                    winAmountText.text = "+" + totalWin.ToString() + " $";
            }
        }
        else
        {
            // KAYBETTİ
            if (losePanel != null)
            {
                losePanel.SetActive(true);
                if (loseAmountText != null)
                    loseAmountText.text = "-" + currentBet.ToString() + " $";
            }
        }
    }

    int CheckLine(int a, int b, int c)
    {
        if (a == b && b == c) return currentBet * 5;
        return 0;
    }

    public void ClosePanels()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
}