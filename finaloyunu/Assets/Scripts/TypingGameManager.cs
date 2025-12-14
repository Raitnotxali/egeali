using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TypingGameManager : MonoBehaviour
{
    public AudioSource wronginput;
    public AudioSource trueinput;
    [Header("Referanslar")]
    public GameObject letterPrefab; // Harf Prefabı (MovingLetter_V2)
    public Transform spawnPoint;    // Mavi alanın sağındaki nokta
    public Transform centerPoint;   // Beyaz kutunun ortası
    public Transform failPoint;     // Kırmızı alanın başı

    [Header("UI Ayarları")]
    public Image progressBar;       // Sarı dolma barı
    public float hitThreshold = 60f; // Vuruş genişliği (Rahat basmak için)

    [Header("Zorluk Ayarları")]
    public float startSpeed = 150f;      // Başlangıç hızı
    public float speedIncrease = 20f;    // Her GÖREV bitince eklenecek hız
    public float spawnRate = 1.5f;       // Harf çıkma sıklığı

    // Değişkenler
    private float currentSpeed;     // O anki hesaplanan hız
    private int currentScore = 0;
    private int targetScore = 5;    // Kaç tane yapınca görev bitsin?
    private float timer;
    private int failStreakAtZero = 0; // 0 puandayken kaç kere hata yaptık?

    private string allowedChars = "QWERTYUIOPASDFGHJKLZXCVBNM";
    private List<LetterMover> activeLetters = new List<LetterMover>();

    void OnEnable()
    {
        // --- 1. HIZI HESAPLA (Zorluk + Kolaylaştırma) ---
        if (GameManager.Instance != null)
        {
            // Temel Hız + (Bitirilen Görev * 20)
            float baseSpeed = startSpeed + (GameManager.Instance.writingCurrentCount * speedIncrease);

            // Eğer oyuncu çok yanmışsa ceza indirimini (writingSpeedPenalty) düş
            currentSpeed = baseSpeed - GameManager.Instance.writingSpeedPenalty;

            // Hız negatif olmasın, en az 100 olsun
            if (currentSpeed < 100f) currentSpeed = 100f;

            Debug.Log("Oyun Başladı! Hız: " + currentSpeed);
        }
        else
        {
            currentSpeed = startSpeed; // GameManager yoksa varsayılan
        }

        // --- 2. SIFIRLAMA ---
        currentScore = 0;
        failStreakAtZero = 0; // Hata sayacını sıfırla
        UpdateUI();
        ClearAllLetters(); // Ekranı temizle
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnLetter();
            timer = 0;
        }

        // Tuş Kontrolü
        if (Input.anyKeyDown)
        {
            string input = Input.inputString.ToUpper();
            if (!string.IsNullOrEmpty(input))
            {
                CheckInput(input[0]);
            }
        }
    }

    void SpawnLetter()
    {
        if (currentScore >= targetScore) return;

        // --- HARF YARATMA (O özel 'false' ayarı burada) ---
        // Harfi, SpawnPoint'in olduğu objenin (Arka planın) içine yaratıyoruz
        GameObject obj = Instantiate(letterPrefab, spawnPoint.parent, false);

        // Konumu SpawnPoint'e mıhla
        obj.transform.position = spawnPoint.position;

        // Ayarları sıfırla (Garanti olsun)
        Vector3 localPos = obj.transform.localPosition;
        obj.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        // En öne getir (Görünür olsun)
        obj.transform.SetAsLastSibling();

        LetterMover mover = obj.GetComponent<LetterMover>();
        char randomChar = allowedChars[Random.Range(0, allowedChars.Length)];

        // Hesapladığımız hızı harfe gönderiyoruz
        mover.Setup(randomChar, this, currentSpeed);

        activeLetters.Add(mover);
    }

    void CheckInput(char pressedKey)
    {
        for (int i = 0; i < activeLetters.Count; i++)
        {
            LetterMover letter = activeLetters[i];
            if (letter == null) continue;

            if (letter.myChar == pressedKey)
            {
                float distance = Mathf.Abs(letter.transform.position.x - centerPoint.position.x);

                if (distance <= hitThreshold)
                {
                    HitSuccess(letter);
                    return;
                }
            }
        }
    }

    void HitSuccess(LetterMover letter)
    {
        Debug.Log("VURDUK!");
        currentScore++;
        trueinput.Play();
        // Doğru yapınca hata serisini sıfırla
        failStreakAtZero = 0;

        UpdateUI();
        RemoveLetter(letter);
        Destroy(letter.gameObject);

        // --- GÖREV BİTME KONTROLÜ ---
        if (currentScore >= targetScore)
        {
            Debug.Log("GÖREV TAMAMLANDI!");

            // Harfleri temizle
            ClearAllLetters();

            // GameManager'a işle
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteWritingTask();
            }

            // Pencereyi kapat
            gameObject.SetActive(false);
        }
    }

    public void MissLetter(LetterMover letter)
    {
        Debug.Log("KAÇTI!");
        wronginput.Play();

        // --- KAYBETME KONTROLÜ (Fail Condition) ---
        if (currentScore == 0)
        {
            failStreakAtZero++;
            Debug.Log("Kritik Hata: " + failStreakAtZero + "/5");

            // 5 kere üst üste yandı mı?
            if (failStreakAtZero >= 5)
            {
                TriggerFailState(); // Oyunu kapat ve kolaylaştır
                return;
            }
        }
        else
        {
            currentScore--;
            failStreakAtZero = 0; // Puanı varsa seriyi bozma, sadece puan düş
        }

        UpdateUI();
    }

    // Çok başarısız olunca çalışan fonksiyon
    void TriggerFailState()
    {
        Debug.Log("OYUNCU YAPAMADI! OYUN KOLAYLAŞTIRILIYOR...");

        // GameManager'da indirimi artır
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ApplyEasierMode();
        }

        ClearAllLetters();
        gameObject.SetActive(false); // Uygulamayı kapat
    }

    public void RemoveLetter(LetterMover letter)
    {
        if (activeLetters.Contains(letter))
            activeLetters.Remove(letter);
    }

    void ClearAllLetters()
    {
        foreach (var letter in activeLetters)
        {
            if (letter != null) Destroy(letter.gameObject);
        }
        activeLetters.Clear();
    }

    void UpdateUI()
    {
        float ratio = (float)currentScore / targetScore;
        if (progressBar != null)
            progressBar.fillAmount = ratio;
    }
}