using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    void Awake()
    {
        currentHP = maxHP;
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
            
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto");
        // aquí luego puedes: desactivar jugador, animación, etc.
        gameObject.SetActive(false);
    }
}
