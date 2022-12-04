using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardControl : MonoBehaviour {
   
    public Board TheBoard;
    public Player PlayerTurn;

    public Player TheWinner => TheBoard.TheWinner;
    public bool IsFinished => TheBoard.IsFinished;
    public Text TurnText;
    public Text Result;
    public GameObject Buttons;

    [SerializeField] enum Algorithms
    {
        Negamax,
        NegamaxAB
    }

    [SerializeField] enum GameModes
    {
        PlayervsIA,
        IAvsIA
    }

    [Header("Settings")]
    [SerializeField] Algorithms maxAlgorithm;
    [SerializeField] Algorithms minAlgorithm;
    [SerializeField] GameModes  gameMode;
    [SerializeField] uint       gamesToPlay;
    [HideInInspector] public static uint algorithmDepth = 4;

    private uint gameCounter;

    // Use this for initialization
    void Start () {
        TheBoard = new Board();
        PlayerTurn = Player.MAX;
        gameCounter = 0;
	}
	
    public int AddToken(int column) {
        if (TheBoard.HasEmptyCells(column))
        {
            var res = TheBoard.AddToken(column, PlayerTurn);
            PlayerTurn = (PlayerTurn == Player.MAX) ? Player.MIN : Player.MAX;
            return res;
        }
        return -1;
    }

    private void Update()
    {
        TurnText.text = PlayerTurn.ToString();

        if(gameMode == GameModes.PlayervsIA)
        {
            if (PlayerTurn == Player.MAX)
            {
                int columnSelected = CallAlgorithm(PlayerTurn);
                int row = AddToken(columnSelected);
                var TheGridTokens = FindObjectOfType<GridTokens>();
                TheGridTokens.TokenControls[columnSelected, row].SetPlayer(Player.MAX);
            }
        }
        else
        {
            if(!IsFinished)
            {
                Buttons.SetActive(false);
                int columnSelected = CallAlgorithm(PlayerTurn);
                int row = AddToken(columnSelected);
                var TheGridTokens = FindObjectOfType<GridTokens>();
                TheGridTokens.TokenControls[columnSelected, row].SetPlayer(PlayerTurn);
            }
        }

        if (IsFinished)
        {
            Buttons.SetActive(false);
            Result.gameObject.SetActive(true);
            Result.text = $"{TheWinner.ToString()} Wins";
            TurnText.gameObject.SetActive(false);

            ++gameCounter;
            if(gameCounter < gamesToPlay && gameMode == GameModes.IAvsIA)
            {
                // Clean UI
                var TheGridTokens = FindObjectOfType<GridTokens>();
                foreach (var token in TheGridTokens.TokenControls)
                    token.SetNone();

                // Clean values
                for (var i = 0; i < 8; i++)
                {
                    for (var j = 0; j < 7; j++)
                    {
                        TheBoard.Columns[i, j] = Player.NONE;
                    }
                }

                TheBoard.IsFinished = false;
            }
        }
    }

    private int CallAlgorithm(Player _playerTurn)
    {
        if (_playerTurn == Player.MAX)
            return FilterAlgorithm(maxAlgorithm);
        else
            return FilterAlgorithm(minAlgorithm);
    }

    private int FilterAlgorithm(Algorithms aiAlgorithm)
    {
        int column = 0;

        switch (aiAlgorithm)
        {
            case Algorithms.Negamax:
                column = Negamax.CallNegamax(TheBoard);
                Debug.Log($"Column Selected {column} : Nodes = {Negamax.Counter}");
                Negamax.Counter = 0;

                break;

            case Algorithms.NegamaxAB:
                column = NegamaxAB.CallNegamaxAB(TheBoard).Key;
                Debug.Log($"Column Selected {column} : Nodes = {NegamaxAB.Counter}");
                NegamaxAB.Counter = 0;

                break;
        }

        return column;
    }
}

public enum Player { NONE, MAX, MIN }

public class Board
{
    public Player TheWinner;
    public bool IsFinished;

    public Player[,] Columns;
    public int NumTokens = 0;

    public Board()
    {
        Columns = new Player[8, 7];
        NumTokens = 0;
        TheWinner = Player.NONE;
        IsFinished = false;
    }

    public bool HasEmptyCells(int column)
    {
        return Columns[column, 6] == Player.NONE;
    }

    public bool IsEmpty(int column)
    {
        return Columns[column, 0] == Player.NONE;
    }

    public int AddToken(int column, Player ply)
    {
        var summit = FindSummit(column);
        Columns[column, summit ] = ply;
        NumTokens++;

        if (HasDraw())
        {
            TheWinner = Player.NONE;
            IsFinished = true;
        }
        else
        {
            switch (ply)
            {
                case Player.MAX:
                    if (HasWinMax())
                    {
                        TheWinner = Player.MAX;
                        IsFinished = true;
                    }
                    break;
                case Player.MIN:
                    {
                        if (HasWinMin())
                        {
                            TheWinner = Player.MIN;
                            IsFinished = true;
                        }
                    }
                    break;
            }
        }
        return summit;
    }

    public int FindSummit(int column)
    {
        var i = -1;
        while (Columns[column, ++i] != Player.NONE) ;
        return i;
    }

    public bool HasWinMax()
    {
        return Winner() == Player.MAX;
    }
    public bool HasWinMin()
    {
        return Winner() == Player.MIN;
    }
    public bool HasDraw()
    {
        return Winner() == Player.NONE && NumTokens==8*7;
    }

    public Player Winner()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 7; j++)
            {
                var res =IsLineStartingAt(i, j);
                if (res != Player.NONE)
                    return res;
            }
        }
        return Player.NONE;
    }
    bool IsLinearMatch(int x, int y, int stepX, int stepY)
    {
        /* Get the value of the start position. */
        Player startValue = Columns[x,y];

        /* Confirm the two values after it match. */
        for (int i = 1; i < 4; ++i)
        {
            var newCol = x + i * stepX;
            var newRow = y + i * stepY;
            if ( newCol> 7 || newCol<0 || newRow >6 || newRow<0)
                return false;
            if (Columns[newCol, newRow] != startValue)
                return false;
        }
        /* If we got here, then they all match! */
        return true;
    }

    Player IsLineStartingAt(int x, int y)
    {
        if (Columns[x, y] == Player.NONE) return Player.NONE;
        if (IsLinearMatch(x, y, 1, 0) ||  // Horizontal
               IsLinearMatch(x, y, 0, 1) ||  // Vertical
               IsLinearMatch(x, y, 1, 1) ||  // Diagonal Down
               IsLinearMatch(x, y, 1, -1))    // Diagonal Up
            return Columns[x, y];
        return Player.NONE;
    }
}