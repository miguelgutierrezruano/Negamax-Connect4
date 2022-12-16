using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspirational : MonoBehaviour
{
    // Algorithm can only be called from one team since its static
    public static int previousScore =  0;
    static int windowRange   = 40;

    private const int INFINITE = int.MaxValue;
    private const int MINUS_INFINITE = int.MinValue;

    public static List<double> executionTimings = new List<double>();

    public static int CallAspirationalSearch(Board board)
    {
        double chrono = Time.realtimeSinceStartup;

        int alpha = MINUS_INFINITE;
        int beta = INFINITE;
        KeyValuePair<int, int> playValuePair;

        if (previousScore != 0)
        {
            alpha = previousScore - windowRange;
            beta  = previousScore + windowRange;

            while(true)
            {
                playValuePair = NegamaxAB.CallNegamaxAB(board, alpha, beta);
                if (playValuePair.Value <= alpha)
                    alpha = MINUS_INFINITE;
                else if (playValuePair.Value >= beta)
                    beta = INFINITE;
                else
                    break;
            }

            previousScore = playValuePair.Value;
        }
        else
        {
            playValuePair = NegamaxAB.CallNegamaxAB(board);
            previousScore = playValuePair.Value;
        }

        double elapsedTime = Time.realtimeSinceStartup - chrono;
        executionTimings.Add(elapsedTime);

        return playValuePair.Key;
    }
}
