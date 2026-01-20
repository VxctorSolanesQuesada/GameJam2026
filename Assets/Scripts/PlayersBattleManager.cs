using System.Collections.Generic;
using UnityEngine;

public class PlayersBattleManager : MonoBehaviour
{
    public PlayerBattleInput[] players = new PlayerBattleInput[4];

    void Awake()
    {
        // Assign indexes automatically based on array order
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                players[i].SetPlayerIndex(i);
        }
    }

    public char[] GetPlayersDirections(List<PlayerBattleInput> players)
    {
        char[] result = new char[4]; int index = 0; 
        foreach (var p in players) 
        { 
            result[index] = p != null ? p.chosenBattle : ' '; 
            index++; 
        }
        return result;
    }

    public bool AllPlayersHaveChosen(List<PlayerBattleInput> players)
    {
        foreach (var p in players)
        {
            if (p == null)
                return false;

            if (!p.hasChosen)
                return false;
        }

        return true;
    }


    public void ResetAllChoicesBattle()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
                players[i].ResetChoice();
        }
    }

    // Devuelve:
    //  1 si A gana
    // -1 si B gana
    //  0 si empate
    public int RPSResult(char a, char b)
    {
        if (a == b) return 0;

        if ((a == 'r' && b == 's') ||
            (a == 's' && b == 'p') ||
            (a == 'p' && b == 'r'))
            return 1;

        return -1;
    }

}
