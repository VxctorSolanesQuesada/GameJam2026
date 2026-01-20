using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleInput : MonoBehaviour
{
    public int playerIndex;       // 0..3
    public char chosenBattle = ' ';  // 'n','s','e','w'
    public bool hasChosen = false;
    public char confirmedBattle = ' ';
    public Sprite battleSprite;

    // Set this from the manager in Start (since players exist from the beginning)
    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    // Hook this to your "Move" action (D-Pad)
    public void OnBattle(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (hasChosen) return;

        Vector2 v = context.ReadValue<Vector2>();
        if (v.magnitude < 0.5f) return;

        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            chosenBattle = v.x > 0 ? 'e' : 'w';
        else
            chosenBattle = v.y > 0 ? 'n' : 's';

        hasChosen = true;

        Debug.Log(gameObject.name + " (P" + (playerIndex + 1) + ") chose " + chosenBattle);
    }


    public void confirmInput()
    {
        confirmedBattle = chosenBattle;
    }
    public void ResetChoice()
    {
        chosenBattle = ' ';
        hasChosen = false;
    }


}
