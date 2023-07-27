using UnityEngine;
using UnityEngine.UI;

namespace Reversi.Board
{
    /// <summary>
    /// �{�[�h�N���X
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
        /// �{�[�h�̃Z��Prefab
        /// </summary>
        //[SerializeField] private GameObject boardCellPrefab;

        /// <summary>
        /// �y��ƂȂ�I�u�W�F�N�g
        /// </summary>
        private GameObject _boardCellsBase;

        /// <summary>
        /// �Z���̈ʒu�z��
        /// </summary>
        private static Vector3[,] _cellPositions;

        /// <summary>
        /// ��ӂ�����̃Z����
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
        ///// �{�[�h��������
        ///// </summary>
        //public void GenerateBoard()
        //{
        //    _cellPositions = new Vector3[CellSideCount, CellSideCount];
        //    for (var x = 0; x < CellSideCount; x++)
        //    {
        //        for (var z = 0; z < CellSideCount; z++)
        //        {
        //            // �Z������
        //            var cell = Instantiate(boardCellPrefab, _boardCellsBase.gameObject.transform);
        //            cell.transform.localPosition = new Vector3(x, 0.4f, z);
        //            cell.transform.localScale = boardCellPrefab.transform.localScale;

        //            // �ʒu��ێ�
        //            _cellPositions[x, z] = cell.transform.localPosition;
        //        }
        //    }
        //}

        ///// <summary>
        ///// �w��Z���̈ʒu���擾
        ///// </summary>
        //public static Vector3 GetCellPosition(int x, int z)
        //{
        //    return _cellPositions[x, z];
        //}
    }
}