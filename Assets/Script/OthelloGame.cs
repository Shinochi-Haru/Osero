using Reversi.Board;
using UnityEngine;
using UnityEngine.EventSystems;

public class OthelloGame : MonoBehaviour, IPointerClickHandler
{
    private StageManager stageManager; // StageManagerへの参照

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>(); // StageManagerを検索して参照を取得

        // StageManagerの初期化
        stageManager.InitializeBoard();

        // ゲームの初期化
        InitializeGame();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックしたセルの座標を取得
        Vector3 clickPosition = eventData.pointerCurrentRaycast.worldPosition;

        // セルの座標から行と列を計算
        int row = Mathf.FloorToInt(clickPosition.x);
        int col = Mathf.FloorToInt(clickPosition.z);

        // StageManagerを使用して石を配置
        PlacePiece(row, col);
    }

    private void PlacePiece(int row, int col)
    {
        // 石を配置する処理をここに記述

        // stageManager.PlacePieceOnBoard(row, col, piecePrefab);
        // 必要ならば、盤面の状態を更新する処理も追加
    }

    private void InitializeGame()
    {
        // 初期配置をセットアップ
        stageManager.PlacePieceOnBoard(3, 3, stageManager.whitePiecePrefab);
        stageManager.PlacePieceOnBoard(4, 4, stageManager.whitePiecePrefab);
        stageManager.PlacePieceOnBoard(3, 4, stageManager.blackPiecePrefab);
        stageManager.PlacePieceOnBoard(4, 3, stageManager.blackPiecePrefab);
    }
}
