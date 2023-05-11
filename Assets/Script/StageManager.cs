using UnityEngine;

namespace Reversi.Board
{
    /// <summary>
    /// �{�[�h�N���X
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        /// <summary>
        /// �{�[�h�̃Z��Prefab
        /// </summary>
        [SerializeField] private GameObject boardCellPrefab;

        /// <summary>
        /// �y��ƂȂ�I�u�W�F�N�g
        /// </summary>
        private GameObject _boardCellsBase;

        /// <summary>
        /// �Z���̈ʒu�z��
        /// </summary>
        private Vector3[,] _cellPositions;

        /// <summary>
        /// ��ӂ�����̃Z����
        /// </summary>
        public static int CellSideCount => 8;

        private void Awake()
        {
            // �y��ƂȂ�I�u�W�F�N�g�𐶐�
            _boardCellsBase = new GameObject("BoardCells");
            _boardCellsBase.transform.SetParent(transform);
            _boardCellsBase.transform.position = transform.position;
            _boardCellsBase.transform.localScale = Vector3.one;

            // �{�[�h����
            GenerateBoard();
        }

        /// <summary>
        /// �{�[�h��������
        /// </summary>
        private void GenerateBoard()
        {
            _cellPositions = new Vector3[CellSideCount, CellSideCount];
            for (var x = 0; x < CellSideCount; x++)
            {
                for (var z = 0; z < CellSideCount; z++)
                {
                    // �Z������
                    var cell = Instantiate(boardCellPrefab, _boardCellsBase.gameObject.transform);
                    cell.transform.localPosition = new Vector3(x, 0.4f, z);
                    cell.transform.localScale = boardCellPrefab.transform.localScale;

                    // �ʒu��ێ�
                    _cellPositions[x, z] = cell.transform.localPosition;
                }
            }
        }

        /// <summary>
        /// �w��Z���̈ʒu���擾
        /// </summary>
        public Vector3 GetCellPosition(int x, int z)
        {
            return _cellPositions[x, z];
        }
    }
}