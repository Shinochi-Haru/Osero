using UnityEngine;
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
    private Text _view = null;

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
    //ステートの遷移
    public void OnCellStateChanged()
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
        else if (_cellState >= CellState.One && _cellState <= CellState.Eight)
        {
            _view.text = ((int)_cellState).ToString();
            _view.color = Color.blue;
        }
    }
    //地雷の数・地雷の表示
    public void SetNumber(int number)
    {
        if (number == 0)
        {
            _cellState = CellState.None;
            _view.text = "";
        }
        else if (number == -1)
        {
            _cellState = CellState.Mine;
            _view.text = "X";
            _view.color = Color.red;
        }
        else if (number >= 1 && number <= 8)
        {
            _cellState = (CellState)number;
            _view.text = number.ToString();
            _view.color = Color.blue;
        }
    }
}
