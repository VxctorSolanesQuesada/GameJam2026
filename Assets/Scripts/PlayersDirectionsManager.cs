using UnityEngine;

public class PlayersDirectionManager : MonoBehaviour
{
    [Header("Assign in order: P1, P2, P3, P4")]
    public PlayerDirectionInput[] players = new PlayerDirectionInput[4];

    void Awake()
    {
        // Assign indexes automatically based on array order
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                players[i].SetPlayerIndex(i);
        }
    }

    public char[] GetPlayersDirections()
    {
        char[] result = new char[4];

        for (int i = 0; i < players.Length; i++)
        {
            result[i] = players[i] != null ? players[i].chosenDir : ' ';
        }

        return result;
    }

    public bool AllPlayersChosen()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) return false;
            if (!players[i].hasChosen) return false;
        }
        return true;
    }

    public void ResetAllChoices()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                players[i].ResetChoice();
        }
    }
}
