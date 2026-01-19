using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    private bool didGameEnded = false;
    private int[] dirValues = new int[4]; // 0 = N, 1 = S, 2 = E, 3 = W
    private char[] playerDir = new char[4]; // 0 = p1, 1 = p2, 2 = p3, 3 = p4

    private List<GameObject> loserPlayers = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while (didGameEnded)
        {
            //Fase dados = direcionts values
            dirValues = RandomDirections.Instance.RegenerateDirections();

            //Fase eleccion dir de cada jugador



            int countS = 0; int countN = 0; int countE = 0; int countW = 0;

            char[] chars = { playerDir[0], playerDir[1], playerDir[2], playerDir[3] };

            foreach (char c in chars)
            {
                switch (char.ToLower(c))
                {
                    case 's': countS++; break;
                    case 'n': countN++; break;
                    case 'e': countE++; break;
                    case 'w': countW++; break;
                }
            }

            if (countN > dirValues[0])
            {
                //Fase batalla con jugadores con playerDir = n



            }
            if (countS > dirValues[1])
            {
                //Fase batalla con jugadores con playerDir = s



            }
            if (countE > dirValues[2])
            {
                //Fase batalla con jugadores con playerDir = E



            }
            if (countW > dirValues[3])
            {
                //Fase batalla con jugadores con playerDir = W



            }

            //Fase eleccion dir de jugador/es que perdieron



            //Fase mover




            //Fase mostrar sala




            //Fase votación




            //Si empate en votación, batalla




            //Restar puntos


        }
    }
}
