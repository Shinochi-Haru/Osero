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
    /// ゲームクリアしているかどうか。
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

        PlaceMines(_mineCount);
    }

    /// <summary>
    /// 指定の行番号・列番号のセルを取得する。
    /// </summary>
    /// <param name="row">行番号。</param>
    /// <param name="column">列番号。</param>
    /// <param name="cell">セル。</param>
    /// <returns>セルを取得できれば true。そうでなければ false。</returns>
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
    /// すべてのセルを対象に、指定の数だけ地雷をランダムに設置する。
    /// </summary>
    /// <param name="mineCount">地雷数。</param>
    /// <param name="ignore">地雷を配置しないセル。</param>
    private void PlaceMines(int mineCount, Cell ignore = null)
    {
        if (mineCount > _cells.Length)
        {
            throw new System.ArgumentException(nameof(mineCount), "地雷数がセル数より大きいです。");
        }

        // すべてのセルの状態を None に初期化する
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
    /// 指定の行番号・列番号に地雷を設置する。
    /// </summary>
    /// <param name="row">行番号。</param>
    /// <param name="column">列番号。</param>
    /// <returns>地雷を設置できれば true。そうでなければ false。</returns>
    private bool TryPlaceMine(int row, int column)
    {
        // セルを取得できなければ失敗
        if (!TryGetCell(row, column, out Cell cell)) { return false; }

        // 地雷設置済みなら失敗
        if (cell.CellState == CellState.Mine) { return false; }

        // 地雷を設置
        cell.CellState = CellState.Mine;

        // 周囲のセル（地雷以外）の値をインクリメントする
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

        // セルをクリックした
        if (target.TryGetComponent<Cell>(out var cell))
        {
            // 最初の一手目が地雷かどうか
            if (_openCount == 0 && cell.CellState == CellState.Mine)
            {
                // 地雷を再配置する
                PlaceMines(_mineCount, cell);
            }

            // セルを開く
            if (TryOpen(cell))
            {
                if (cell.CellState == CellState.Mine) // 開いたセルが地雷
                {
                    Debug.Log("ゲームーオーバー");
                }
                else if (IsSuccess)
                {
                    Debug.Log("ゲームークリア");
                }
            }
        }
    }

    /// <summary>
    /// 指定したセルを開く。
    /// </summary>
    /// <param name="cell">開くセル。</param>
    /// <returns>セルを開けたら true。そうでなければ false。</returns>
    private bool TryOpen(Cell cell)
        => TryGetCellPosition(cell, out var r, out var c)
            && TryOpen(r, c);

    /// <summary>
    /// 指定の行番号・列番号のセルを開く。
    /// </summary>
    /// <param name="row">行番号。</param>
    /// <param name="column">列番号。</param>
    /// <returns>セルを開くことができれば true。そうでなければ false。</returns>
    private bool TryOpen(int row, int column)
    {
        // 無効な行番号・列番号なら失敗
        if (!TryGetCell(row, column, out var cell)) { return false; }

        // 既に開いている場合は失敗。
        if (cell.IsOpen) { return false; }

        cell.Open(); // セル自身を開く
        _openCount++;

        if (cell.CellState == CellState.None) // 開いたセルが空白だった
        {
            // 周囲のセルを展開する
            TryOpen(row - 1, column - 1); // 左上
            TryOpen(row - 1, column); // 上
            TryOpen(row - 1, column + 1); // 右上
            TryOpen(row, column - 1); // 左
            TryOpen(row, column + 1); // 右
            TryOpen(row + 1, column - 1); // 左下
            TryOpen(row + 1, column); // 下
            TryOpen(row + 1, column + 1); // 右下
        }

        return true;
    }

    /// <summary>
    /// 指定したセルの行番号と列番号を返す。
    /// </summary>
    /// <param name="cell">調べるセル。</param>
    /// <param name="row">行番号。</param>
    /// <param name="column">列番号。</param>
    /// <returns>成功すれば true。失敗すれば false。</returns>
    private bool TryGetCellPosition(Cell cell, out int row, out int column)
    {
        for (var r = 0; r < _cells.GetLength(0); r++)
        {
            for (var c = 0; c < _cells.GetLength(1); c++)
            {
                if (cell == _cells[r, c])
                {
                    row = r;
                    column = c;
                    return true;
                }
            }
        }

        row = 0; column = 0;
        return false;
    }
}