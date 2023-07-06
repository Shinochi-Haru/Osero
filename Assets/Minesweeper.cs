using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Minesweeper : MonoBehaviour
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

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        var cells = new Cell[_rows, _columns];
        var parent = _gridLayoutGroup.transform;
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.transform.SetParent(parent);
                cells[r, c] = cell;
            }
        }

        var minePositions = new HashSet<Vector2Int>();

        //�n���̔z�u
        for (var i = 0; i < _mineCount; i++)
        {
            var r = Random.Range(0, _rows);
            var c = Random.Range(0, _columns);
            var position = new Vector2Int(r, c);

            if (minePositions.Contains(position))
            {
                i--;
                continue;
            }

            var cell = cells[r, c];
            cell.CellState = CellState.Mine;
            minePositions.Add(position);
        }

        // ���͂̒n������ݒ肷��
        for (var r = 0; r < _rows; r++)
        {
            for (var c = 0; c < _columns; c++)
            {
                var cell = cells[r, c];
                if (cell.CellState != CellState.Mine)
                {
                    var surroundingMineCount = GetSurroundingMineCount(cells, r, c);
                    cell.SetNumber(surroundingMineCount);
                }
            }
        }
    }
    //�ڂ��Ă���n���̃J�E���g����
    private int GetSurroundingMineCount(Cell[,] cells, int row, int column)
    {
        var count = 0;
        var rows = cells.GetLength(0);
        var columns = cells.GetLength(1);

        for (var r = Mathf.Max(0, row - 1); r <= Mathf.Min(row + 1, rows - 1); r++)
        {
            for (var c = Mathf.Max(0, column - 1); c <= Mathf.Min(column + 1, columns - 1); c++)
            {
                if (r == row && c == column) continue;

                var cell = cells[r, c];
                if (cell.CellState == CellState.Mine)
                {
                    count++;
                }
            }
        }
        return count;
    }
}