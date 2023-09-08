using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCont : MonoBehaviour
{
    private OthelloGame othelloGame; // OthelloGame�X�N���v�g�ւ̎Q��

    private void Start()
    {
        othelloGame = FindObjectOfType<OthelloGame>(); // OthelloGame�X�N���v�g��T��
    }

    private void OnMouseDown()
    {
        // �Z�����N���b�N���ꂽ�Ƃ��̏���
        int row = Mathf.RoundToInt(transform.position.z / othelloGame.spacing);
        int col = Mathf.RoundToInt(transform.position.x / othelloGame.spacing);
        othelloGame.PlacePiece(row, col);
    }

}
