using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MiniMax
{
    public static int MaxPly = 2;
    private static int BestAction;

    public static int Counter = 0;
    public static int CallMiniMax(Board board)
    {
        Counter = 0;
        GameState initState = new GameState(board, 0);
        var res = MaxValor(initState);

        return BestAction;
    }

    private static int MaxValor(GameState state)
    {
        if (state.Suspend())
        {
            return state.Evaluate();
        }
        var candidates = state.CreateCandidates();
        var alpha = -1000;
        for (var i = 0; i< candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;
            //Debug Metrics
            Counter++;

            var newAlpha = MinValor(cand);
            if (newAlpha > alpha)
            {
                BestAction = i;
                alpha = newAlpha;
            }
        }
        return alpha;
    }
    private static int MinValor(GameState state)
    {
        if (state.Suspend())
        {
            return state.Evaluate();
        }
        var candidates = state.CreateCandidates();
        var beta = 1000;
        for (var i = 0; i < candidates.Length; i++)
        {
            var cand = candidates[i];
            if (cand == null) continue;

            //Debug Metrics
            Counter++;

            var newBeta = MaxValor(cand);
            if (newBeta < beta)
            {
                
                beta = newBeta;
            }
        }

        
        return beta;
    }
}

public class GameState
{
    public Board Board;
    public int Ply;

    public GameState(Board parentBoard, int ply)
    {
        Board = new Board();
        Board.NumTokens = parentBoard.NumTokens;

        Board.Columns = new Player[8, 7];
        Array.Copy(parentBoard.Columns, 0, Board.Columns, 0, 8 * 7);

        Ply = ply;
    }

    public void Simulate(int column)
    {
        Board.AddToken(column, (Ply % 2 == 0) ? Player.MAX : Player.MIN);
    }

    public bool Suspend()
    {
        return (Board.IsFinished || Ply == NegamaxAB.MaxPly);
    }

    public int Evaluate()
    {
        if (Board.IsFinished)
        {
            if (Board.HasDraw()) return 0;
            if (Board.HasWinMax()) return +1000;
            if (Board.HasWinMin()) return -1000;
        }

        int gameStateValue = 0;

        // Iterate columns
        for (int i = 0; i < 8; i++)
        {
            // Skip empty columns
            if (Board.IsEmpty(i)) continue;

            // Iterate rows of column
            for (int j = 0; j < 7; j++)
            {
                // Skip empty cells
                if (Board.Columns[i, j] == Player.NONE) continue;

                gameStateValue += SearchPlayVertical(Board.Columns[i, j], i, j);
            }
        }

        return gameStateValue;
    }

    private int SearchPlayVertical(Player player, int x, int y)
    {
        int upPlay = 0;
        int downPlay = 0;

        SearchInSteps(player, ref upPlay, 0, x, y, 0, 1);
        SearchInSteps(player, ref downPlay, 0, x, y, 0, -1);

        return upPlay + downPlay;
    }

    // Increment value of play recursively
    private void SearchInSteps(Player player, ref int value, int distanceFromStart, int x, int y, int xStep, int yStep)
    {
        // Stop if reached limit
        if (distanceFromStart >= 4)
            return;

        //switch()
    }

    //Null is invalid action.
    public GameState[] CreateCandidates()
    {
        var res = new GameState[8];
        for (int i = 0; i < 8; i++)
        {
            if (!Board.IsEmpty(i))
            {
                res[i] = null;
                continue;
            }
            res[i] = new GameState(Board, Ply + 1);
            res[i].Board.AddToken(i, ((Ply + 1) % 2 == 0)?Player.MIN:Player.MAX);
        }
        return res;
    }
}
