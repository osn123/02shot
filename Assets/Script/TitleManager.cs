using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour { // �^�C�g����ʂ̃R���g���[���[�N���X
    public GameObject titleImage; // �^�C�g���摜��GameObject
    public GameObject pressSpaceImage; // "Press Space"�̉摜��GameObject
    [SerializeField] private float moveSpeed= 2f;
    [SerializeField] private float targetY = 0f; // �^�C�g���摜���~�܂�Y���W

    private bool isTitleInPosition = false; // �^�C�g�����ʒu�ɓ��B�������ǂ����̃t���O

    void Start() { // �X�^�[�g���\�b�h
        pressSpaceImage.SetActive(false); // ������Ԃł�"Press Space"�̉摜���\��
    }

    void Update() { // �X�V���\�b�h
        if (!isTitleInPosition) { // �^�C�g�����ʒu�ɓ��B���Ă��Ȃ��ꍇ
            Vector3 currentPosition = titleImage.transform.position; // ���݂̃^�C�g���摜�̈ʒu���擾
            if (currentPosition.y > targetY) { // ���݂�Y���W���ڕWY���W���傫���ꍇ
                currentPosition.y -= moveSpeed * Time.deltaTime; // Y���W���ړ����x�Ɋ�Â��Č���
                titleImage.transform.position = currentPosition; // �^�C�g���摜�̈ʒu���X�V
            } else { // �ڕW�ʒu�ɓ��B�����ꍇ
                currentPosition.y = targetY; // Y���W��ڕW�ʒu�ɐݒ�
                titleImage.transform.position = currentPosition; // �^�C�g���摜�̈ʒu���X�V
                isTitleInPosition = true; // �^�C�g�����ʒu�ɓ��B�����t���O��ݒ�
                StartCoroutine(BlinkPressSpaceImage()); // "Press Space"�̉摜��_�ł�����R���[�`�����J�n
            }
        } else { // �^�C�g�����ʒu�ɓ��B�����ꍇ
            if (Input.GetKeyDown(KeyCode.Space)) { // �X�y�[�X�L�[�������ꂽ�ꍇ
                LoadGameScene(); // �Q�[���V�[����ǂݍ���
            }
        }
    }

    void LoadGameScene() { // �Q�[���V�[����ǂݍ��ރ��\�b�h
        SceneManager.LoadScene("GameScene"); // �Q�[���V�[���ɑJ��
    }

    IEnumerator BlinkPressSpaceImage() { // "Press Space"�̉摜��_�ł�����R���[�`��
        while (true) { // �������[�v
            pressSpaceImage.SetActive(true); // �摜��\��
            yield return new WaitForSeconds(0.5f); // 0.5�b�ҋ@
            pressSpaceImage.SetActive(false); // �摜���\��
            yield return new WaitForSeconds(0.5f); // 0.5�b�ҋ@
        }
    }
}
