using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sample2 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private int _row = 5;
    [SerializeField]private int _column = 5;
    [SerializeField]private Text _movesText; // �萔��\������e�L�X�g�v�f
    [SerializeField]private Text _timeText; // ���Ԃ�\������e�L�X�g�v�f
    private Image[,] _cells;
    private bool _gameInProgress;
    private int _moves;
    private float _startTime;

    private void Start()
    {
        // �Q�[���J�n���̎萔�Ǝ��Ԃ�������
        _moves = 0;
        _startTime = Time.time;
        _cells = new Image[_row, _column];

        // �Q�[�����i�s�����������t���O��������
        _gameInProgress = false;

        // �Q�[���J�n���̎萔�Ǝ��Ԃ�������
        _moves = 0;
        _startTime = Time.time;

        // �Z�����쐬�������_���ȐF�ŏ�����
        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                var cell = new GameObject($"Cell({r}, {c})");
                cell.transform.parent = transform;
                var image = cell.AddComponent<Image>();

                // �Z���̐F�������_���ɏ�����
                image.color = GetRandomColor();

                _cells[r, c] = image;
            }
        }
        UpdateMovesText();
        UpdateTimeText();
    }

    private void Update()
    {
        // �Q�[�����i�s���ł���΁A�N���A������s��
        if (_gameInProgress)
        {
            if (CheckGameClear())
            {
                _gameInProgress = false;
                float elapsedTime = Time.time - _startTime;
            }
            // ���Ԃ̃e�L�X�g���X�V
            UpdateTimeText();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject;
        var image = cell.GetComponent<Image>();

        if (!_gameInProgress)
        {
            _gameInProgress = true;
            _startTime = Time.time;
        }

        // �N���b�N�����Z���̍��W���擾
        int clickedRow = -1;
        int clickedColumn = -1;

        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                if (_cells[r, c] == image)
                {
                    clickedRow = r;
                    clickedColumn = c;
                    break;
                }
            }
        }

        // �N���b�N�����Z���Ƃ��̎��͂̃Z���̐F�𔽓]
        FlipCellColor(clickedRow, clickedColumn);
        FlipCellColor(clickedRow - 1, clickedColumn); // ��̃Z��
        FlipCellColor(clickedRow + 1, clickedColumn); // ���̃Z��
        FlipCellColor(clickedRow, clickedColumn - 1); // ���̃Z��
        FlipCellColor(clickedRow, clickedColumn + 1); // �E�̃Z��


        // �萔���C���N�������g
        _moves++;

        // �萔�̃e�L�X�g���X�V
        UpdateMovesText();
    }

    private void UpdateMovesText()
    {
        _movesText.text = "�萔: " + _moves.ToString();
    }

    private void UpdateTimeText()
    {
        float elapsedTime = Time.time - _startTime;
        _timeText.text = "����: " + elapsedTime.ToString("F2") + "s";
    }
    private void FlipCellColor(int row, int column)
    {
        if (row >= 0 && row < _row && column >= 0 && column < _column)
        {
            var cell = _cells[row, column];
            if (cell.color == Color.white)
            {
                cell.color = Color.black;
            }
            else
            {
                cell.color = Color.white;
            }
        }
    }

    private Color GetRandomColor()
    {
        // �����_���ȐF��Ԃ�
        return Random.value < 0.5f ? Color.white : Color.black;
    }

    private bool CheckGameClear()
    {
        // �S�ẴZ�������ł��邩�ǂ����𔻒�
        foreach (var cell in _cells)
        {
            if (cell.color != Color.black)
            {
                return false;
            }
        }
        return true;
    }
}
