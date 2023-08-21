using UnityEngine;
using System.Collections.Generic;

public class OthelloAI : MonoBehaviour
{
    private OthelloGame othelloGame;
    private int enemyPieceType = 2; // �G�̐΂̎�ށi���j

    private void Start()
    {
        othelloGame = FindObjectOfType<OthelloGame>();
    }

    public void MakeMove()
    {
        List<Vector2Int> validMoves = GetValidMoves();

        if (validMoves.Count > 0)
        {
            // �����őI�񂾎�����ۂɒu���������s��
            Vector2Int bestMove = ChooseBestMove(validMoves);
           // othelloGame.PlacePiece(bestMove.x, bestMove.y, enemyPieceType);
        }
    }

    List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        for (int row = 0; row < othelloGame.rows; row++)
        { 
        //    for (int col = 0; col < othelloGame.columns; col++)
        //    //{
        //    //    if (othelloGame.CanPlacePiece(row, col, enemyPieceType))
        //    //    {
        //    //        validMoves.Add(new Vector2Int(row, col));
        //    //    }
        //    //}
        }

        return validMoves;
    }

    Vector2Int ChooseBestMove(List<Vector2Int> validMoves)
    {
        // ���Ƀ����_���Ɏ��I�ԗ�
        int randomIndex = Random.Range(0, validMoves.Count);
        return validMoves[randomIndex];
    }
}
