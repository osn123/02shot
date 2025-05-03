using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {
    public RectTransform titleImage; // �^�C�g���摜��RectTransform
    //public GameObject titleImage; // �^�C�g���摜��RectTransform
    public GameObject pressSpaceImage; // "Press Space"�̉摜�I�u�W�F�N�g
    public float moveSpeed = 200f; // �^�C�g���摜�̈ړ����x
    public float targetY = 0f; // �^�C�g���摜���~�܂�Y���W

    private bool isTitleInPosition = false;

    void Start() {
        // ������Ԃł�"Press Space"�̉摜���\��
        pressSpaceImage.SetActive(false);
    }

    void Update() {
        if (!isTitleInPosition) {
            // �^�C�g���摜�𒆉��Ɉړ�
            Vector3 currentPosition = titleImage.anchoredPosition;
            if (currentPosition.y > targetY) {
                currentPosition.y -= moveSpeed * Time.deltaTime;
                titleImage.anchoredPosition = currentPosition;
            } else {
                // �ڕW�ʒu�ɓ��B������ʒu���Œ肵�A"Press Space"��\��
                currentPosition.y = targetY;
                titleImage.anchoredPosition = currentPosition;
                isTitleInPosition = true;
                pressSpaceImage.SetActive(true);
            }
        } else {
            // �X�y�[�X�L�[�������ꂽ��Q�[���V�[���ɑJ��
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadGameScene();
            }
        }
    }

    void LoadGameScene() {
        // �Q�[���V�[���ɑJ�� (�V�[������"GameScene"�ɕύX���Ă�������)
        SceneManager.LoadScene("GameScene");
    }
}
