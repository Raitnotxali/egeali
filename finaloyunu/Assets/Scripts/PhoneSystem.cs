using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi (Yazılar için şart)

public class PhoneSystem : MonoBehaviour
{
    [Header("1. Açma Kapama Ayarı")]
    public GameObject phoneScreenUI; // Telefonun kendisi (Açılacak Panel)
    bool otp = true;
 
    [Header("2. İçerik Textleri (Sürükle Bırak)")]
    public TextMeshProUGUI moneyText; // Para yazısı ($ 100)
    public TextMeshProUGUI timeText;  // Saat yazısı (04:59)
    public TextMeshProUGUI taskText;  // Görev Listesi (Döküman: 1/5 vb.)
    public int moneygoal = 2000;

    // Update her karede çalışır, verileri anlık günceller
    void Update()
    {
        // GameManager yoksa veya Telefon Kapalıysa boşuna işlem yapma
        if (GameManager.Instance == null || !phoneScreenUI.activeSelf) return;

        // --- 1. PARAYI GÜNCELLE ---
        moneyText.text = $" {moneygoal}/{GameManager.Instance.currentMoney.ToString()}";

        // --- 2. SÜREYİ GÜNCELLE (Dakika:Saniye) ---
        int kalanSaniye = Mathf.CeilToInt(GameManager.Instance.kalanSure);
        int dk = kalanSaniye / 60;
        int sn = kalanSaniye % 60;
        timeText.text = string.Format("{0:00}:{1:00}", dk, sn);

        // --- 3. GÖREVLERİ GÜNCELLE (Burası Yeni!) ---
        // GameManager'dan sayıları çekip alt alta yazdırıyoruz (\n alt satıra geçer)
        string gorevListesi = "";

        // Döküman Görevi (Yapılan / Hedef)
        gorevListesi += "D.Writing: " + GameManager.Instance.writingCurrentCount + " / " + GameManager.Instance.writingDailyGoal + "\n";

        // Eşleştirme Görevi
        gorevListesi += "D.Matching: " + GameManager.Instance.matchingCurrentCount + " / " + GameManager.Instance.matchingDailyGoal + "\n";

  

        // Hazırladığımız metni ekrana bas
        taskText.text = gorevListesi;
    }

    // ----------------------------------------------------
    // KONTROL KISMI (Açma / Kapama)
    // ----------------------------------------------------

    public void OpenPhone()
    {
        if(otp)
        {
            otp = false;
            phoneScreenUI.SetActive(true);
            phoneScreenUI.transform.SetAsLastSibling(); // En öne getir
        }
        else
        {
            otp = true;
            phoneScreenUI.SetActive(false);
        }
        
    }

    public void ClosePhone()
    {
        phoneScreenUI.SetActive(false);
    }
}