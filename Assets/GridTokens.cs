using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTokens : MonoBehaviour {

    public GameObject TokenPrefab;
    public TokenControl[,] TokenControls;
	// Use this for initialization
	void Start () {
        TokenControls = new TokenControl[8 , 7];
		for (var i = 0; i < 8 ; i++)
        {
            for (var j = 0; j < 7; j++)
            {
                var Go = GameObject.Instantiate(TokenPrefab, transform);
                Go.name = "Token_" + i+"_"+j;
                TokenControls[i, j] = Go.GetComponent<TokenControl>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
