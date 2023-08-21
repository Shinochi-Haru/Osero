using UnityEngine;

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

        Vector2Int[] directions = {
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0),
            new Vector2Int(-1, 1), new Vector2Int(1, 1),
            new Vector2Int(-1, -1), new Vector2Int(1, -1)
        };

        foreach (Vector2Int dir in directions)
        {
            int r = row + dir.y;
            int c = col + dir.x;
            bool validDirection = false;
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
                    validDirection = true;
                    break;
                }

                r += dir.y;
                c += dir.x;
            }

            if (validDirection)
            {
                r = row + dir.y;
                c = col + dir.x;

                while (initialPieces[r, c] == opponentPieceType)
                {
                    initialPieces[r, c] = pieceType;

                    Vector3 piecePosition = grid[r, c].transform.position;
                    Quaternion rotation = Quaternion.identity;
                    GameObject newPiecePrefab = pieceType == 1 ? blackPiecePrefab : whitePiecePrefab;
                    Instantiate(newPiecePrefab, piecePosition, rotation);

                    r += dir.y;
                    c += dir.x;
                }
            }
        }
    }

    public void PlacePiece(int row, int col)
    {
        if (initialPieces[row, col] != 0)
            return;

        int pieceType = 1;
        initialPieces[row, col] = pieceType;
        FlipPieces(row, col, pieceType);
        PlacePiece(row, col, pieceType); // ‚±‚±‚ÅPlacePiece‚ðŒÄ‚Ño‚µ‚ÄÎ‚ð•\Ž¦
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
}
