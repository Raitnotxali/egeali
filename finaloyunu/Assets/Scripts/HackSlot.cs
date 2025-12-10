using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HackSlot : MonoBehaviour
{
    public TMP_InputField inputField;
    public Image background;

    void Awake()
    {
        if (inputField == null) inputField = GetComponent<TMP_InputField>();
        if (background == null) background = GetComponent<Image>();
    }

    // ARTIK RENGÝ DEÐÝL, RESMÝ DEÐÝÞTÝRÝYORUZ
    public void SetSprite(Sprite newSprite)
    {
        if (newSprite != null)
        {
            background.sprite = newSprite;

            // ÖNEMLÝ: Rengi BEYAZ yapýyoruz ki resim kendi orijinal renginde görünsün.
            // (Eðer gri kalýrsa resim karanlýk görünür)
            background.color = Color.white;
        }
    }

    public void ResetSlot(bool isInput, Sprite defaultSprite)
    {
        // Sýfýrlarken varsayýlan resmi tak
        SetSprite(defaultSprite);

        inputField.text = "";
        inputField.interactable = isInput;
    }

    public int GetDigit()
    {
        if (string.IsNullOrEmpty(inputField.text)) return -1;
        return int.Parse(inputField.text);
    }

    public void SetDigit(int number)
    {
        inputField.text = number.ToString();
    }
}