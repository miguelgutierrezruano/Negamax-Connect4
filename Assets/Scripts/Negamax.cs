using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Negamax
{
    public static int MaxPly = 3;
    public static int Counter = 0;

    public static int CallNegamax(Board board)
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

            int candValue = -NegamaxValue(cand);
            evaluatedActions.Add(i, candValue);
        }

        // Return action to execute
        int bestAction = evaluatedActions.FirstOrDefault(x => x.Value == evaluatedActions.Values.Max()).Key;
        return bestAction;
    }

    private static int NegamaxValue(GameState state)
    {
        // If state should evaluate return it
        if(state.Suspend())
        {
            if (state.Ply % 2 == 0)
                return  state.Evaluate();
            else
                return -state.Evaluate();
        }

        // Expand node
        var candidates = state.CreateCandidates();

        // Dictionary with posible actions and their value
        Dictionary<int, int> evaluatedActions = new Dictionary<int, int>();

        // Add every possible candidate with its value
        for (var i = 0; i < candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;

            Counter++;

            int candValue = -NegamaxValue(cand);
            evaluatedActions.Add(i, candValue);
        }

        // Select highest
        var nodeValue = evaluatedActions.FirstOrDefault(x => x.Value == evaluatedActions.Values.Max()).Value;
        return nodeValue;
    }
}
