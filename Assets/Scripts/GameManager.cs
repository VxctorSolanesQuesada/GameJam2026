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




            //Fase mostrar sala



            //Fase votación (inputs: lista de player / output: player que ha perdido) 



            //Restar puntos


        }
    }
}
