using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MineAns : MonoBehaviour
{
    public int row;
    public int column;
    public bool isAlive = false;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        UpdateCellColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // �Z�����N���b�N���ꂽ�Ƃ��̏������L�q���܂�
        isAlive = !isAlive;
        UpdateCellColor();
    }

    public void UpdateCellColor()
    {
        image.color = isAlive ? Color.black : Color.white;
    }
}
