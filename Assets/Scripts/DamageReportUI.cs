using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageReportUI : MonoBehaviour
{
    [Header("UI Setup")]
    public Transform container;          // Vertical Layout Group
    public GameObject damageEntryPrefab; // Prefab con Image + Text
    public GameObject panel;

    public void ShowDamageReport(Dictionary<PlayerHealth, int> dmgDict)
    {
        // Limpiar entradas anteriores
        foreach (Transform child in container)
            Destroy(child.gameObject);

        panel.SetActive(true);

        // Crear una entrada por jugador
        foreach (var kvp in dmgDict)
        {
            PlayerHealth pHealth = kvp.Key;
            int lostHP = kvp.Value;

            GameObject entry = Instantiate(damageEntryPrefab, container);

            // Obtener referencias del prefab
            Image img = entry.transform.Find("Image").GetComponent<Image>();
            TMP_Text txt = entry.transform.Find("Text").GetComponent<TMP_Text>();

            // Asignar sprite del jugador
            img.sprite = pHealth.playerSprite;

            // Asignar texto
            txt.text = $"Has lost {lostHP} HP!";
        }
    }

    public void Deactivate()
    {
        panel.SetActive(false);
    }
}
