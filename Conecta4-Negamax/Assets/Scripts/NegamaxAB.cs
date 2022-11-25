using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NegamaxAB : MonoBehaviour
{
    public static int MaxPly = 5;
    public static int Counter = 0;

    private const int INFINITE = int.MaxValue;
    private const int MINUS_INFINITE = int.MinValue;

    public static int CallNegamaxAB(Board board)
    {
        GameState initState = new GameState(board, 0);
        int bestAction = NegamaxValueAB(initState, MINUS_INFINITE, INFINITE);

        return bestAction;
    }

    private static int NegamaxValueAB(GameState state, int alpha, int beta)
    {
        // If state should evaluate return it
        if (state.Suspend())
        {
            if (state.Ply % 2 == 0)
                return -state.Evaluate();
            else
                return state.Evaluate();
        }

        // Expand node
        var candidates = state.CreateCandidates();

        // Dictionary with posible actions and their value
        Dictionary<int, int> evaluatedActions = new Dictionary<int, int>();

        // Add every possible candidate with its value
        for (var i = 0; i < candidates.Length; i++)
        {
            // Only expand min nodes

            var cand = candidates[i];
            if (cand == null) continue;

            Counter++;

            int candValue = -NegamaxValueAB(cand, -beta, -alpha);
            evaluatedActions.Add(i, candValue);

            // Change alpha of node if candValue is greater
            if (candValue > alpha)
                alpha = candValue;
        }

        // Select highest
        var nodeKey = evaluatedActions.FirstOrDefault(x => x.Value == evaluatedActions.Values.Max()).Key;
        return nodeKey;
    }

    private int GetBestPlay(Dictionary<int, int> actionsDictionary)
    {
        return actionsDictionary.FirstOrDefault(x => x.Value == actionsDictionary.Values.Max()).Key;
    }

    private int GetBestValue(Dictionary<int, int> actionsDictionary)
    {
        return actionsDictionary.FirstOrDefault(x => x.Value == actionsDictionary.Values.Max()).Value;
    }
}
