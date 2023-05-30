using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    [SerializeField] private int cellCount = 5; // セルの数をInspectorビューから設定

    GameObject[] cells; // セルの配列
    int currentCellIndex = 0; // 現在のセルのインデックス
    bool spaceKeyPressed = false; // スペースキーが押されたかどうかのフラグ

    private void Start()
    {
        cells = new GameObject[cellCount]; // セルの配列を設定された数に初期化

        for (var i = 0; i < cellCount; i++)
        {
            GameObject obj = new GameObject($"Cell{i}");
            obj.transform.parent = transform;

            Image image = obj.AddComponent<Image>();
            if (i == 0) { image.color = Color.red; }
            else { image.color = Color.white; }

            cells[i] = obj; // セルを配列に格納
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !spaceKeyPressed) // 左キーを押した（スペースキーが押されていない場合）
        {
            // 現在のセルの色を白にする
            cells[currentCellIndex].GetComponent<Image>().color = Color.white;

            // インデックスを更新し、ループさせる
            currentCellIndex--;
            if (currentCellIndex < 0)
                currentCellIndex = cells.Length - 1;

            // 更新されたセルの色を赤にする
            cells[currentCellIndex].GetComponent<Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !spaceKeyPressed) // 右キーを押した（スペースキーが押されていない場合）
        {
            // 現在のセルの色を白にする
            cells[currentCellIndex].GetComponent<Image>().color = Color.white;

            // インデックスを更新し、ループさせる
            currentCellIndex++;
            if (currentCellIndex >= cells.Length)
                currentCellIndex = 0;

            // 更新されたセルの色を赤にする
            cells[currentCellIndex].GetComponent<Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !spaceKeyPressed) // スペースキーを押した（まだスペースキーが押されていない場合）
        {
            spaceKeyPressed = true; // スペースキーが押されたフラグを立てる

            // 選択中のセルを削除
            Destroy(cells[currentCellIndex]);

            // インデックスを更新し、ループさせる
            currentCellIndex++;
            if (currentCellIndex >= cells.Length)
                currentCellIndex = 0;

            // すべてのセルを白にする
            foreach (GameObject cell in cells)
            {
                cell.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
