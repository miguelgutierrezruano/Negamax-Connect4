using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspirational : MonoBehaviour
{
    // Algorithm can only be called from one team since its static
    static int previousScore =  0;
    static int windowRange   = 20;

    private const int INFINITE = int.MaxValue;
    private const int MINUS_INFINITE = int.MinValue;

    public static int CallAspirationalSearch(Board board)
    {
        int alpha = MINUS_INFINITE;
        int beta = INFINITE;
        int bestPlay = 0;

        if (previousScore != 0)
        {

        }
        else
        {
            //bestPlay = NegamaxAB.CallNegamaxAB(board);
        }

        return bestPlay;
    }
}
