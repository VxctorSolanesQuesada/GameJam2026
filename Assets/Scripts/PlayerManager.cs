using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerHealth[] players;


    public List<PlayerHealth> GetAlivePlayers()
    {
        List<PlayerHealth> alive = new List<PlayerHealth>();

        for (int i = 0; i < players.Length; i++)
        {
            PlayerHealth p = players[i];

            if (p != null && p.gameObject.activeSelf && p.currentHP > 0)
            {
                alive.Add(p);
            }
        }

        return alive;
    }
    public PlayerHealth GetPlayer(int index)
    {
        if (players == null) return null;
        if (index < 0 || index >= players.Length) return null;
        return players[index];
    }

    public bool IsPlayerAlive(int index)
    {
        PlayerHealth p = GetPlayer(index);
        if (p == null) return false;
        return p.gameObject.activeSelf && p.currentHP > 0;
    }

    public int GetCurrentHP(int index)
    {
        PlayerHealth p = GetPlayer(index);
        if (p == null) return 0;
        return p.currentHP;
    }

    public void DamagePlayer(int index, int amount)
    {
        PlayerHealth p = GetPlayer(index);
        if (p == null) return;

        p.TakeDmg(amount); 
    }
}
