using Reversi.Board;
using UnityEngine;

public class OthelloGame : MonoBehaviour
{
    public GameObject blackPiecePrefab; // ���΂̃v���n�u
    public GameObject whitePiecePrefab; // ���΂̃v���n�u
    public int boardSize = 8; // �Ֆʂ̃T�C�Y
    private PieceType[,] board; // �Ֆʂ̏��
    private PieceType currentPlayer; // ���݂̃v���C���[
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
        HighlightValidMoves(); // �n�C���C�g�\�����s
    }

    private void InitializeBoard()
    {
        board = new PieceType[boardSize, boardSize];
        currentPlayer = PieceType.Black; // �ŏ��̎�Ԃ͍��΂���

        // �Ֆʂ̏�����
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i, j] = PieceType.Empty;
            }
        }

        // �����z�u
        board[3, 3] = PieceType.White;
        board[4, 4] = PieceType.White;
        board[3, 4] = PieceType.Black;
        board[4, 3] = PieceType.Black;

        // �΂̕\��
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
        // ��̃}�X�ł��邩�`�F�b�N
        if (board[row, col] != PieceType.Empty)
        {
            return false;
        }

        // 8�����ɑ΂��ĂЂ�����Ԃ���΂����邩�`�F�b�N
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue; // �������g�̃}�X�̓`�F�b�N���Ȃ�
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

        // ����̐΂��A�����Ă��邩�`�F�b�N
        if (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == GetOpponentPlayer())
        {
            x += dx;
            y += dy;

            // �����̐΂����邩�`�F�b�N
            while (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == GetOpponentPlayer())
            {
                x += dx;
                y += dy;
            }

            // �����̐΂�����΂Ђ�����Ԃ���
            if (x >= 0 && x < boardSize && y >= 0 && y < boardSize && board[x, y] == currentPlayer)
            {
                return true;
            }
        }

        return false;
    }

    private void PlacePiece(int row, int col)
    {
        // �Ђ�����Ԃ���΂����邩�`�F�b�N
        if (!IsValidMove(row, col))
        {
            return;
        }

        // �΂�u��
        board[row, col] = currentPlayer;
        InstantiatePiece(row, col, currentPlayer == PieceType.Black ? blackPiecePrefab : whitePiecePrefab);

        // �΂��Ђ�����Ԃ�
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

        // ��Ԃ���シ��
        currentPlayer = GetOpponentPlayer();

        // �G��AI�̎��
        if (currentPlayer == PieceType.White)
        {
            EnemyAI();
        }

        // �n�C���C�g�\�����X�V����
        HighlightValidMoves();
    }


    private void FlipPieces(int row, int col, int dx, int dy)
    {
        int x = row + dx;
        int y = col + dy;

        while (board[x, y] != currentPlayer)
        {
            // �΂��Ђ�����Ԃ�
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
        // ��ԍŏ��̋�̃}�X�Ƀ����_���ɐ΂�u��
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
        // �}�E�X�̃N���b�N�����o���Đ΂�u��
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
        // �n�C���C�g�\������Ă���I�u�W�F�N�g��S�č폜����
        GameObject[] highlights = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject highlight in highlights)
        {
            Destroy(highlight);
        }

        // �Ֆʏ�̑S�Ẵ}�X�ɑ΂��āA�΂��u���邩�ǂ������`�F�b�N����
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (IsValidMove(row, col))
                {
                    // �΂��u����}�X�̍��W�Ƀn�C���C�g��\�����鏈����ǉ�
                    Vector3 position = new Vector3(row, 0.2f, col);
                    // �n�C���C�g�p�̃v���n�u���C���X�^���X�����ĕ\��
                    Instantiate(highlightPrefab, position, Quaternion.identity);
                }
            }
        }
    }


}