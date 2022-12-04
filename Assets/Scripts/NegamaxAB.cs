using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NegamaxAB : MonoBehaviour
{
    public static int Counter = 0;

    private const int INFINITE = int.MaxValue;
    private const int MINUS_INFINITE = int.MinValue;

    public static KeyValuePair<int, int> CallNegamaxAB(Board board, int alpha = MINUS_INFINITE, int beta = INFINITE)
    {
        GameState initState = new GameState(board, 0);

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

            int candValue = -NegamaxValueAB(cand, alpha, beta);
            evaluatedActions.Add(i, candValue);

            // Update alpha
            if (candValue > alpha)
                alpha = candValue;
        }

        // Return action to execute
        return GetBestPair(evaluatedActions);
    }

    private static int NegamaxValueAB(GameState state, int alpha, int beta)
    {
        // If state should evaluate return it
        if (state.Suspend())
        {
            if (state.Ply % 2 == 0)
                return  state.Evaluate();
            else
                return -state.Evaluate();
        }

        int nodeValue = MINUS_INFINITE;

        // Expand node
        var candidates = state.CreateCandidates();

        // Add every possible candidate with its value
        for (var i = 0; i < candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;

            Counter++;

            int candValue = -NegamaxValueAB(cand, -beta, -Mathf.Max(alpha, nodeValue));

            // Update nodeValue
            if (candValue > nodeValue)
                nodeValue = candValue;

            if (nodeValue >= beta)
                return nodeValue;
        }

        // Return highest
        return nodeValue;
    }

    private static KeyValuePair<int, int> GetBestPair(Dictionary<int, int> actionsDictionary)
    {
        return actionsDictionary.FirstOrDefault(x => x.Value == actionsDictionary.Values.Max());
    }
}
