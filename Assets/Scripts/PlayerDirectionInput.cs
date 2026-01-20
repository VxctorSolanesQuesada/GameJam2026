using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDirectionInput : MonoBehaviour
{
    public int playerIndex;       // 0..3
    public char chosenDir = ' ';  // 'n','s','e','w'
    public bool hasChosen = false;

    // Set this from the manager in Start (since players exist from the beginning)
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    // Hook this to your "Move" action (D-Pad)
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (hasChosen) return;

        Vector2 v = context.ReadValue<Vector2>();
        if (v.magnitude < 0.5f) return;

        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            chosenDir = v.x > 0 ? 'e' : 'w';
        else
            chosenDir = v.y > 0 ? 'n' : 's';

        hasChosen = true;

        Debug.Log(gameObject.name + " (P" + (playerIndex + 1) + ") chose " + chosenDir);
    }

    public void ResetChoice()
    {
        chosenDir = ' ';
        hasChosen = false;
    }
}
