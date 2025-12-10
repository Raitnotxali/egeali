using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    public static MatchingManager Instance;

    [Header("Ayarlar")]
    public GameObject filePrefab;   // Dosya Prefabý
    public Transform spawnArea;     // Dosyalarýn doðacaðý Kýrmýzý Alan
    public RectTransform appWindowBoundary; // Dosyalarýn dýþýna çýkamayacaðý ANA PENCERE
    public int startFileCount = 5;  // Ýlk baþta kaç dosya olsun?

    private int currentLevelFiles;  // Zorluk seviyesi (Dosya Sayýsý)

    [Header("Görseller")]
    public Sprite moneyIcon;
    public Sprite companyIcon;
    public Sprite employeeIcon;

    [Header("Ýsim Havuzlarý (10'ar tane)")]
    public List<string> moneyNames;
    public List<string> companyNames;
    public List<string> employeeNames;

    // Masadaki aktif dosya sayýsý
    private int activeFileCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Oyun ilk açýldýðýnda zorluðu baþlangýç deðerine eþitle
        // (0 kontrolü yapýyoruz ki oyun içinde artarsa sýfýrlanmasýn)
        if (currentLevelFiles == 0) currentLevelFiles = startFileCount;
    }

    void OnEnable()
    {
        SpawnFiles(); // Pencere her açýldýðýnda dosyalarý saç
    }

    public void SpawnFiles()
    {
        // 1. Önce masayý temizle (Eski çöp kalmasýn)
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }

        activeFileCount = 0;
        List<FileType> spawnList = new List<FileType>();

        // 2. KURAL: Her türden EN AZ 1 tane kesin olsun
        spawnList.Add(FileType.Money);
        spawnList.Add(FileType.Company);
        spawnList.Add(FileType.Employee);

        // 3. KURAL: Geri kalanlarý rastgele doldur
        // (Eðer currentLevelFiles 3'ten küçükse hata vermesin diye Math.Max kullanýyoruz)
        int remainingCount = Mathf.Max(0, currentLevelFiles - 3);
        for (int i = 0; i < remainingCount; i++)
        {
            spawnList.Add((FileType)Random.Range(0, 3));
        }

        // Listeyi karýþtýr (Hepsi sýrayla gelmesin)
        ShuffleList(spawnList);

        // Masaya diz
        foreach (FileType type in spawnList)
        {
            CreateFile(type);
        }
    }

    void CreateFile(FileType type)
    {
        // HATA KONTROLÜ 1: Prefab var mý?
        if (filePrefab == null)
        {
            Debug.LogError("MatchingManager Hatasý: 'File Prefab' kutusu boþ!");
            return;
        }

        // Dosyayý yarat
        GameObject newFile = Instantiate(filePrefab, spawnArea);
        DraggableFile script = newFile.GetComponent<DraggableFile>();

        // HATA KONTROLÜ 2: Script var mý?
        if (script == null)
        {
            Debug.LogError("MatchingManager Hatasý: Prefabýnda 'DraggableFile' scripti yok!");
            Destroy(newFile);
            return;
        }

        // Rastgele isim ve doðru ikon seç
        string randomName = "Bilinmiyor";
        Sprite correctIcon = null;

        switch (type)
        {
            case FileType.Money:
                if (moneyNames.Count > 0) randomName = moneyNames[Random.Range(0, moneyNames.Count)];
                correctIcon = moneyIcon;
                break;
            case FileType.Company:
                if (companyNames.Count > 0) randomName = companyNames[Random.Range(0, companyNames.Count)];
                correctIcon = companyIcon;
                break;
            case FileType.Employee:
                if (employeeNames.Count > 0) randomName = employeeNames[Random.Range(0, employeeNames.Count)];
                correctIcon = employeeIcon;
                break;
        }

        // Dosyayý Kur (Tip, Ýsim, Ýkon ve SINIR ALANI gönderiyoruz)
        script.SetupFile(type, randomName, correctIcon, appWindowBoundary);

        // Rastgele Konum (SpawnArea içinde daðýt)
        RectTransform rt = newFile.GetComponent<RectTransform>();
        RectTransform area = spawnArea.GetComponent<RectTransform>();

        // Kenarlardan biraz pay býrakarak (50px) rastgele yer seç
        float x = Random.Range(-area.rect.width / 2 + 50, area.rect.width / 2 - 50);
        float y = Random.Range(-area.rect.height / 2 + 50, area.rect.height / 2 - 50);

        rt.anchoredPosition = new Vector2(x, y);

        activeFileCount++;
    }

    // DOÐRU EÞLEÞTÝRME YAPILINCA BU ÇALIÞIR
    public void CheckTaskCompletion()
    {
        activeFileCount--;

        // Masadaki tüm dosyalar bitti mi?
        if (activeFileCount <= 0)
        {
            Debug.Log("GÖREV TAMAMLANDI! Ödül veriliyor...");

            // 1. GameManager'a parayý ve görevi iþlet
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteMatchingTask();
            }

            // 2. Zorluðu Artýr (Bir sonraki tur 1 dosya fazla gelsin)
            currentLevelFiles++;

            // 3. Masayý Temizle
            foreach (Transform child in spawnArea)
            {
                Destroy(child.gameObject);
            }

            // 4. Pencereyi Kapat
            gameObject.SetActive(false);
        }
    }

    // YANLIÞ EÞLEÞTÝRME YAPILINCA BU ÇALIÞIR (CEZA SÝSTEMÝ)
    public void FailTask()
    {
        Debug.Log("YANLIÞ EÞLEÞTÝRME! Ekran Kapanýyor...");

        // 1. Masayý temizle
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }

        // 2. Pencereyi kapat (Cezalý çýkýþ)
        gameObject.SetActive(false);
    }

    // Listeyi karýþtýrma fonksiyonu
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}