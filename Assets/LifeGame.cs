using UnityEngine;
using UnityEngine.UI;
using static CellController;

namespace Tutorial.LifeGame
{
    public class LifeGame : MonoBehaviour
    {
        [SerializeField]
        private int _rows = 10; // 行数

        [SerializeField]
        private int _columns = 20; // 列数

        [SerializeField]
        private CellController _cellPrefab = null; // セルのコピー元プレハブ

        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup = null;

        private CellController[,] _cells;

        private void Start()
        {
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = _columns;
            var parent = _gridLayoutGroup.gameObject.transform;

            _cells = new CellController[_rows, _columns];
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    var cell = Instantiate(_cellPrefab, parent);
                    cell.name = $"Cell({r}, {c})";
                    _cells[r, c] = cell;
                }
            }

            // 適当にランダムで生きているセルを設置
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    _cells[r, c].State = Random.value > 0.8
                        ? CellState.Alive : CellState.Dead;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Step();
            }
        }

        /// <summary>
        /// セルの状態を判定して次の世代に更新する。
        /// </summary>
        private void Step()
        {
            // 次の世代の状態を保存しておくための二次元配列
            var nextCells = new CellState[_rows, _columns];

            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    var cell = _cells[r, c];

                    // 周囲の生きているセル数
                    var ac = GetNeighborCount(r, c, CellState.Alive);

                    if (cell.State == CellState.Dead) // 死んでいるセル
                    {
                        // 周囲に生きたセルが3つあれば誕生
                        if (ac == 3) { nextCells[r, c] = CellState.Alive; }
                    }
                    else // 生きているセル
                    {
                        // 周囲に生きたセルが2～3つあれば生存、それ以外は死滅
                        if (ac == 2 || ac == 3) { nextCells[r, c] = CellState.Alive; }
                        else { nextCells[r, c] = CellState.Dead; }
                    }
                }
            }

            // すべての判定が終わったら次の世代に入れ替える
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    _cells[r, c].State = nextCells[r, c];
                }
            }
        }

        /// <summary>
        /// 指定の行番号・列番号の周囲の指定の状態のセル数を返す。
        /// </summary>
        /// <param name="row">行番号。</param>
        /// <param name="column">列番号。</param>
        /// <param name="state">セルの状態。</param>
        /// <returns>周囲の <paramref name="state"/> 状態のセル数を返す。</returns>
        private int GetNeighborCount(int row, int column, CellState state)
        {
            int count = 0;

            // 周囲のセルが指定の状態なら count をインクリメントする。
            { if (TryGetCell(row - 1, column - 1, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row - 1, column, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row - 1, column + 1, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row, column - 1, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row, column + 1, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row + 1, column - 1, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row + 1, column, out CellController x) && x.State == state) { count++; } }
            { if (TryGetCell(row + 1, column + 1, out CellController x) && x.State == state) { count++; } }

            return count;
        }

        /// <summary>
        /// 指定の行番号・列番号のセルを取得する。
        /// </summary>
        /// <param name="row">行番号。</param>
        /// <param name="column">列番号。</param>
        /// <param name="cell">セル。</param>
        /// <returns>セルを取得できれば true。そうでなければ false。</returns>
        private bool TryGetCell(int row, int column, out CellController cell)
        {
            if (row < 0 || column < 0 || row >= _cells.GetLength(0) || column >= _cells.GetLength(1))
            {
                cell = null;
                return false;
            }

            cell = _cells[row, column];
            return true;
        }
    }
}