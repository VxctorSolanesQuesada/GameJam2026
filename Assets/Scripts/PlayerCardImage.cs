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

    // Flip animation state
    private bool isFlipping = false;
    private bool reachedHalf = false;
    private float flipTimer = 0f;
    private float flipDuration = 0.5f;
    private CardIcon pendingIcon;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (!isFlipping)
            return;

        flipTimer += Time.deltaTime;

        if (!reachedHalf)
        {
            // Primera mitad: 0 ? 90 grados
            float t = flipTimer / flipDuration;
            float angle = Mathf.Lerp(0f, 90f, t);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            if (t >= 1f)
            {
                // Cambiar sprite justo en 90°
                targetImage.sprite = GetSprite(pendingIcon);

                // Preparar segunda mitad
                flipTimer = 0f;
                reachedHalf = true;
            }
        }
        else
        {
            // Segunda mitad: 90 ? 0 grados
            float t = flipTimer / flipDuration;
            float angle = Mathf.Lerp(90f, 0f, t);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);

            if (t >= 1f)
            {
                // Fin de animación
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                isFlipping = false;
            }
        }
    }

    public void Set(CardIcon icon)
    {
        if (isFlipping)
            return; // evita solapamientos

        pendingIcon = icon;
        flipTimer = 0f;
        reachedHalf = false;
        isFlipping = true;
    }

    public CardIcon GetCurrent(out Sprite sprite)
    {
        sprite = targetImage.sprite;
        return CardIcon.Default; // Puedes mejorarlo si quieres
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

    // Métodos helper
    public void SetDefault() => Set(CardIcon.Default);
    public void SetNorte() => Set(CardIcon.Norte);
    public void SetSur() => Set(CardIcon.Sur);
    public void SetEste() => Set(CardIcon.Este);
    public void SetOeste() => Set(CardIcon.Oeste);
    public void SetPiedra() => Set(CardIcon.Piedra);
    public void SetPapel() => Set(CardIcon.Papel);
    public void SetTijeras() => Set(CardIcon.Tijeras);
}
