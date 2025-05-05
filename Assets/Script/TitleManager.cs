using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {
    [Header("�^�C�g���摜")]
    public GameObject titleImage; // �^�C�g���摜��GameObject

    [Header("\"Press Space\"�摜")]
    public GameObject pressSpaceImage; // "Press Space"�̉摜��GameObject

    [Header("�ݒ�l")]
    [SerializeField] private float moveSpeed = 2f;         // �^�C�g���摜�̈ړ����x
    [SerializeField] private float targetY = 0f;           // �^�C�g���摜���~�܂�Y���W
    [SerializeField] private float blinkInterval = 0.5f;   // ���ł̊Ԋu

    private AudioSource audioSource;  // AudioSource
    public AudioClip StartClip;   // �G���jSE



    // �����ϐ�
    private float blinkTimer = 0f;             // ���ł̃^�C�}�[
    private bool isPressSpaceVisible = true;   // "Press Space"�̕\�����
    // �X�e�[�g�̒�`
    private enum TitleState {
        MovingTitle, // �^�C�g���摜���ړ���
        WaitingForInput, // ���͑҂�
    }

    private TitleState currentState = TitleState.MovingTitle; // ���݂̃X�e�[�g

    void Start() {
        audioSource = GetComponent<AudioSource>(); // AudioSource�R���|�[�l���g���擾
        pressSpaceImage.SetActive(false); // ������Ԃł�"Press Space"�̉摜���\��
        Player.isDead = false; // �v���C���[�̎��S��Ԃ����Z�b�g
    }

    void Update() {
        switch (currentState) {
            case TitleState.MovingTitle:
                HandleTitleMovement();
                break;
            case TitleState.WaitingForInput:
                HandleBlinking();
                HandleInput();
                break;
            default:
                break;
        }
    }

    private void HandleTitleMovement() {
        Vector3 currentPosition = titleImage.transform.position; // ���݂̃^�C�g���摜�̈ʒu���擾
        if (currentPosition.y > targetY) { // ���݂�Y���W���ڕWY���W���傫���ꍇ
            currentPosition.y -= moveSpeed * Time.deltaTime; // Y���W���ړ����x�Ɋ�Â��Č���
            titleImage.transform.position = currentPosition; // �^�C�g���摜�̈ʒu���X�V
        } else { // �ڕW�ʒu�ɓ��B�����ꍇ
            currentPosition.y = targetY; // Y���W��ڕW�ʒu�ɐݒ�
            titleImage.transform.position = currentPosition; // �^�C�g���摜�̈ʒu���X�V
            pressSpaceImage.SetActive(true); // "Press Space"�̉摜��\��
            currentState = TitleState.WaitingForInput; // �X�e�[�g����͑҂��ɕύX
        }
    }

    private void HandleBlinking() {
        blinkTimer += Time.deltaTime; // �^�C�}�[���X�V
        if (blinkTimer >= blinkInterval) { // ���ŊԊu�𒴂����ꍇ
            isPressSpaceVisible = !isPressSpaceVisible; // �\����Ԃ�؂�ւ�
            pressSpaceImage.SetActive(isPressSpaceVisible); // �\��/��\����؂�ւ�
            blinkTimer = 0f; // �^�C�}�[�����Z�b�g
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Space)) { // �X�y�[�X�L�[�������ꂽ�ꍇ
            StartCoroutine(LoadGameScene()); // �Q�[���V�[����ǂݍ���
        }
    }

    private IEnumerator LoadGameScene() {
        audioSource.PlayOneShot(StartClip); // �G���jSE���Đ�
        yield return new WaitForSeconds(StartClip.length); // SE�̍Đ����I���܂őҋ@
        SceneManager.LoadScene("GameScene"); // �Q�[���V�[���ɑJ��
    }
}
