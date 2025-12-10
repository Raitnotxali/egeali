using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Yeni Input sistemi için

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sahte Fare Resimlerimiz")]
    public GameObject arrowCursorUI;
    public GameObject handCursorUI;

    void Start()
    {
        Cursor.visible = false;

        // Baþlangýçta null kontrolü yapalým
        if (arrowCursorUI != null) arrowCursorUI.SetActive(true);
        if (handCursorUI != null) handCursorUI.SetActive(false);
    }

    void Update()
    {
        // Obje yoksa (yok edildiyse) kodu çalýþtýrma
        if (arrowCursorUI == null || handCursorUI == null) return;

        // Farenin pozisyonunu al
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (arrowCursorUI.activeSelf)
        {
            arrowCursorUI.transform.position = mousePos;
        }
        else if (handCursorUI.activeSelf)
        {
            handCursorUI.transform.position = mousePos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // KONTROL: Objeler hala sahnede mi?
        if (arrowCursorUI == null || handCursorUI == null) return;

        arrowCursorUI.SetActive(false);
        handCursorUI.SetActive(true);
        handCursorUI.transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // KONTROL: Objeler hala sahnede mi? (Hata veren yer burasýydý)
        if (arrowCursorUI == null || handCursorUI == null) return;

        handCursorUI.SetActive(false);
        arrowCursorUI.SetActive(true);
        arrowCursorUI.transform.SetAsLastSibling();
    }
}