using UnityEngine;

public class OthelloGame : MonoBehaviour
{
    [SerializeField] public GameObject CellPrefab;
    [SerializeField] public GameObject blackPiecePrefab; // ���΂̃v���t�@�u
    [SerializeField] public GameObject whitePiecePrefab; // ���΂̃v���t�@�u
    [SerializeField] public float spacing = 1.0f; // �I�u�W�F�N�g�Ԃ̊Ԋu
    [SerializeField] public int rows = 8; // �s��
    [SerializeField] public int columns = 8; // ��

    private GameObject[,] grid; // �O���b�h�̓񎟌��z��

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
            Quaternion rotation = pieceType == 1 ? Quaternion.Euler(180, 0, 0) : Quaternion.identity;
            Instantiate(piecePrefab, piecePosition, rotation);
        }
    }
    public void PlacePiece(int row, int col)
    {
        // ���΂�u���������s��
        Vector3 piecePosition = grid[row, col].transform.position;
        Quaternion rotation = Quaternion.identity;
        Instantiate(blackPiecePrefab, piecePosition, rotation);
    }

    void SetupInitialPieces()
    {
        // �����z�u�̃f�[�^�i1�����΁A2�����΂������j
        int[,] initialPieces = {
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

}
