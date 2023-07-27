using UnityEngine;
using UnityEngine.UI;

namespace Reversi.Board
{
    /// <summary>
    /// ボードクラス
    /// </summary>
    public class StageManager : MonoBehaviour
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
        /// ボードのセルPrefab
        /// </summary>
        //[SerializeField] private GameObject boardCellPrefab;

        /// <summary>
        /// 土台となるオブジェクト
        /// </summary>
        private GameObject _boardCellsBase;

        /// <summary>
        /// セルの位置配列
        /// </summary>
        private static Vector3[,] _cellPositions;

        /// <summary>
        /// 一辺あたりのセル数
        /// </summary>
        public static int CellSideCount => 8;

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

        ///// <summary>
        ///// ボード生成処理
        ///// </summary>
        //public void GenerateBoard()
        //{
        //    _cellPositions = new Vector3[CellSideCount, CellSideCount];
        //    for (var x = 0; x < CellSideCount; x++)
        //    {
        //        for (var z = 0; z < CellSideCount; z++)
        //        {
        //            // セル生成
        //            var cell = Instantiate(boardCellPrefab, _boardCellsBase.gameObject.transform);
        //            cell.transform.localPosition = new Vector3(x, 0.4f, z);
        //            cell.transform.localScale = boardCellPrefab.transform.localScale;

        //            // 位置を保持
        //            _cellPositions[x, z] = cell.transform.localPosition;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 指定セルの位置を取得
        ///// </summary>
        //public static Vector3 GetCellPosition(int x, int z)
        //{
        //    return _cellPositions[x, z];
        //}
    }
}