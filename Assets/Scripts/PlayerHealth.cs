using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public Sprite playerSprite;

    [SerializeField] public GameObject canvasObjectToActivate;
    [SerializeField] public Image targetImage;
    [SerializeField] public TextMeshProUGUI hpText;
    [SerializeField] public TextMeshProUGUI hpPlayerText;


    void Awake()
    {
        //currentHP = maxHP;
    }

    public void TakeDmg(int num)
    {
        currentHP -= num;
        if (currentHP < 0)
        {
            currentHP = 0;
        }

        Debug.Log($"{gameObject.name} vida: {currentHP}/{maxHP}");

        if (currentHP == 0)
        {
            Die();
        }

        hpPlayerText.text = $"{currentHP}";
        ActivateUI(num);
    }

    public void ActivateUI(int lostHP)
    {
        // Activar canvas / panel
        if (canvasObjectToActivate != null)
        {
            canvasObjectToActivate.SetActive(true);
        }

        // Aplicar sprite del jugador
        targetImage.sprite = playerSprite;


        // Cambiar texto
        if (hpText != null)
        {
            hpText.text = $"Has lost {lostHP} HP!";
        }
    }

    public void DeactivateUI()
    {
        if (canvasObjectToActivate != null)
        {
            canvasObjectToActivate.SetActive(false);
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto");
        // aquí luego puedes: desactivar jugador, animación, etc.
        gameObject.SetActive(false);
    }


}
