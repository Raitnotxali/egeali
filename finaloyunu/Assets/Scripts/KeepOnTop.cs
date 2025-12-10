using UnityEngine;

public class KeepOnTop : MonoBehaviour
{
    // Update yerine LateUpdate kullanýyoruz.
    // Neden? Çünkü diðer tüm kodlar (telefon açma vs.) iþini bitirsin,
    // en son son sözü biz söyleyelim ve en öne geçelim.
    void LateUpdate()
    {
        transform.SetAsLastSibling();
    }
}