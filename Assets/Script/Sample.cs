using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
    [SerializeField] private int cellCount = 5; // �Z���̐���Inspector�r���[����ݒ�

    GameObject[] cells; // �Z���̔z��
    int currentCellIndex = 0; // ���݂̃Z���̃C���f�b�N�X
    bool spaceKeyPressed = false; // �X�y�[�X�L�[�������ꂽ���ǂ����̃t���O

    private void Start()
    {
        cells = new GameObject[cellCount]; // �Z���̔z���ݒ肳�ꂽ���ɏ�����

        for (var i = 0; i < cellCount; i++)
        {
            GameObject obj = new GameObject($"Cell{i}");
            obj.transform.parent = transform;

            Image image = obj.AddComponent<Image>();
            if (i == 0) { image.color = Color.red; }
            else { image.color = Color.white; }

            cells[i] = obj; // �Z����z��Ɋi�[
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !spaceKeyPressed) // ���L�[���������i�X�y�[�X�L�[��������Ă��Ȃ��ꍇ�j
        {
            // ���݂̃Z���̐F�𔒂ɂ���
            cells[currentCellIndex].GetComponent<Image>().color = Color.white;

            // �C���f�b�N�X���X�V���A���[�v������
            currentCellIndex--;
            if (currentCellIndex < 0)
                currentCellIndex = cells.Length - 1;

            // �X�V���ꂽ�Z���̐F��Ԃɂ���
            cells[currentCellIndex].GetComponent<Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !spaceKeyPressed) // �E�L�[���������i�X�y�[�X�L�[��������Ă��Ȃ��ꍇ�j
        {
            // ���݂̃Z���̐F�𔒂ɂ���
            cells[currentCellIndex].GetComponent<Image>().color = Color.white;

            // �C���f�b�N�X���X�V���A���[�v������
            currentCellIndex++;
            if (currentCellIndex >= cells.Length)
                currentCellIndex = 0;

            // �X�V���ꂽ�Z���̐F��Ԃɂ���
            cells[currentCellIndex].GetComponent<Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !spaceKeyPressed) // �X�y�[�X�L�[���������i�܂��X�y�[�X�L�[��������Ă��Ȃ��ꍇ�j
        {
            spaceKeyPressed = true; // �X�y�[�X�L�[�������ꂽ�t���O�𗧂Ă�

            // �I�𒆂̃Z�����폜
            Destroy(cells[currentCellIndex]);

            // �C���f�b�N�X���X�V���A���[�v������
            currentCellIndex++;
            if (currentCellIndex >= cells.Length)
                currentCellIndex = 0;

            // ���ׂẴZ���𔒂ɂ���
            foreach (GameObject cell in cells)
            {
                cell.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
