using UnityEngine;
using UnityEngine.UI;

namespace Reversi.Board
{
    /// <summary>
    /// ボードクラス
    /// </summary>
    public class StageManager : MonoBehaviour
    {

        [SerializeField] public GameObject blackPiecePrefab; // 黒石のプレハブ

        [SerializeField] public GameObject whitePiecePrefab; // 白石のプレハブ

        [SerializeField]
        private int _rows = 1;

        [SerializeField]
        private int _columns = 1;

        [SerializeField]
        private int _mineCount = 1;

        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup = null;

        [SerializeField]
        private Grit _cellPrefab = null;

        private Grit[,] _cells;
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

            _cells = new Grit[_rows, _columns];
            _cellPositions = new Vector3[_rows, _columns]; // _cellPositions を初期化

            var parent = _gridLayoutGroup.gameObject.transform;
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    var cell = Instantiate(_cellPrefab);
                    cell.transform.SetParent(parent);
                    cell.name = $"Cell({r}, {c})";
                    _cells[r, c] = cell;

                    // セルの位置を記録
                    _cellPositions[r, c] = cell.transform.position;
                }
            }
        }
    

        // 初期盤面をセットアップするメソッド
        public void InitializeBoard()
        {
            // 既存のセルに配置されているオブジェクトを削除（省略）

            // 初期配置
            PlacePieceOnBoard(3, 3, whitePiecePrefab);
            PlacePieceOnBoard(4, 4, whitePiecePrefab);
            PlacePieceOnBoard(3, 4, blackPiecePrefab);
            PlacePieceOnBoard(4, 3, blackPiecePrefab);
        }

        // ボード上に石を配置するメソッド
        public void PlacePieceOnBoard(int row, int col, GameObject piecePrefab)
        {
            // セルの位置を取得
            Vector3 cellPosition = _cellPositions[row, col];

            // 石をセルの位置に配置
            Instantiate(piecePrefab, cellPosition, Quaternion.identity);
        }
    }
}