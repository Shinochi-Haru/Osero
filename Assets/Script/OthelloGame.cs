using Reversi.Board;
using UnityEngine;

public class OthelloGame : MonoBehaviour
{
    public GameObject blackPiecePrefab; // 黒石のプレハブ
    public GameObject whitePiecePrefab; // 白石のプレハブ
    public int boardSize = 8; // 盤面のサイズ
    private PieceType[,] board; // 盤面の状態
    private PieceType currentPlayer; // 現在のプレイヤー
    [SerializeField] GameObject highlightPrefab;

    private enum PieceType
    {
        Empty,
        Black,
        White
    }

    private void Start()
    {
        InitializeBoard();
        HighlightValidMoves(); // ハイライト表示を行
    }

    private void InitializeBoard()
    {
        board = new PieceType[boardSize, boardSize];
        currentPlayer = PieceType.Black; // 最初の手番は黒石から

        // 盤面の初期化
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i, j] = PieceType.Empty;
            }
        }

        // 初期配置
        board[3, 3] = PieceType.White;
        board[4, 4] = PieceType.White;
        board[3, 4] = PieceType.Black;
        board[4, 3] = PieceType.Black;

        // 石の表示
        InstantiatePiece(3, 3, whitePiecePrefab);
        InstantiatePiece(4, 4, whitePiecePrefab);
        InstantiatePiece(3, 4, blackPiecePrefab);
        InstantiatePiece(4, 3, blackPiecePrefab);
    }

    private void InstantiatePiece(int row, int col, GameObject piecePrefab)
    {
        Vector3 position = new Vector3(row, 0.1f, col);
        Instantiate(piecePrefab, position, Quaternion.identity);
    }

    private bool IsValidMove(int row, int col)
    {
        // 空のマスであるかチェック
        if (board[row, col] != PieceType.Empty)
        {
            return false;
        }

        // 8方向に対してひっくり返せる石があるかチェック
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue; // 自分自身のマスはチェックしない
                }

                if (IsValidDirection(row, col, dx, dy))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsValidDirection(int row, int col, int dx, int dy)
    {
        int x = row + dx;
        int y = col + dy;

        // 相手の石が連続しているかチェック
        if (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == GetOpponentPlayer())
        {
            x += dx;
            y += dy;

            // 自分の石があるかチェック
            while (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == GetOpponentPlayer())
            {
                x += dx;
                y += dy;
            }

            // 自分の石があればひっくり返せる
            if (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == currentPlayer)
            {
                return true;
            }
        }

        return false;
    }

    private void PlacePiece(int row, int col)
    {
        // ひっくり返せる石があるかチェック
        if (!IsValidMove(row, col))
        {
            return;
        }

        // 石を置く
        board[row, col] = currentPlayer;
        InstantiatePiece(row, col, currentPlayer == PieceType.Black ? blackPiecePrefab : whitePiecePrefab);

        // 石をひっくり返す
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                if (IsValidDirection(row, col, dx, dy))
                {
                    FlipPieces(row, col, dx, dy);
                }
            }
        }

        // 手番を交代する
        currentPlayer = GetOpponentPlayer();

        // 敵のAIの手番
        if (currentPlayer == PieceType.White)
        {
            EnemyAI();
        }

        // ハイライト表示を更新する
        HighlightValidMoves();
    }


    private void FlipPieces(int row, int col, int dx, int dy)
    {
        int x = row + dx;
        int y = col + dy;

        while (board[x, y] != currentPlayer)
        {
            // 石をひっくり返す
            board[x, y] = currentPlayer;
            InstantiatePiece(x, y, currentPlayer == PieceType.Black ? blackPiecePrefab : whitePiecePrefab);

            x += dx;
            y += dy;
        }
    }

    private PieceType GetOpponentPlayer()
    {
        return currentPlayer == PieceType.Black ? PieceType.White : PieceType.Black;
    }

    private void EnemyAI()
    {
        // 一番最初の空のマスにランダムに石を置く
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j] == PieceType.Empty && IsValidMove(i, j))
                {
                    PlacePiece(i, j);
                    return;
                }
            }
        }
    }

    private void Update()
    {
        // マウスのクリックを検出して石を置く
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int row = Mathf.FloorToInt(hit.point.x);
                int col = Mathf.FloorToInt(hit.point.z);

                if (row >= 0 && row < boardSize && col >= 0 && col < boardSize)
                {
                    PlacePiece(row, col);
                }
            }
        }
    }
    private void HighlightValidMoves()
    {
        // ハイライト表示されているオブジェクトを全て削除する
        GameObject[] highlights = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject highlight in highlights)
        {
            Destroy(highlight);
        }

        // 盤面上の全てのマスに対して、石が置けるかどうかをチェックする
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (IsValidMove(row, col))
                {
                    // 石が置けるマスの座標にハイライトを表示する処理を追加
                    Vector3 position = new Vector3(row, 0.2f, col);
                    // ハイライト用のプレハブをインスタンス化して表示
                    Instantiate(highlightPrefab, position, Quaternion.identity);
                }
            }
        }
    }


}