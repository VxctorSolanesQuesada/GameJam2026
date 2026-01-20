using UnityEngine;
using UnityEngine.UI;

public enum CardIcon
{
    Default,
    Norte,
    Sur,
    Este,
    Oeste,
    Piedra,
    Papel,
    Tijeras
}

[RequireComponent(typeof(Image))]
public class PlayerCardImage : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite norteSprite;
    [SerializeField] private Sprite surSprite;
    [SerializeField] private Sprite esteSprite;
    [SerializeField] private Sprite oesteSprite;
    [SerializeField] private Sprite piedraSprite;
    [SerializeField] private Sprite papelSprite;
    [SerializeField] private Sprite tijerasSprite;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    public void Set(CardIcon icon)
    {
        targetImage.sprite = GetSprite(icon);
    }

    public CardIcon GetCurrent(out Sprite sprite)
    {
        sprite = targetImage.sprite;
        return CardIcon.Default; // CHECK
    }

    private Sprite GetSprite(CardIcon icon)
    {
        return icon switch
        {
            CardIcon.Default => defaultSprite,
            CardIcon.Norte => norteSprite,
            CardIcon.Sur => surSprite,
            CardIcon.Este => esteSprite,
            CardIcon.Oeste => oesteSprite,
            CardIcon.Piedra => piedraSprite,
            CardIcon.Papel => papelSprite,
            CardIcon.Tijeras => tijerasSprite,
            _ => defaultSprite
        };
    }

    public void SetDefault() => Set(CardIcon.Default);
    public void SetNorte() => Set(CardIcon.Norte);
    public void SetSur() => Set(CardIcon.Sur);
    public void SetEste() => Set(CardIcon.Este);
    public void SetOeste() => Set(CardIcon.Oeste);
    public void SetPiedra() => Set(CardIcon.Piedra);
    public void SetPapel() => Set(CardIcon.Papel);
    public void SetTijeras() => Set(CardIcon.Tijeras);
}
