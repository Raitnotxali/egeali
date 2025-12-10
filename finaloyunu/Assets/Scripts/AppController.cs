using UnityEngine;
using UnityEngine.UI;

public class AppWindow : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject taskbarIconPrefab; // Aþaðýda çýkacak küçük ikonun Prefabý
    public Transform taskbarGrid;        // Taskbar'daki "Layout Group" olan obje

    private GameObject currentTaskbarIcon; // O an yaratýlan ikonun yedeði

    // PENCEREYÝ KAPATMA (X TUÞU)
    public void CloseApp()
    {
        // 1. Varsa aþaðýdaki ikonu yok et
        if (currentTaskbarIcon != null)
        {
            Destroy(currentTaskbarIcon);
        }

        // 2. Pencereyi tamamen kapat
        gameObject.SetActive(false);
    }

   

    // AÞAÐIDAN GERÝ ÇAÐIRMA
    public void RestoreApp()
    {
        // 1. Pencereyi aç
        gameObject.SetActive(true);

        // 2. Aþaðýdaki ikonu yok et (Çünkü artýk ekrandayýz)
        if (currentTaskbarIcon != null)
        {
            Destroy(currentTaskbarIcon);
        }
    }
}