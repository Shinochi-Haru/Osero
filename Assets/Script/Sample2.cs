using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sample2 : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private int _row = 5;
    [SerializeField]private int _column = 5;
    [SerializeField]private Text _movesText; // 手数を表示するテキスト要素
    [SerializeField]private Text _timeText; // 時間を表示するテキスト要素
    private Image[,] _cells;
    private bool _gameInProgress;
    private int _moves;
    private float _startTime;

    private void Start()
    {
        // ゲーム開始時の手数と時間を初期化
        _moves = 0;
        _startTime = Time.time;
        _cells = new Image[_row, _column];

        // ゲームが進行中かを示すフラグを初期化
        _gameInProgress = false;

        // ゲーム開始時の手数と時間を初期化
        _moves = 0;
        _startTime = Time.time;

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
        UpdateMovesText();
        UpdateTimeText();
    }

    private void Update()
    {
        // ゲームが進行中であれば、クリア判定を行う
        if (_gameInProgress)
        {
            if (CheckGameClear())
            {
                _gameInProgress = false;
                float elapsedTime = Time.time - _startTime;
            }
            // 時間のテキストを更新
            UpdateTimeText();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cell = eventData.pointerCurrentRaycast.gameObject;
        var image = cell.GetComponent<Image>();

        if (!_gameInProgress)
        {
            _gameInProgress = true;
            _startTime = Time.time;
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

        // クリックしたセルとその周囲のセルの色を反転
        FlipCellColor(clickedRow, clickedColumn);
        FlipCellColor(clickedRow - 1, clickedColumn); // 上のセル
        FlipCellColor(clickedRow + 1, clickedColumn); // 下のセル
        FlipCellColor(clickedRow, clickedColumn - 1); // 左のセル
        FlipCellColor(clickedRow, clickedColumn + 1); // 右のセル


        // 手数をインクリメント
        _moves++;

        // 手数のテキストを更新
        UpdateMovesText();
    }

    private void UpdateMovesText()
    {
        _movesText.text = "手数: " + _moves.ToString();
    }

    private void UpdateTimeText()
    {
        float elapsedTime = Time.time - _startTime;
        _timeText.text = "時間: " + elapsedTime.ToString("F2") + "s";
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

    private bool CheckGameClear()
    {
        // 全てのセルが黒であるかどうかを判定
        foreach (var cell in _cells)
        {
            if (cell.color != Color.black)
            {
                return false;
            }
        }
        return true;
    }
}
