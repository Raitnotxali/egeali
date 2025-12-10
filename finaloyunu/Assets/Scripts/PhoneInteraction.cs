using UnityEngine;
using UnityEngine.EventSystems; // Farenin giriþ çýkýþýný anlamasý için þart

public class SimpleHoverActive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Üstüne Gelince Açýlacak Obje")]
    public GameObject objectToOpen; // Parlak olan resim objesi buraya gelecek

    // Fare üstüne gelince çalýþýr
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objectToOpen != null)
        {
            objectToOpen.SetActive(true); // Objeyi GÖSTER
        }
    }

    // Fare üstünden gidince çalýþýr
    public void OnPointerExit(PointerEventData eventData)
    {
        if (objectToOpen != null)
        {
            objectToOpen.SetActive(false); // Objeyi GÝZLE
        }
    }
}