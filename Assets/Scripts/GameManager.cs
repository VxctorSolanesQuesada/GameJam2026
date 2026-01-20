using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public MapController grid;
    public PlayerManager playerManager;


    public List<PlayerHealth> playing = new List<PlayerHealth>();

    private bool didGameEnded = false;
    private int[] dirValues = new int[4]; // 0 = N, 1 = S, 2 = E, 3 = W


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (didGameEnded)
        {
            return;
        }
            
        //Fase dados = direcionts values
        dirValues = RandomDirections.Instance.RegenerateDirections();


        playing = playerManager.GetAlivePlayers();

        //Fase eleccion dir de cada jugador (inputs: lista de player, dir bloquada )


        //int countS = 0; int countN = 0; int countE = 0; int countW = 0;

        //char[] chars = { playerDir[0], playerDir[1], playerDir[2], playerDir[3] };

        //foreach (char c in chars)
        //{
        //    switch (char.ToLower(c))
        //    {
        //        case 's': countS++; break;
        //        case 'n': countN++; break;
        //        case 'e': countE++; break;
        //        case 'w': countW++; break;
        //    }
        //}

        //if (countN > dirValues[0])
        //{
        //    //Fase batalla con jugadores con playerDir = n (inputs: lista de player / output: player o players que perdieron)



        //}
        //if (countS > dirValues[1])
        //{
        //    //Fase batalla con jugadores con playerDir = s (inputs: lista de player / output: player o players que perdieron)



        //}
        //if (countE > dirValues[2])
        //{
        //    //Fase batalla con jugadores con playerDir = E (inputs: lista de player / output: player o players que perdieron)



        //}
        //if (countW > dirValues[3])
        //{
        //    //Fase batalla con jugadores con playerDir = W (inputs: lista de player / output: player o players que perdieron)



        //}

        //Fase eleccion dir de jugador/es que perdieron (inputs: lista de player, dir bloqueda)



        //Fase mover

        foreach (PlayerHealth n in playing)
        {
            PlayerGridPosition pg = n.gameObject.GetComponent<PlayerGridPosition>();
            pg.Move(pg.direction);
            grid.RevealTile(pg.gridPos.x, pg.gridPos.y);
            pg.direction = Direction.None;
        }


        //Agrupar jugadores por sala
        Dictionary<Vector2Int, List<PlayerGridPosition>> rooms = new Dictionary<Vector2Int, List<PlayerGridPosition>>();
        foreach (var p in playing) 
        {
            PlayerGridPosition pg = p.gameObject.GetComponent<PlayerGridPosition>();
            if (!rooms.ContainsKey(pg.gridPos)) 
                rooms[pg.gridPos] = new List<PlayerGridPosition>(); 

            rooms[pg.gridPos].Add(pg); 
        }

        //Ejecutar daño/votacion por sala
        foreach (var kvp in rooms) 
        { 
            Vector2Int pos = kvp.Key; 
            List<PlayerGridPosition> playersInRoom = kvp.Value; 
           
            Tile tile = grid.grid[pos.x, pos.y]; 
            
            if (playersInRoom.Count == 1) 
            { 
                PlayerGridPosition p = playersInRoom[0]; 
                if (tile.damageValue > 0) 
                {
                    //Restar puntos
                    foreach (PlayerGridPosition p1 in playersInRoom)
                    {
                        PlayerHealth pHP = p1.gameObject.GetComponent<PlayerHealth>();
                        pHP.TakeDmg(tile.damageValue);
                    }
                } 
            } 
            else if (playersInRoom.Count > 1) 
            { 
                Debug.Log($"Batalla en sala {pos} entre {playersInRoom.Count} jugadores");


                if (tile.damageValue > 0)
                {


                    //Restar puntos
                    //ChosenPlayerHealth.TakeDmg(tile.damageValue);


                }
            }
        }



        



    }
}
