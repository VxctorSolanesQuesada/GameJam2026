using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TurnPhase { Dice, SetUpDirectionChoice, DirectionChoice, DirectionBattle, Move, RoomResolution, EndTurn };
    public MapController grid;
    public PlayerManager playerManager;
    public PlayersDirectionManager dirManager;
    public PlayersBattleManager battleManager;
    public BattleUIController battleUIController;
    public DamageReportUI damageReportUI;
    public VoteUIController voteUIController; 


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

    private List<PlayerDirectionInput> battleGroup;
    private List<PlayerBattleInput> currentBattlingPlayers;
    private PlayerBattleInput battle1;
    private PlayerBattleInput battle2;
    private bool playersChosen = false;
    private bool draw = false;
    private bool waitingVoteInput = false;


    public float pauseTimerChoicesSelcted = 5.0f;
    public float pauseTimerDiceRoll = 3.0f;
    public float pauseBattleTimer = 3.0f;
    public float pauseVoteTimer = 3.0f;
    public float pauseHpTimer = 3.0f;
    public float pauseWonBattleTimer = 3.0f;


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
    private (bool empate, List<PlayerVoteInput> ganadores)
    CalculateVoteResult(List<PlayerVoteInput> voters)
    {
        Dictionary<PlayerVoteInput, int> voteCount = new Dictionary<PlayerVoteInput, int>();

        // Inicializar
        foreach (var v in voters)
            voteCount[v] = 0;

        // Contar votos
        foreach (var v in voters)
        {
            if (v.chosenVote >= 0 && v.chosenVote < voters.Count)
            {
                PlayerVoteInput votedPlayer = voters[v.chosenVote];
                voteCount[votedPlayer]++;
            }
        }

        // Encontrar máximo
        int maxVotes = -1;
        foreach (var kvp in voteCount)
            if (kvp.Value > maxVotes)
                maxVotes = kvp.Value;

        // Obtener ganadores
        List<PlayerVoteInput> winners = new List<PlayerVoteInput>();
        foreach (var kvp in voteCount)
            if (kvp.Value == maxVotes)
                winners.Add(kvp.Key);

        bool empate = winners.Count > 1;

        return (empate, winners);
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
                battleUIController.AnimateOutroInstant();

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
                        switch (pDI.confirmedDir)
                        {
                            case 's':
                                pDI.pCartImage.SetSur();
                                StartCoroutine(ChoicesSelectedTimer());
                                break;
                            case 'n':
                                pDI.pCartImage.SetNorte();
                                StartCoroutine(ChoicesSelectedTimer());
                                break;
                            case 'e':
                                pDI.pCartImage.SetEste();
                                StartCoroutine(ChoicesSelectedTimer());
                                break;
                            case 'w':
                                pDI.pCartImage.SetOeste();
                                StartCoroutine(ChoicesSelectedTimer());
                                break;
                        }
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

                    playersChosen = false;
                    directionBattleResolved = true;
                }
                // Si estamos esperando inputs de la batalla actual
                if (waitingDirectionBattleInput)
                {
                    battleUIController.HideBattle();
                    if (dirManager.AllPlayersHaveChosen(currentBattlePlayers))
                    {
                        foreach (var p in currentBattlePlayers)
                        {
                            p.confirmInput();
                            switch (p.confirmedDir)
                            {
                                case 's':
                                    p.pCartImage.SetSur();
                                    StartCoroutine(PauseBattleSelectedTimer());
                                    break;
                                case 'n':
                                    p.pCartImage.SetNorte();
                                    StartCoroutine(PauseBattleSelectedTimer());
                                    break;
                                case 'e':
                                    p.pCartImage.SetEste();
                                    StartCoroutine(PauseBattleSelectedTimer());
                                    break;
                                case 'w':
                                    p.pCartImage.SetOeste();
                                    StartCoroutine(ChoicesSelectedTimer());
                                    break;
                            }
                        }
                            

                        waitingDirectionBattleInput = false;
                        currentBattlePlayers.Clear();
                        playersChosen = false;

                        battleGroup.Clear();
                        currentBattlingPlayers.Clear();
                        battle1 = null;
                        battle2 = null;
                        break;

                    }
                    else
                    {
                        break; // seguir esperando
                    }
                }

                // Si no estamos esperando inputs, resolver la siguiente batalla
                if (pendingBattles.Count > 0)
                {
                    if (!playersChosen)
                    {
                        battleGroup = pendingBattles.Peek();

                        currentBattlingPlayers = new List<PlayerBattleInput>();

                        // Elegir el primero
                        int index1 = Random.Range(0, battleGroup.Count);
                        battle1 = battleGroup[index1].gameObject.GetComponent<PlayerBattleInput>();
                        currentBattlingPlayers.Add(battle1);

                        // Elegir el segundo asegurando que no sea el mismo
                        int index2;
                        do
                        {
                            index2 = Random.Range(0, battleGroup.Count);
                        }
                        while (index2 == index1);

                        battle2 = battleGroup[index2].gameObject.GetComponent<PlayerBattleInput>();
                        currentBattlingPlayers.Add(battle2);
                        playersChosen = true;
                        battleUIController.ShowBattle(currentBattlingPlayers);
                        battleManager.ResetAllChoicesBattle();


                    }

                    if (draw)
                    {
                        draw = false;
                        battleUIController.AnimateOutroInstant();

                    }

                    if (battleManager.AllPlayersHaveChosen(currentBattlingPlayers))
                    {
                        battleUIController.AnimateOutroInstant();

                        foreach (var p in currentBattlingPlayers)
                        {
                            p.confirmInput();
                        }

                        char a = battle1.confirmedBattle; 
                        char b = battle2.confirmedBattle;

                        int result = battleManager.RPSResult(a, b);

                        if (result == 0)
                        {
                            // EMPATE ? repetir batalla
                            battleUIController.AnimateIntro(currentBattlingPlayers);
                            StartCoroutine(PauseBattleSelectedTimer());
                            Debug.Log("Empate en batalla RPS. Repitiendo...");
                            draw = true;
                            battleManager.ResetAllChoicesBattle();
                            break; 
                        }

                        // HAY GANADOR
                        PlayerBattleInput winner = (result == 1) ? battle1 : battle2; 
                        PlayerBattleInput loser = (result == 1) ? battle2 : battle1;
                        
                        battleUIController.AnimateIntro(currentBattlingPlayers);
                        StartCoroutine(PauseBattleSelectedTimer());

                        

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
                
                foreach (PlayerDirectionInput pDI in listDirInput)
                {
                    pDI.pCartImage.SetDefault();

                }
                StartCoroutine(ChoicesSelectedTimer());

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

                Debug.Log("=== FASE: RoomResolution ? Resolviendo daño y votaciones en salas ===");

                Dictionary<PlayerHealth, int> takingDmg = new Dictionary<PlayerHealth, int>();

                foreach (var p in playing)
                    p.DeactivateUI();

                //---- Agrupar jugadores por sala ----
                Dictionary<Vector2Int, List<PlayerGridPosition>> rooms = new Dictionary<Vector2Int, List<PlayerGridPosition>>();

                foreach (var p in playing)
                {
                    PlayerGridPosition pg = p.GetComponent<PlayerGridPosition>();

                    if (!rooms.ContainsKey(pg.gridPos))
                        rooms[pg.gridPos] = new List<PlayerGridPosition>();

                    rooms[pg.gridPos].Add(pg);
                }

                //---- Resolver daño/votación por sala ----
                foreach (var kvp in rooms)
                {
                    Vector2Int pos = kvp.Key;
                    List<PlayerGridPosition> playersInRoom = kvp.Value;

                    Tile tile = grid.grid[pos.x, pos.y];

                    // Si solo hay 1 jugador ? daño directo
                    if (playersInRoom.Count == 1)
                    {
                        if (tile.damageValue > 0)
                        {
                            PlayerHealth pHP = playersInRoom[0].GetComponent<PlayerHealth>();
                            pHP.TakeDmg(tile.damageValue);
                            takingDmg[pHP] = tile.damageValue;
                            playing.Remove(pHP);
                        }
                    }
                    else
                    {
                        Debug.Log($"Sala {pos} tiene {playersInRoom.Count} jugadores ? Resolviendo votación");

                        // Obtener PlayerVoteInput de cada jugador
                        List<PlayerVoteInput> voteInputs = new List<PlayerVoteInput>();
                        foreach (var pg in playersInRoom)
                            voteInputs.Add(pg.GetComponent<PlayerVoteInput>());

                        // Primera vez que entramos en votación ? resetear inputs
                        if (!waitingVoteInput)
                        {
                            foreach (var v in voteInputs)
                                v.ResetChoice();

                            waitingVoteInput = true;
                        }

                        // Mostrar UI de votación
                        voteUIController.ShowVoteUI(voteInputs);

                        // Esperar a que todos voten
                        if (!voteInputs.TrueForAll(v => v.hasChosen))
                            return; // Pausa RoomResolution sin repetir daño

                        // Ya votaron todos ? limpiar flag
                        waitingVoteInput = false;

                        voteUIController.HideVoteUI();


                        // Calcular resultado de la votación
                        var result = CalculateVoteResult(voteInputs);

                        if (result.empate)
                        {
                            Debug.Log("Empate en votación ? varios jugadores reciben daño");

                            foreach (var v in result.ganadores)
                            {
                                PlayerHealth pHP = v.gameObject.GetComponent<PlayerHealth>();
                                pHP.TakeDmg(tile.damageValue);
                                takingDmg[pHP] = tile.damageValue;
                                playing.Remove(pHP);
                            }
                        }
                        else
                        {
                            Debug.Log($"Jugador elegido por votación: {result.ganadores[0].name}");

                            PlayerHealth pHP = result.ganadores[0].gameObject.GetComponent<PlayerHealth>();
                            pHP.TakeDmg(tile.damageValue);
                            takingDmg[pHP] = tile.damageValue;
                            playing.Remove(pHP);
                        }
                    }
                }

                // Mostrar UI de daño
                damageReportUI.ShowDamageReport(takingDmg);
                StartCoroutine(PauseShowHpTimer());

                currentPhase = TurnPhase.EndTurn;
                break;



            case TurnPhase.EndTurn:
                damageReportUI.Deactivate();
                Debug.Log("=== FASE: EndTurn ? Turno finalizado, reiniciando ciclo ===");
                currentPhase = TurnPhase.Dice;

                break;
            }
    }
    IEnumerator ChoicesSelectedTimer()
    {
        didGameEnded = true;
        yield return new WaitForSeconds(pauseTimerChoicesSelcted);
        Debug.Log("Choices Selected finished");
        didGameEnded = false;
    }

    IEnumerator PauseBattleSelectedTimer()
    {
        didGameEnded = true;

        yield return new WaitForSeconds(pauseBattleTimer);
        didGameEnded = false;

    }

    IEnumerator PauseVoteSelectedTimer()
    {
        yield return new WaitForSeconds(pauseVoteTimer);
    }

    IEnumerator PauseBattleWonSelectedTimer()
    {
        yield return new WaitForSeconds(pauseWonBattleTimer);
    }

    IEnumerator PauseShowHpTimer()
    {
        didGameEnded = true;

        yield return new WaitForSeconds(pauseHpTimer);
        didGameEnded = false;

    }



}
