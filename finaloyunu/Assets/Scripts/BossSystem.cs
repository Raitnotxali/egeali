using System.Collections;
using UnityEngine;
using TMPro;

public class BossManager : MonoBehaviour
{
    [Header("Animasyon (YENÝ)")]
    public Animator bossAnimator;     // Boss'un Animator bileþeni

    [Header("Hareket Ayarlarý")]
    public Transform bossObject;      // Boss'un kendisi (Resmi)
    public Transform startPoint;      // Sol taraftaki bekleme noktasý
    public Transform checkPoint;      // Masanýn yaný (Kontrol noktasý)
    public float moveSpeed = 2.0f;    // Yürüme hýzý

    [Header("Zamanlama")]
    public float minWaitTime = 60f;
    public float maxWaitTime = 120f;

    [Header("Yasaklý Uygulamalar")]
    public GameObject[] illegalApps;

    [Header("UI & Diyalog")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Ceza Ayarlarý")]
    public int penaltyAmount = 500;
    public int maxStrikes = 3;

    private int currentStrikes = 0;

    [Header("Günlük Ziyaret Ayarý")]
    public int maxDailyVisits = 3; // Boss günde kaç kere gelsin? (Motordan ayarla)
    private int currentVisitCount = 0; // Þu an kaçýncý ziyarette? (Bunu elleme)

    void Start()
    {
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        while (true)
        {

            if (currentVisitCount >= maxDailyVisits)
            {
                Debug.Log("Boss bugünlük kotayý doldurdu! Artýk gelmeyecek.");
                yield break; // Döngüyü tamamen durdurur, bir daha gelmez.
            }
            // 1. BEKLEME
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            Debug.Log("Boss " + waitTime + " saniye bekliyor.");
            yield return new WaitForSeconds(waitTime);

            currentVisitCount++;
            Debug.Log("Boss Geliyor! Bu " + currentVisitCount + ". geliþi.");

            // 2. MASAYA YÜRÜME (Ýleri git = true)
            Debug.Log("Boss kontrole geliyor...");
            yield return StartCoroutine(MoveToTarget(checkPoint.position, true));

            // 3. KONTROL
            CheckPlayer();
            yield return new WaitForSeconds(3f);
            if (dialoguePanel != null) dialoguePanel.SetActive(false);

            if (currentStrikes >= maxStrikes)
            {
                Debug.Log("KOVULDUN! Oyun Bitti.");
                yield break;
            }

            // 4. GERÝ DÖNME (Ýleri git = false)
            Debug.Log("Boss geri dönüyor.");
            yield return StartCoroutine(MoveToTarget(startPoint.position, false));


        }
    }

    // GÜNCELLENMÝÞ YÜRÜME FONKSÝYONU
    // goingToDesk: True ise masaya gidiyor (Ýleri), False ise dönüyor (Geri)
    IEnumerator MoveToTarget(Vector3 target, bool goingToDesk)
    {
        // --- ANÝMASYONU BAÞLAT ---
        if (bossAnimator != null)
        {
            if (goingToDesk)
            {
                // Ýleri yürüme parametresini aç
                bossAnimator.SetBool("isGoingForward", true);
            }
            else
            {
                // Geri yürüme parametresini aç
                bossAnimator.SetBool("isGoingBack", true);
            }
        }

        // Yönü ayarla (Saða mý sola mý bakacak?)
        if (target.x > bossObject.position.x)
            bossObject.localScale = new Vector3(Mathf.Abs(bossObject.localScale.x), bossObject.localScale.y, 1); // Saða bak
        else
            bossObject.localScale = new Vector3(-Mathf.Abs(bossObject.localScale.x), bossObject.localScale.y, 1); // Sola bak

        // Hedefe varana kadar yürü
        while (Vector3.Distance(bossObject.position, target) > 0.1f)
        {
            bossObject.position = Vector3.MoveTowards(bossObject.position, target, moveSpeed * Time.deltaTime);
            yield return null; // Bir sonraki kareyi bekle
        }

        // Hedeve vardýk, pozisyonu tam oturt
        bossObject.position = target;

        // --- ANÝMASYONU DURDUR (Idle'a dön) ---
        if (bossAnimator != null)
        {
            // Ýkisini de kapat ki Idle durumuna geçsin
            bossAnimator.SetBool("isGoingForward", false);
            bossAnimator.SetBool("isGoingBack", false);
        }
    }

    void CheckPlayer()
    {
        bool caught = false;
        foreach (GameObject app in illegalApps)
        {
            if (app != null && app.activeSelf)
            {
                caught = true;
                break;
            }
        }

        if (dialoguePanel != null) dialoguePanel.SetActive(true);

        if (caught)
        {
            currentStrikes++;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentMoney -= penaltyAmount;
                GameManager.Instance.UpdateUI();
            }

            if (dialogueText != null)
            {
                dialogueText.color = Color.red;
                dialogueText.text = "WHAT ARE YOU DOING HERE?! \n(-500$)";
            }

            if (currentStrikes >= maxStrikes)
            {
                if (dialogueText != null) dialogueText.text = "THAT'S ENOUGH! YOU FIRED!";
                // Lose Panel açma kodu buraya...
            }
        }
        else
        {
            if (dialogueText != null)
            {
                dialogueText.color = Color.green; // veya siyah
                dialogueText.text = "Good job, keep working.";
            }
        }
    }
}