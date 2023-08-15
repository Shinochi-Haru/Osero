using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    /// <summary>
    /// �Z���̏�ԁB
    /// </summary>
    public enum CellState
    {
        /// <summary>
        /// ���S�B
        /// </summary>
        Dead,

        /// <summary>
        /// �����B
        /// </summary>
        Alive,
    }
    [SerializeField]
    private CellState _state = CellState.Dead; // �Z���̏��

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
    private Image _image = null; // �Z���摜

    [SerializeField]
    private Color AliveColor = Color.black; // �����Ă���Z���̐F

    [SerializeField]
    private Color DeadColor = Color.white; // ����ł���Z���̐F

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
