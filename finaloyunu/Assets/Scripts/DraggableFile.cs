using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class DraggableFile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Dosya Bilgileri")]
    public FileType fileType;
    public TextMeshProUGUI nameText;
    public Image iconImage;

    [HideInInspector] public Transform originalParent;

    // YENİ: Sınırlandırma Alanı
    [HideInInspector] public RectTransform dragLimitRect;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas myCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        myCanvas = GetComponentInParent<Canvas>();
    }

    // Setup'a 'limitArea' parametresi ekledik (Opsiyonel)
    public void SetupFile(FileType type, string text, Sprite icon, RectTransform limitArea)
    {
        fileType = type;
        nameText.text = text;
        iconImage.sprite = icon;
        dragLimitRect = limitArea; // Sınırı kaydet
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        if (myCanvas != null) transform.SetParent(myCanvas.transform, true);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (myCanvas == null) return;

        // 1. Normal Hareket
        rectTransform.anchoredPosition += eventData.delta / myCanvas.scaleFactor;

        // 2. SINIRLAMA (CLAMP) KODU 🔥
        if (dragLimitRect != null)
        {
            // Sınır kutusunun dünya koordinatlarını al (Köşeleri)
            Vector3[] corners = new Vector3[4];
            dragLimitRect.GetWorldCorners(corners);
            // corners[0] = Sol Alt, corners[2] = Sağ Üst

            // Dosyanın şu anki pozisyonunu al
            Vector3 position = rectTransform.position;

            // X ve Y'yi köşeler arasında sıkıştır
            position.x = Mathf.Clamp(position.x, corners[0].x, corners[2].x);
            position.y = Mathf.Clamp(position.y, corners[0].y, corners[2].y);

            // Sıkıştırılmış pozisyonu geri uygula
            rectTransform.position = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent, true);
        transform.SetAsLastSibling();
    }
}