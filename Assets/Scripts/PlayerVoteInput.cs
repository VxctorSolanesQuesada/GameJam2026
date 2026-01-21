using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVoteInput : MonoBehaviour
{
    public int playerIndex;
    public Sprite voteSprite;

    public int chosenVote = -1;   // índice 0–3 según dirección
    public bool hasChosen = false;

    public void OnVote(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (hasChosen) return;

        Vector2 v = ctx.ReadValue<Vector2>();
        if (v.magnitude < 0.5f) return;

        // Determinar dirección dominante
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            // Horizontal
            chosenVote = (v.x > 0) ? 1 : 3; // derecha = 1, izquierda = 3
        }
        else
        {
            // Vertical
            chosenVote = (v.y > 0) ? 0 : 2; // arriba = 0, abajo = 2
        }

        hasChosen = true;

        Debug.Log($"{name} votó dirección {chosenVote}");
    }

    public void ResetChoice()
    {
        chosenVote = -1;
        hasChosen = false;
    }

    public void ConfirmInput()
    {
        // opcional
    }
}
