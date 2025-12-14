using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MatchingManager : MonoBehaviour
{
    public static MatchingManager Instance;
    public AudioSource trueanswer;
    public AudioSource wronganswer;

    [Header("Ayarlar")]
    public GameObject filePrefab;   // Dosya Prefab�
    public Transform spawnArea;     // Dosyalar�n do�aca�� K�rm�z� Alan
    public RectTransform appWindowBoundary; // Dosyalar�n d���na ��kamayaca�� ANA PENCERE
    public int startFileCount = 5;  // �lk ba�ta ka� dosya olsun?

    private int currentLevelFiles;  // Zorluk seviyesi (Dosya Say�s�)

    [Header("G�rseller")]
    public Sprite moneyIcon;
    public Sprite companyIcon;
    public Sprite employeeIcon;

    [Header("�sim Havuzlar� (10'ar tane)")]
    public List<string> moneyNames;
    public List<string> companyNames;
    public List<string> employeeNames;

    // Masadaki aktif dosya say�s�
    private int activeFileCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Oyun ilk a��ld���nda zorlu�u ba�lang�� de�erine e�itle
        // (0 kontrol� yap�yoruz ki oyun i�inde artarsa s�f�rlanmas�n)
        if (currentLevelFiles == 0) currentLevelFiles = startFileCount;
    }

    void OnEnable()
    {
        SpawnFiles(); // Pencere her a��ld���nda dosyalar� sa�
    }

    public void SpawnFiles()
    {
        // 1. �nce masay� temizle (Eski ��p kalmas�n)
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }

        activeFileCount = 0;
        List<FileType> spawnList = new List<FileType>();

        // 2. KURAL: Her t�rden EN AZ 1 tane kesin olsun
        spawnList.Add(FileType.Money);
        spawnList.Add(FileType.Company);
        spawnList.Add(FileType.Employee);

        // 3. KURAL: Geri kalanlar� rastgele doldur
        // (E�er currentLevelFiles 3'ten k���kse hata vermesin diye Math.Max kullan�yoruz)
        int remainingCount = Mathf.Max(0, currentLevelFiles - 3);
        for (int i = 0; i < remainingCount; i++)
        {
            spawnList.Add((FileType)Random.Range(0, 3));
        }

        // Listeyi kar��t�r (Hepsi s�rayla gelmesin)
        ShuffleList(spawnList);

        // Masaya diz
        foreach (FileType type in spawnList)
        {
            CreateFile(type);
        }
    }

    void CreateFile(FileType type)
    {
        // HATA KONTROL� 1: Prefab var m�?
        if (filePrefab == null)
        {
            Debug.LogError("MatchingManager Hatas�: 'File Prefab' kutusu bo�!");
            return;
        }

        // Dosyay� yarat
        GameObject newFile = Instantiate(filePrefab, spawnArea);
        DraggableFile script = newFile.GetComponent<DraggableFile>();

        // HATA KONTROL� 2: Script var m�?
        if (script == null)
        {
            Debug.LogError("MatchingManager Hatas�: Prefab�nda 'DraggableFile' scripti yok!");
            Destroy(newFile);
            return;
        }

        // Rastgele isim ve do�ru ikon se�
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

        // Dosyay� Kur (Tip, �sim, �kon ve SINIR ALANI g�nderiyoruz)
        script.SetupFile(type, randomName, correctIcon, appWindowBoundary);

        // Rastgele Konum (SpawnArea i�inde da��t)
        RectTransform rt = newFile.GetComponent<RectTransform>();
        RectTransform area = spawnArea.GetComponent<RectTransform>();

        // Kenarlardan biraz pay b�rakarak (50px) rastgele yer se�
        float x = Random.Range(-area.rect.width / 2 + 50, area.rect.width / 2 - 50);
        float y = Random.Range(-area.rect.height / 2 + 50, area.rect.height / 2 - 50);

        rt.anchoredPosition = new Vector2(x, y);

        activeFileCount++;
    }

    // DO�RU E�LE�T�RME YAPILINCA BU �ALI�IR
    public void CheckTaskCompletion()
    {
        activeFileCount--;
        trueanswer.Play();

        // Masadaki t�m dosyalar bitti mi?
        if (activeFileCount <= 0)
        {
            Debug.Log("G�REV TAMAMLANDI! �d�l veriliyor...");

            // 1. GameManager'a paray� ve g�revi i�let
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteMatchingTask();
            }

            // 2. Zorlu�u Art�r (Bir sonraki tur 1 dosya fazla gelsin)
            currentLevelFiles++;

            // 3. Masay� Temizle
            foreach (Transform child in spawnArea)
            {
                Destroy(child.gameObject);
            }

            // 4. Pencereyi Kapat
            gameObject.SetActive(false);
        }
    }

    // YANLI� E�LE�T�RME YAPILINCA BU �ALI�IR (CEZA S�STEM�)
    public void FailTask()
    {
        Debug.Log("YANLI� E�LE�T�RME! Ekran Kapan�yor...");
        wronganswer.Play();

        // 1. Masay� temizle
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }

        // 2. Pencereyi kapat (Cezal� ��k��)
        gameObject.SetActive(false);
    }

    // Listeyi kar��t�rma fonksiyonu
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