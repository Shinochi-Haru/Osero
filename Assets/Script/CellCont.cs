using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCont : MonoBehaviour
{
    private OthelloGame othelloGame; // OthelloGameスクリプトへの参照

    private void Start()
    {
        othelloGame = FindObjectOfType<OthelloGame>(); // OthelloGameスクリプトを探す
    }

    private void OnMouseDown()
    {
        // セルがクリックされたときの処理
        int row = Mathf.RoundToInt(transform.position.z / othelloGame.spacing);
        int col = Mathf.RoundToInt(transform.position.x / othelloGame.spacing);
        othelloGame.PlacePiece(row, col);
    }

}
