using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    /// <summary>
    /// セルの状態。
    /// </summary>
    public enum CellState
    {
        /// <summary>
        /// 死亡。
        /// </summary>
        Dead,

        /// <summary>
        /// 生存。
        /// </summary>
        Alive,
    }
    [SerializeField]
    private CellState _state = CellState.Dead; // セルの状態

    public CellState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChanged();
        }
    }

    [SerializeField]
    private Image _image = null; // セル画像

    [SerializeField]
    private Color AliveColor = Color.black; // 生きているセルの色

    [SerializeField]
    private Color DeadColor = Color.white; // 死んでいるセルの色

    private void OnValidate()
    {
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        if (_image)
        {
            _image.color = (_state == CellState.Alive) ? AliveColor : DeadColor;
        }
    }
}
