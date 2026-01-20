using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public enum TurnPhase { Dice, SetUpDirectionChoice, DirectionChoice, DirectionBattle, Move, RoomResolution, EndTurn };
    public MapController grid;
    public PlayerManager playerManager;
    public PlayersDirectionManager dirManager;
    public PlayersBattleManager battleManager;


    public List<PlayerHealth> playing = new List<PlayerHealth>();
    public List<PlayerDirectionInput> listDirInput = new List<PlayerDirectionInput>();
    private TurnPhase currentPhase = TurnPhase.Dice;
    private bool didGameEnded = false;
    private int[] dirValues = new int[4]; // 0 = N, 1 = S, 2 = E, 3 = W

    private bool directionBattleResolved = false;
    private bool roomResolutionResolved = false;

    private Queue<List<PlayerDirectionInput>> pendingBattles = new Queue<List<PlayerDirectionInput>>();
    private bool waitingDirectionBattleInput = false;
    private List<PlayerDirectionInput> currentBattlePlayers = new List<PlayerDirectionInput>();


    List<PlayerDirectionInput> GetPlayersByDirection(List<PlayerDirectionInput> list, char dir)
    {
        List<PlayerDirectionInput> result = new List<PlayerDirectionInput>();

        foreach (var p in list)
        {
            if (char.ToLower(p.confirmedDir) == char.ToLower(dir))
            {
                result.Add(p);
            }
        }

        return result;
    }

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

        switch (currentPhase)
        {
            case TurnPhase.Dice:
                Debug.Log("=== FASE: Dice ? Generando valores de dirección ===");
                //--------Fase dados--------
                dirValues = RandomDirections.Instance.RegenerateDirections();
                currentPhase = TurnPhase.SetUpDirectionChoice;
                break;

            case TurnPhase.SetUpDirectionChoice:
                Debug.Log("=== FASE: SetUpDirectionChoice ? Preparando jugadores y reseteando inputs ===");
                //-----Setup Fase Escoger dirección--------
                playing = new List<PlayerHealth>();
                playing = playerManager.GetAlivePlayers();

                listDirInput = new List<PlayerDirectionInput>();

                foreach (PlayerHealth ph in playing)
                {
                    PlayerDirectionInput dir = ph.gameObject.GetComponent<PlayerDirectionInput>();
                    listDirInput.Add(dir);
                }

                dirManager.ResetAllChoices();
                battleManager.ResetAllChoicesBattle();

                pendingBattles.Clear();
                waitingDirectionBattleInput = false;
                currentBattlePlayers.Clear();
                directionBattleResolved = false;

                roomResolutionResolved = false;

                currentPhase = TurnPhase.DirectionChoice;
                break;

            case TurnPhase.DirectionChoice:
                Debug.Log("=== FASE: DirectionChoice ? Esperando que todos elijan dirección ===");
                //------Fase Escoger Dir---------
                if (dirManager.AllPlayersHaveChosen(listDirInput))
                {
                    foreach (PlayerDirectionInput pDI in listDirInput)
                    {
                        pDI.confirmInput();
                    }

                    currentPhase = TurnPhase.DirectionBattle;

                }
                break;

            case TurnPhase.DirectionBattle:
                Debug.Log("=== FASE: DirectionBattle ? Resolviendo conflictos de direcciones ===");
                //-----Fase Direction Battle------

                if (!directionBattleResolved)
                {
                    int countS = 0; int countN = 0; int countE = 0; int countW = 0;

                    foreach (PlayerDirectionInput pDI in listDirInput)
                    {
                        switch (char.ToLower(pDI.confirmedDir))
                        {
                            case 's': countS++; break;
                            case 'n': countN++; break;
                            case 'e': countE++; break;
                            case 'w': countW++; break;
                        }
                    }

                    // Detectar TODAS las batallas
                    if (countN > dirValues[0])
                        pendingBattles.Enqueue(GetPlayersByDirection(listDirInput, 'n'));

                    if (countS > dirValues[1])
                        pendingBattles.Enqueue(GetPlayersByDirection(listDirInput, 's'));

                    if (countE > dirValues[2])
                        pendingBattles.Enqueue(GetPlayersByDirection(listDirInput, 'e'));

                    if (countW > dirValues[3])
                        pendingBattles.Enqueue(GetPlayersByDirection(listDirInput, 'w'));

                    directionBattleResolved = true;
                }
                // Si estamos esperando inputs de la batalla actual
                if (waitingDirectionBattleInput)
                {
                    if (dirManager.AllPlayersHaveChosen(currentBattlePlayers))
                    {
                        foreach (var p in currentBattlePlayers)
                            p.confirmInput();

                        waitingDirectionBattleInput = false;
                        currentBattlePlayers.Clear();
                    }
                    else
                    {
                        break; // seguir esperando
                    }
                }

                // Si no estamos esperando inputs, resolver la siguiente batalla
                if (pendingBattles.Count > 0)
                {
                    List<PlayerDirectionInput> battleGroup = pendingBattles.Peek();

                    List<PlayerBattleInput> currentBattlingPlayers = new List<PlayerBattleInput>();

                    // Elegir el primero
                    int index1 = Random.Range(0, battleGroup.Count);
                    PlayerBattleInput battle1 = battleGroup[index1].gameObject.GetComponent<PlayerBattleInput>();
                    currentBattlingPlayers.Add(battle1);

                    // Elegir el segundo asegurando que no sea el mismo
                    int index2;
                    do
                    {
                        index2 = Random.Range(0, battleGroup.Count);
                    }
                    while (index2 == index1);

                    PlayerBattleInput battle2 = battleGroup[index2].gameObject.GetComponent<PlayerBattleInput>();
                    currentBattlingPlayers.Add(battle2);

                    if (battleManager.AllPlayersHaveChosen(currentBattlingPlayers))
                    {
                        foreach(var p in currentBattlingPlayers)
                        {
                            p.confirmInput();
                        }

                        char a = battle1.confirmedBattle; 
                        char b = battle2.confirmedBattle;

                        int result = battleManager.RPSResult(a, b);

                        if (result == 0)
                        { 
                            // EMPATE ? repetir batalla
                            Debug.Log("Empate en batalla RPS. Repitiendo...");
                            battleManager.ResetAllChoicesBattle(); 
                            break; 
                        }

                        // HAY GANADOR
                        PlayerBattleInput winner = (result == 1) ? battle1 : battle2; 
                        PlayerBattleInput loser = (result == 1) ? battle2 : battle1;

                        Debug.Log($"Ganador de la batalla: {winner.name}");

                        currentBattlePlayers.Clear(); 
                        currentBattlePlayers.Add(loser.gameObject.GetComponent<PlayerDirectionInput>());


                        dirManager.ResetAllChoices();
                        battleManager.ResetAllChoicesBattle();
                        waitingDirectionBattleInput = true;
                        pendingBattles.Dequeue();
                    }   

                    break; // esperar inputs
                }

                // Si no quedan batallas ? pasar a Move
                currentPhase = TurnPhase.Move;
                break;



            case TurnPhase.Move:
                Debug.Log("=== FASE: Move ? Moviendo jugadores y revelando tiles ===");
                //---------Fase Move--------

                foreach (PlayerHealth n in playing)
                {
                    PlayerDirectionInput pDI = n.gameObject.GetComponent<PlayerDirectionInput>();
                    PlayerGridPosition pg = n.gameObject.GetComponent<PlayerGridPosition>();
                    pg.Move(pDI.confirmedDir);
                    grid.RevealTile(pg.gridPos.x, pg.gridPos.y);
                    pDI.confirmedDir = ' ';
                }
                currentPhase = TurnPhase.RoomResolution;

                break;

            case TurnPhase.RoomResolution:
                Debug.Log("=== FASE: RoomResolution ? Resolviendo daño y batallas en salas ===");
                //----Agrupar jugadores por sala-----
                Dictionary<Vector2Int, List<PlayerGridPosition>> rooms = new Dictionary<Vector2Int, List<PlayerGridPosition>>();
                foreach (var p in playing)
                {
                    PlayerGridPosition pg = p.gameObject.GetComponent<PlayerGridPosition>();
                    if (!rooms.ContainsKey(pg.gridPos))
                        rooms[pg.gridPos] = new List<PlayerGridPosition>();

                    rooms[pg.gridPos].Add(pg);
                }

                //----Ejecutar daño/votacion por sala---
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
                            PlayerGridPosition randomPlayer = playersInRoom[Random.Range(0, playersInRoom.Count)];

                            PlayerHealth pHP = randomPlayer.gameObject.GetComponent<PlayerHealth>();
                            pHP.TakeDmg(tile.damageValue);


                        }
                    }
                }

                currentPhase = TurnPhase.EndTurn;

                break;

            case TurnPhase.EndTurn:
                Debug.Log("=== FASE: EndTurn ? Turno finalizado, reiniciando ciclo ===");
                currentPhase = TurnPhase.Dice;

                break;
            }







        


        


        



        



    }
}
