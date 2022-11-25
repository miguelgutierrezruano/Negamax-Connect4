using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenControl : MonoBehaviour {

    public Color MaxColor = Color.red;
    public Color MinColor = Color.blue;
    public Color NoneColor = Color.white;
    private Image _TheImage;
    Player CurrentPlayer;
    public void Start()
    {
        _TheImage = GetComponent<Image>();
        CurrentPlayer = Player.NONE;
    }
    public void SetPlayer(Player player)
    {
        switch (player)
        {
            case Player.MAX:
                SetMax();
                break;
            case Player.MIN:
                SetMin();
                break;
            case Player.NONE:
                SetNone();
                break;

        }
    }
    public void SetMax()
    {
        CurrentPlayer = Player.MAX;
        _TheImage.color = MaxColor;
    }
    public void SetMin()
    {
        CurrentPlayer = Player.MIN;
        _TheImage.color = MinColor;
    }
    public void SetNone()
    {
        CurrentPlayer = Player.NONE;
        _TheImage.color = NoneColor;
    }

}
