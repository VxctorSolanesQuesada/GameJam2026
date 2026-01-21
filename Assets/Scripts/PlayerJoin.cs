using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinAssigner : MonoBehaviour
{
    [Header("Jugadores en escena (con PlayerInput)")]
    public PlayerInput[] players; // Asigna los 4 PlayerInput en el inspector

    private List<PlayerInput> unassignedPlayers = new List<PlayerInput>();
    private HashSet<InputDevice> assignedDevices = new HashSet<InputDevice>();

    void Start()
    {
        // Inicialmente todos los jugadores están sin asignar
        unassignedPlayers.AddRange(players);

        // Desactivar PlayerInput hasta asignarles un mando
        foreach (var p in players)
            p.enabled = false;
    }

    public void AssignDevice(InputDevice device)
    {
        // Si este mando ya está asignado ? ignorar
        if (assignedDevices.Contains(device)) return;
        // Si no quedan jugadores libres ? ignorar
        if (unassignedPlayers.Count == 0)
            return;

        // Asignar al primer jugador libre
        PlayerInput player = unassignedPlayers[0];
        unassignedPlayers.RemoveAt(0);

        // Activar PlayerInput y asignar el mando
        player.enabled = true;
        player.SwitchCurrentControlScheme(device);

        assignedDevices.Add(device);

        Debug.Log($"Mando {device.displayName} asignado a {player.gameObject.name}");
    }
}
