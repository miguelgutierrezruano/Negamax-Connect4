using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegaScout : MonoBehaviour
{
    private const int INFINITE = int.MaxValue;
    private const int MINUS_INFINITE = int.MinValue;

    public static int Counter = 0;

    public static int CallNegaScout(Board board, uint maxDepth)
    {
        GameState initState = new GameState(board, 0);

        int alpha = MINUS_INFINITE;
        int beta = INFINITE;

        // Expand node
        var candidates = initState.CreateCandidates();

        // Dictionary with posible actions and their value
        Dictionary<int, int> evaluatedActions = new Dictionary<int, int>();

        // Calculate values for every candidate
        for (var i = 0; i < candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;

            Counter++;

            int candValue = -NegaScoutABValue(cand, alpha, beta, maxDepth);
            evaluatedActions.Add(i, candValue);

            // Update alpha
            if (candValue > alpha)
                alpha = candValue;
        }

        // Return action to execute
        return NegamaxAB.GetBestPair(evaluatedActions).Key;
    }


    private static int NegaScoutABValue(GameState state, int alpha, int beta, uint maxDepth)
    {
        // If state should evaluate return it
        if (state.Suspend())
        {
            if (state.Ply % 2 == 0)
                return state.Evaluate();
            else
                return -state.Evaluate();
        }

        // Keep best value
        int bestValue = int.MinValue;

        int adaptativeBeta = beta;

        var candidates = state.CreateCandidates();

        // Add every possible candidate with its value
        for (var i = 0; i < candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;

            Counter++;

            int candValue = -NegaScoutABValue(cand, -beta, -Mathf.Max(alpha, bestValue), maxDepth);

            if(candValue > bestValue)
            {
                if(adaptativeBeta == beta || state.Ply >= maxDepth - 2)
                {
                    bestValue = candValue;
                }
                // Do a test searching for better plays
                else
                {
                    int testValue = -NegaScoutABValue(cand, -beta, -candValue, maxDepth);
                    bestValue = testValue;
                }

                if (bestValue >= beta)
                    return bestValue;

                adaptativeBeta = Mathf.Max(alpha, bestValue) + 1;
            }
        }

        return bestValue;
    }
}
