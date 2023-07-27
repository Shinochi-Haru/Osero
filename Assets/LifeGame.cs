using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LifeGame : MonoBehaviour, IPointerClickHandler
{
    private enum CellStatus
    {
        Dead = 0, // 死亡
        Arive = 1, // 生存
    }
    [SerializeField] private int _row = 5;
    [SerializeField] private int _column = 5;
    private Image[,] _cells;
    private bool _gameInProgress;

    // 世代更新の間隔（秒）を設定（例：1秒ごとに更新）
    [SerializeField] private float _generationInterval = 1f;
    private float _timeSinceLastGeneration;

    private void Start()
    {
        // ゲームが進行中かを示すフラグを初期化
        _gameInProgress = false;

        // _cells配列を指定された行数と列数で初期化する
        _cells = new Image[_row, _column];

        // セルを作成しランダムな色で初期化
        for (var r = 0; r < _row; r++)
        {
            for (var c = 0; c < _column; c++)
            {
                var cell = new GameObject($"Cell({r}, {c})");
                cell.transform.parent = transform;
                var image = cell.AddComponent<Image>();

                // セルの色をランダムに初期化
                image.color = GetRandomColor();

                _cells[r, c] = image;
            }
        }
    }

    private void Update()
    {
        if (_gameInProgress)
        {
            // 時間が経過したら新しい世代を計算
            _timeSinceLastGeneration += Time.deltaTime;
            if (_timeSinceLastGeneration >= _generationInterval)
            {
                CalculateNextGeneration();
                _timeSinceLastGeneration = 0f;
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject;
        var image = cell.GetComponent<Image>();

        if (!_gameInProgress)
        {
            _gameInProgress = true;
        }

        // クリックしたセルの座標を取得
        int clickedRow = -1;
        int clickedColumn = -1;

        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                if (_cells[r, c] == image)
                {
                    clickedRow = r;
                    clickedColumn = c;
                    break;
                }
            }
        }
        FlipCellColor(clickedRow, clickedColumn);
    }
    private void FlipCellColor(int row, int column)
    {
        if (row >= 0 && row < _row && column >= 0 && column < _column)
        {
            var cell = _cells[row, column];
            if (cell.color == Color.white)
            {
                cell.color = Color.black;
            }
            else
            {
                cell.color = Color.white;
            }
        }
    }

    private Color GetRandomColor()
    {
        // ランダムな色を返す
        return Random.value < 0.5f ? Color.white : Color.black;
    }

    private void CalculateNextGeneration()
    {
        var nextGeneration = new CellStatus[_row, _column];

        // 新しい世代のセルの状態を計算
        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                var aliveNeighbors = CountAliveNeighbors(r, c);
                if (_cells[r, c].color == Color.black)
                {
                    // 死んでいるセルの処理
                    if (aliveNeighbors == 3)
                    {
                        nextGeneration[r, c] = CellStatus.Arive;
                    }
                    else if(aliveNeighbors >= 1)
                    {
                        nextGeneration[r, c] = CellStatus.Dead;
                    }
                }
                else
                {
                    // 生きているセルの処理
                    if (aliveNeighbors == 2 || aliveNeighbors == 3)
                    {
                        nextGeneration[r, c] = CellStatus.Arive;
                    }
                    else if(aliveNeighbors <= 4)
                    {
                        nextGeneration[r, c] = CellStatus.Dead;
                    }
                }
            }
        }

        // 新しい世代を反映
        for (int r = 0; r < _row; r++)
        {
            for (int c = 0; c < _column; c++)
            {
                if (nextGeneration[r, c] == CellStatus.Arive)
                {
                    _cells[r, c].color = Color.black;
                }
                else
                {
                    _cells[r, c].color = Color.white;
                }
            }
        }
    }

    private int CountAliveNeighbors(int row, int column)
    {
        int count = 0;
        for (int r = row - 1; r <= row + 1; r++)
        {
            for (int c = column - 1; c <= column + 1; c++)
            {
                if (r >= 0 && r < _row && c >= 0 && c < _column && !(r == row && c == column))
                {
                    if (_cells[r, c].color == Color.black)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    } 
}
