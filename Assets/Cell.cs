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

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Text _view = null;

    private bool _isClosed = true;
    private bool _isMine = false;

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

    public Vector2Int GridPosition { get; internal set; }

    private void Start()
    {
        Close();
    }

    public void Close()
    {
        _isClosed = true;
        _view.text = "";
        _view.color = Color.black;
    }

    public void Open()
    {
        _isClosed = false;

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

    public void OnCellStateChanged()
    {
        if (_isClosed)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isClosed)
        {
            Open();

            if (_cellState == CellState.Mine)
            {
                // ゲームオーバー処理
                Debug.Log("Game Over!");
            }
        }
    }

    public void SetNumber(int number)
    {
        _cellState = (CellState)number;

        if (_isClosed)
        {
            _view.text = "";
        }
        else
        {
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
    }
}
