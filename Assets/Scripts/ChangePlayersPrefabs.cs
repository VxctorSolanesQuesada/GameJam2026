using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangePlayersPrebafbs : MonoBehaviour
{
    public int index = 0;

    [SerializeField] private List<GameObject> players = new List<GameObject>(); // Lista de prefabs de jugadores
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>(); // Lista de puntos de spawn

    private PlayerInputManager manager;

    void Start()
    {
        manager = GetComponent<PlayerInputManager>();

        index = 0;

        // Asigna el prefab inicial para el primer jugador
        if (players.Count > 0)
        {
            manager.playerPrefab = players[index];
        }
        else
        {
            Debug.LogError("La lista de prefabs está vacía.");
        }
    }

    public void changePlayers(PlayerInput input)
    {
        // Asigna posición del jugador
        if (spawnPoints.Count > 0)
        {
            int spawnIndex = index % spawnPoints.Count;
            Transform nextSpawnPoint = spawnPoints[spawnIndex];
            input.transform.position = nextSpawnPoint.position;
            input.transform.rotation = nextSpawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("La lista de puntos de spawn está vacía.");
        }

        // Aumenta índice para el siguiente jugador
        index++;

        // Previene que se pase del tamaño de la lista
        if (index >= players.Count)
        {
            index = 0;
        }

        // Asigna el próximo prefab
        manager.playerPrefab = players[index];
    }
}