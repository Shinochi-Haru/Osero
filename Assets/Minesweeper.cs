using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int _rows = 1;

    [SerializeField]
    private int _columns = 1;

    [SerializeField]
    private int _mineCount = 1;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = null;

    [SerializeField]
    private Cell _cellPrefab = null;

    private Cell[,] _cells;

    /// <summary>
    /// �Q�[���N���A���Ă��邩�ǂ����B
    /// </summary>
    private bool IsSuccess => _openCount == (_cells.Length - _mineCount);

    private int _openCount = 0;

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _cells = new Cell[_rows, _columns];
        var parent = _gridLayoutGroup.gameObject.transform;
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cell.name = $"Cell({r}, {c})";
                _cells[r, c] = cell;
            }
        }
    }

    /// <summary>
    /// �w��̍s�ԍ��E��ԍ��̃Z�����擾����B
    /// </summary>
    /// <param name="row">�s�ԍ��B</param>
    /// <param name="column">��ԍ��B</param>
    /// <param name="cell">�Z���B</param>
    /// <returns>�Z�����擾�ł���� true�B�����łȂ���� false�B</returns>
    private bool TryGetCell(int row, int column, out Cell cell)
    {
        if (row < 0 || column < 0 || row >= _cells.GetLength(0) || column >= _cells.GetLength(1))
        {
            cell = null;
            return false;
        }

        cell = _cells[row, column];
        return true;
    }

    /// <summary>
    /// ���ׂẴZ����ΏۂɁA�w��̐������n���������_���ɐݒu����B
    /// </summary>
    /// <param name="mineCount">�n�����B</param>
    /// <param name="ignore">�n����z�u���Ȃ��Z���B</param>
    private void PlaceMines(int mineCount, Cell ignore)
    {
        if (mineCount > _cells.Length)
        {
            throw new System.ArgumentException(nameof(mineCount), "�n�������Z�������傫���ł��B");
        }

        // ���ׂẴZ���̏�Ԃ� None �ɏ���������
        foreach (var cell in _cells) { cell.CellState = CellState.None; }

        for (var i = 0; i < _mineCount;)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            if (ignore == _cells[r, c]) { continue; }
            if (TryPlaceMine(r, c)) { i++; }
        }
    }

    /// <summary>
    /// �w��̍s�ԍ��E��ԍ��ɒn����ݒu����B
    /// </summary>
    /// <param name="row">�s�ԍ��B</param>
    /// <param name="column">��ԍ��B</param>
    /// <returns>�n����ݒu�ł���� true�B�����łȂ���� false�B</returns>
    private bool TryPlaceMine(int row, int column)
    {
        // �Z�����擾�ł��Ȃ���Ύ��s
        if (!TryGetCell(row, column, out Cell cell)) { return false; }

        // �n���ݒu�ς݂Ȃ玸�s
        if (cell.CellState == CellState.Mine) { return false; }

        // �n����ݒu
        cell.CellState = CellState.Mine;

        // ���͂̃Z���i�n���ȊO�j�̒l���C���N�������g����
        { if (TryGetCell(row - 1, column - 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row - 1, column, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row - 1, column + 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row, column - 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row, column + 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row + 1, column - 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row + 1, column, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }
        { if (TryGetCell(row + 1, column + 1, out Cell x) && x.CellState != CellState.Mine) { x.CellState++; } }

        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var target = eventData.pointerCurrentRaycast.gameObject;

        // �Z�����N���b�N����
        if (target.TryGetComponent<Cell>(out var cell))
        {
            // �ŏ��̈��ڂ��n�����ǂ���
            if (_openCount == 0)
            {
                // �n�����Ĕz�u����
                PlaceMines(_mineCount, cell);
            }

            // �Z�����J��
            cell.Open();
            _openCount++;
            if (cell.CellState == CellState.Mine) // �J�����Z�����n��
            {
                Debug.Log("�Q�[���[�I�[�o�[");
            }
            else if (IsSuccess)
            {
                Debug.Log("�Q�[���[�N���A");
            }
        }
    }
}