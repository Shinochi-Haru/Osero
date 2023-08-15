using Reversi.Board;
using UnityEngine;
using UnityEngine.EventSystems;

public class OthelloGame : MonoBehaviour, IPointerClickHandler
{
    private StageManager stageManager; // StageManager�ւ̎Q��

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>(); // StageManager���������ĎQ�Ƃ��擾

        // StageManager�̏�����
        stageManager.InitializeBoard();

        // �Q�[���̏�����
        InitializeGame();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �N���b�N�����Z���̍��W���擾
        Vector3 clickPosition = eventData.pointerCurrentRaycast.worldPosition;

        // �Z���̍��W����s�Ɨ���v�Z
        int row = Mathf.FloorToInt(clickPosition.x);
        int col = Mathf.FloorToInt(clickPosition.z);

        // StageManager���g�p���Đ΂�z�u
        PlacePiece(row, col);
    }

    private void PlacePiece(int row, int col)
    {
        // �΂�z�u���鏈���������ɋL�q

        // stageManager.PlacePieceOnBoard(row, col, piecePrefab);
        // �K�v�Ȃ�΁A�Ֆʂ̏�Ԃ��X�V���鏈�����ǉ�
    }

    private void InitializeGame()
    {
        // �����z�u���Z�b�g�A�b�v
        stageManager.PlacePieceOnBoard(3, 3, stageManager.whitePiecePrefab);
        stageManager.PlacePieceOnBoard(4, 4, stageManager.whitePiecePrefab);
        stageManager.PlacePieceOnBoard(3, 4, stageManager.blackPiecePrefab);
        stageManager.PlacePieceOnBoard(4, 3, stageManager.blackPiecePrefab);
    }
}
