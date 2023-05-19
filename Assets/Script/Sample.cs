using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    int[] imageArray = new int[5] { 1, 2, 3, 4, 5 };
    GameObject obj;
    Image image;
    private void Start()
    {
        for (var i = 0; i < 5; i++)
        {
            obj = new GameObject($"Cell{i}");
            obj.transform.parent = transform;

            image = obj.AddComponent<Image>();
            if (i == 0) { image.color = Color.red; }
            else { image.color = Color.white; }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左キーを押した
        {

        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 右キーを押したら右のCellが赤くなりすでに赤いのは白くなる
        {
            for (int i = 0; i < imageArray.Length; i++)
            {
                if (i == imageArray[i]) { image.color = Color.red; }
                else { image.color = Color.white; }
            }
        }
    }
}
