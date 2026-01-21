using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinListener : MonoBehaviour
{
    public PlayerJoinAssigner joinAssigner;
    void Start() { var pi = GetComponent<PlayerInput>(); pi.actions.FindActionMap("Join").Enable(); }
    public void OnJoin(InputAction.CallbackContext ctx)
    {
        Debug.Log("Alguien se unio");

        if (!ctx.performed)
            return;

        InputDevice device = ctx.control.device;

        joinAssigner.AssignDevice(device);
    }
}
