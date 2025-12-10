using UnityEngine;

public class DesktopIcon : MonoBehaviour
{
    public GameObject windowToOpen;

    public void OpenWindow()
    {
        if (windowToOpen != null)
        {
            // ÝSPÝYONCU KOD:
            Debug.Log("Þu Obje Açýlmaya Çalýþýlýyor: " + windowToOpen.name);

            // Obje sahnede mi yoksa dosya mý anlamak için pozisyonuna bakalým
            Debug.Log("Objenin Konumu: " + windowToOpen.transform.position);

            windowToOpen.SetActive(true);
            windowToOpen.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogError("HATA: 'Window To Open' kutusu boþ kanka!");
        }
    }
}