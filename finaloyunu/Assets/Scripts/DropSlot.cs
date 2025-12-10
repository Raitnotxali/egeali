using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public FileType requiredType; // Bu klasör hangi tip dosyaları kabul ediyor?

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        DraggableFile file = droppedObj.GetComponent<DraggableFile>();

        if (file != null)
        {
            // DOĞRU DOSYA MI?
            if (file.fileType == requiredType)
            {
                Debug.Log("Doğru Eşleşme!");
                Destroy(droppedObj); // Dosyayı yok et
                MatchingManager.Instance.CheckTaskCompletion(); // Görevi kontrol et
            }
            else
            {
                // 🔥 YANLIŞ DOSYA! (Burayı değiştirdik)
                Debug.Log("YANLIŞ! Ekran Kapanıyor...");

                // Dosyayı hemen yok et ki havada asılı kalmasın
                Destroy(droppedObj);

                // Yöneticiye "Kapat dükkanı" de
                MatchingManager.Instance.FailTask();
            }
        }
    }
}