using UnityEngine;
using UnityEngine.UI;

namespace Reversi.Board
{
    /// <summary>
    /// �{�[�h�N���X
    /// </summary>
    public class StageManager : MonoBehaviour
    {

        [SerializeField] public GameObject blackPiecePrefab; // ���΂̃v���n�u

        [SerializeField] public GameObject whitePiecePrefab; // ���΂̃v���n�u

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

            _cells = new Grit[_rows, _columns];
            _cellPositions = new Vector3[_rows, _columns]; // _cellPositions ��������

            var parent = _gridLayoutGroup.gameObject.transform;
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    var cell = Instantiate(_cellPrefab);
                    cell.transform.SetParent(parent);
                    cell.name = $"Cell({r}, {c})";
                    _cells[r, c] = cell;

                    // �Z���̈ʒu���L�^
                    _cellPositions[r, c] = cell.transform.position;
                }
            }
        }
    

        // �����Ֆʂ��Z�b�g�A�b�v���郁�\�b�h
        public void InitializeBoard()
        {
            // �����̃Z���ɔz�u����Ă���I�u�W�F�N�g���폜�i�ȗ��j

            // �����z�u
            PlacePieceOnBoard(3, 3, whitePiecePrefab);
            PlacePieceOnBoard(4, 4, whitePiecePrefab);
            PlacePieceOnBoard(3, 4, blackPiecePrefab);
            PlacePieceOnBoard(4, 3, blackPiecePrefab);
        }

        // �{�[�h��ɐ΂�z�u���郁�\�b�h
        public void PlacePieceOnBoard(int row, int col, GameObject piecePrefab)
        {
            // �Z���̈ʒu���擾
            Vector3 cellPosition = _cellPositions[row, col];

            // �΂��Z���̈ʒu�ɔz�u
            Instantiate(piecePrefab, cellPosition, Quaternion.identity);
        }
    }
}