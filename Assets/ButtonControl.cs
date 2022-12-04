using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {
    
    private int ColumnId;
    private BoardControl TheBoardControl;
    private GridTokens TheGridTokens;
    // Use this for initialization
	void Start () {
        ColumnId = int.Parse(GetComponentInChildren<Text>().text)-1;
        TheBoardControl = FindObjectOfType<BoardControl>();
        TheGridTokens = FindObjectOfType<GridTokens>();
	}
	
	public void AddToken()
    {
        if (TheBoardControl.TheBoard.HasEmptyCells(ColumnId))
        {
            var currentPlayer= TheBoardControl.PlayerTurn;
            
            var rowPos = TheBoardControl.AddToken(ColumnId);
            TheGridTokens.TokenControls[ColumnId, rowPos].SetPlayer(currentPlayer);
            
        }
        else
        {
            Debug.Log("Not Empty! " + ColumnId);
        }
    }
}
