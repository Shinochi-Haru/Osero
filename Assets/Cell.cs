using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CellState
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,

    Mine = -1,
}

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Text _view = null; // 状態を表示するテキスト

    [SerializeField]
    private Image _cover = null; // 背面を隠す蓋

    [SerializeField]
    private CellState _cellState = CellState.None;
    public CellState CellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged();
        }
    }

    /// <summary>
    /// セルが開いているかどうか。
    /// </summary>
    public bool IsOpen => !_cover.enabled;

    /// <summary>
    /// 閉じているセルを開く。
    /// </summary>
    public void Open() => _cover.enabled = false;

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (_view == null) { return; }

        if (_cellState == CellState.None)
        {
            _view.text = "";
        }
        else if (_cellState == CellState.Mine)
        {
            _view.text = "X";
            _view.color = Color.red;
        }
        else
        {
            _view.text = ((int)_cellState).ToString();
            _view.color = Color.blue;
        }
    }
}