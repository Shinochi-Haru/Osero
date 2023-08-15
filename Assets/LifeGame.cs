using UnityEngine;
using UnityEngine.UI;
using static CellController;

namespace Tutorial.LifeGame
{
    public class LifeGame : MonoBehaviour
    {
        [SerializeField]
        private int _rows = 10; // �s��

        [SerializeField]
        private int _columns = 20; // ��

        [SerializeField]
        private CellController _cellPrefab = null; // �Z���̃R�s�[���v���n�u

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

            // �K���Ƀ����_���Ő����Ă���Z����ݒu
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
        /// �Z���̏�Ԃ𔻒肵�Ď��̐���ɍX�V����B
        /// </summary>
        private void Step()
        {
            // ���̐���̏�Ԃ�ۑ����Ă������߂̓񎟌��z��
            var nextCells = new CellState[_rows, _columns];

            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    var cell = _cells[r, c];

                    // ���͂̐����Ă���Z����
                    var ac = GetNeighborCount(r, c, CellState.Alive);

                    if (cell.State == CellState.Dead) // ����ł���Z��
                    {
                        // ���͂ɐ������Z����3����Βa��
                        if (ac == 3) { nextCells[r, c] = CellState.Alive; }
                    }
                    else // �����Ă���Z��
                    {
                        // ���͂ɐ������Z����2�`3����ΐ����A����ȊO�͎���
                        if (ac == 2 || ac == 3) { nextCells[r, c] = CellState.Alive; }
                        else { nextCells[r, c] = CellState.Dead; }
                    }
                }
            }

            // ���ׂĂ̔��肪�I������玟�̐���ɓ���ւ���
            for (var r = 0; r < _rows; r++)
            {
                for (var c = 0; c < _columns; c++)
                {
                    _cells[r, c].State = nextCells[r, c];
                }
            }
        }

        /// <summary>
        /// �w��̍s�ԍ��E��ԍ��̎��͂̎w��̏�Ԃ̃Z������Ԃ��B
        /// </summary>
        /// <param name="row">�s�ԍ��B</param>
        /// <param name="column">��ԍ��B</param>
        /// <param name="state">�Z���̏�ԁB</param>
        /// <returns>���͂� <paramref name="state"/> ��Ԃ̃Z������Ԃ��B</returns>
        private int GetNeighborCount(int row, int column, CellState state)
        {
            int count = 0;

            // ���͂̃Z�����w��̏�ԂȂ� count ���C���N�������g����B
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
        /// �w��̍s�ԍ��E��ԍ��̃Z�����擾����B
        /// </summary>
        /// <param name="row">�s�ԍ��B</param>
        /// <param name="column">��ԍ��B</param>
        /// <param name="cell">�Z���B</param>
        /// <returns>�Z�����擾�ł���� true�B�����łȂ���� false�B</returns>
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