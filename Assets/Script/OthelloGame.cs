using UnityEngine;
using System.Collections.Generic;
using System;

public class OthelloGame : MonoBehaviour
{
    [SerializeField] public GameObject CellPrefab;
    [SerializeField] public GameObject blackPiecePrefab;
    [SerializeField] public GameObject whitePiecePrefab;
    [SerializeField] public float spacing = 1.0f;
    [SerializeField] public int rows = 8;
    [SerializeField] public int columns = 8;

    private GameObject[,] grid;
    private int[,] initialPieces;
    private int currentPlayer = 1; // 1: ���A2: ��

    private System.Random random = new System.Random(); // �����_���������p�̃I�u�W�F�N�g

    private Vector2Int[] directions = {
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(-1, 1), new Vector2Int(1, 1),
        new Vector2Int(-1, -1), new Vector2Int(1, -1)
    };

    private void Start()
    {
        InitializeGrid();
        CreateGrid();
        SetupInitialPieces();
    }

    void InitializeGrid()
    {
        grid = new GameObject[rows, columns];
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * spacing, 0, row * spacing);
                GameObject cell = Instantiate(CellPrefab, position, Quaternion.identity);
                grid[row, col] = cell;
            }
        }
    }

    void PlacePiece(int row, int col, int pieceType)
    {
        GameObject piecePrefab = pieceType == 1 ? blackPiecePrefab : pieceType == 2 ? whitePiecePrefab : null;

        if (piecePrefab != null)
        {
            Vector3 piecePosition = grid[row, col].transform.position;
            Quaternion rotation = Quaternion.identity;
            Instantiate(piecePrefab, piecePosition, rotation);
        }
    }

    void FlipPieces(int row, int col, int pieceType)
    {
        int opponentPieceType = pieceType == 1 ? 2 : 1;

        foreach (Vector2Int dir in directions)
        {
            int r = row + dir.y;
            int c = col + dir.x;
            bool validDirection = false;
            bool foundOpponentPiece = false;

            List<Vector2Int> piecesToFlip = new List<Vector2Int>(); // �Ђ�����Ԃ��Ώۂ̐΂�ێ����郊�X�g

            while (r >= 0 && r < rows && c >= 0 && c < columns)
            {
                int cellPieceType = initialPieces[r, c];

                if (cellPieceType == 0)
                    break;

                if (cellPieceType == opponentPieceType)
                    foundOpponentPiece = true;

                if (cellPieceType == pieceType)
                {
                    validDirection = true;
                    break;
                }

                if (foundOpponentPiece)
                {
                    // ���܂ꂽ�΂����X�g�ɒǉ�
                    piecesToFlip.Add(new Vector2Int(r, c));
                }

                r += dir.y;
                c += dir.x;
            }

            if (validDirection)
            {
                // ���܂ꂽ�΂��Ђ�����Ԃ�
                foreach (Vector2Int pieceToFlip in piecesToFlip)
                {
                    initialPieces[pieceToFlip.x, pieceToFlip.y] = pieceType;

                    Vector3 piecePosition = grid[pieceToFlip.x, pieceToFlip.y].transform.position;
                    Quaternion rotation = Quaternion.identity;
                    GameObject newPiecePrefab = pieceType == 1 ? blackPiecePrefab : whitePiecePrefab;
                    Instantiate(newPiecePrefab, piecePosition, rotation);
                }
            }
        }
    }


    public void PlacePiece(int row, int col)
    {
        if (initialPieces[row, col] != 0)
            return;

        int pieceType = currentPlayer;
        int opponentPieceType = 3 - pieceType; // ����̐΂̎��

        bool isValidMove = false;

        foreach (Vector2Int dir in directions)
        {
            int r = row + dir.y;
            int c = col + dir.x;
            bool foundOpponentPiece = false;

            while (r >= 0 && r < rows && c >= 0 && c < columns)
            {
                int cellPieceType = initialPieces[r, c];

                if (cellPieceType == 0)
                    break;

                if (cellPieceType == opponentPieceType)
                    foundOpponentPiece = true;

                if (cellPieceType == pieceType && foundOpponentPiece)
                {
                    isValidMove = true;
                    break;
                }

                r += dir.y;
                c += dir.x;
            }

            if (isValidMove)
            {
                break;
            }
        }

        if (isValidMove)
        {
            initialPieces[row, col] = pieceType;
            PlacePiece(row, col, pieceType);
            FlipPieces(row, col, pieceType);

            // �^�[�������
            currentPlayer = 3 - currentPlayer;
        }
    }

    void SetupInitialPieces()
    {
        initialPieces = new int[,]
        {
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 1, 2, 0, 0, 0},
        {0, 0, 0, 2, 1, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0}
        };

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int pieceType = initialPieces[row, col];
                PlacePiece(row, col, pieceType);
            }
        }
    }


    private List<Vector2Int> FindLegalMoves(int player)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (initialPieces[row, col] == 0)
                {
                    if (IsMoveLegal(row, col, player))
                    {
                        legalMoves.Add(new Vector2Int(row, col));
                    }
                }
            }
        }

        return legalMoves;
    }

    private bool IsMoveLegal(int row, int col, int player)
    {
        int opponentPieceType = 3 - player;

        foreach (Vector2Int dir in directions)
        {
            int r = row + dir.y;
            int c = col + dir.x;
            bool foundOpponentPiece = false;

            while (r >= 0 && r < rows && c >= 0 && c < columns)
            {
                int cellPieceType = initialPieces[r, c];

                if (cellPieceType == 0)
                    break;

                if (cellPieceType == opponentPieceType)
                    foundOpponentPiece = true;

                if (cellPieceType == player && foundOpponentPiece)
                {
                    return true;
                }

                r += dir.y;
                c += dir.x;
            }
        }

        return false;
    }

    private void Update()
    {
        // ���݂̃v���C���[��AI�̏ꍇ�A���@�Ȏ��I��
        if (currentPlayer == 2) // AI��2�Ԗڂ̃v���C���[�Ɖ���
        {
            List<Vector2Int> legalMoves = FindLegalMoves(currentPlayer);
            if (legalMoves.Count > 0)
            {
                // AI�����@�Ȏ��I��
                Vector2Int bestMove = (Vector2Int)ChooseBestMove(legalMoves, currentPlayer);

                // ���@�Ȏ��I�񂾏ꍇ�ɂ̂ݐ΂�u��
                if (bestMove != null)
                {
                    PlacePiece(bestMove.x, bestMove.y);
                    FlipPieces(bestMove.x, bestMove.y, currentPlayer); // �΂��Ђ�����Ԃ�
                }
            }
        }
    }

    // ���@�Ȏ�̒�����AI���œK�Ȏ��I�ԃ��W�b�N��ǉ�
    private Vector2Int? ChooseBestMove(List<Vector2Int> legalMoves, int player)
    {
        // �����ɍœK�Ȏ�̑I�����W�b�N���������܂��B
        // ���̗�ł̓����_���Ȏ��I�т܂����AAI�̐헪�ɉ����ĕύX���Ă��������B

        if (legalMoves.Count > 0)
        {
            int randomIndex = random.Next(0, legalMoves.Count);
            return legalMoves[randomIndex];
        }

        return null; // ���@�Ȏ肪�Ȃ��ꍇ
    }

}
