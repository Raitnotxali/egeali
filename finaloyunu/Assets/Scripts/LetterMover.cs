using UnityEngine;
using TMPro;

public class LetterMover : MonoBehaviour
{
    public char myChar;
    public float speed; // Hýz artýk dýþarýdan (Manager'dan) gelecek

    private RectTransform rectPos;
    private TypingGameManager manager;
    private bool isMissed = false;

    // Emniyet kilidi
    private bool isInitialized = false;

    // --- DÜZELTÝLEN KISIM BURASI (3. Parametre: newSpeed) ---
    public void Setup(char letter, TypingGameManager gameManager, float newSpeed)
    {
        myChar = letter;
        manager = gameManager;

        // Gelen hýzý alýp kendi hýzýmýza eþitliyoruz
        speed = newSpeed;

        GetComponent<TextMeshProUGUI>().text = myChar.ToString();
        rectPos = GetComponent<RectTransform>();

        // Kilidi aç
        isInitialized = true;
    }
    // -------------------------------------------------------

    void Update()
    {
        if (!isInitialized) return;

        // Yönetici yoksa (oyun kapandýysa) kendini imha et
        if (manager == null || manager.failPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        // Harekete baþla
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Kýrmýzý alana girdi mi?
        if (!isMissed && transform.position.x < manager.failPoint.position.x)
        {
            isMissed = true;
            manager.MissLetter(this);
            GetComponent<TextMeshProUGUI>().color = Color.red;
        }

        // Ekrandan çok çýktýysa yok et
        if (transform.position.x < manager.failPoint.position.x - 200)
        {
            manager.RemoveLetter(this);
            Destroy(gameObject);
        }
    }
}