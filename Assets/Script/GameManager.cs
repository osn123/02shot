using UnityEngine;

// �Q�[���S�̂̊Ǘ����s���N���X
public class GameManager : MonoBehaviour {
    public GameObject background1; // 1���ڂ̔w�i�摜�I�u�W�F�N�g
    public GameObject background2; // 2���ڂ̔w�i�摜�I�u�W�F�N�g
    [SerializeField] private float scrollSpeed = 2f; // �w�i�摜�̃X�N���[�����x�iInspector�Œ����\�j
    private float backgroundHeight; // �w�i�摜�̍������i�[����ϐ�

    void Start() {
        // �w�i�摜�̍������擾
        backgroundHeight = background1.GetComponentInChildren<SpriteRenderer>().bounds.size.y * 4; // 1���ڂ̔w�i�̍������擾���A4�{����
    }

    void Update() {
        // ���t���[���A�w�i�摜���X�N���[��������
        ScrollBackground(background1); // 1���ڂ̔w�i���X�N���[��
        ScrollBackground(background2); // 2���ڂ̔w�i���X�N���[��

        // �w�i�摜����ʊO�ɏo���ꍇ�A�ʒu�����Z�b�g����
        ResetPositionIfNeeded(background1,background2); // 1���ڂ̔w�i�����Z�b�g
        ResetPositionIfNeeded(background2,background1); // 2���ڂ̔w�i�����Z�b�g
    }

    void ScrollBackground(GameObject background) {
        // ���݂̈ʒu���擾
        Vector3 position = background.transform.position;

        // �������Ɉړ��i�X�N���[�����x�ɉ����Ĉړ��ʂ��v�Z�j
        position.y -= scrollSpeed * Time.deltaTime; // �X�N���[�����x�Ɋ�Â���Y���W������������

        // �v�Z�����ʒu��w�i�I�u�W�F�N�g�ɓK�p
        background.transform.position = position; // �V�����ʒu��w�i�I�u�W�F�N�g�ɐݒ�
    }

    void ResetPositionIfNeeded(GameObject currentBackground,GameObject otherBackground) {
        // ���݂̔w�i����ʊO�i-backgroundHeight�ȉ��j�ɏo���ꍇ
        if (currentBackground.transform.position.y <= -backgroundHeight) {
            // ��������̔w�i�̏�Ɍ��݂̔w�i���ړ�������
            currentBackground.transform.position = new Vector3(
                currentBackground.transform.position.x, // X���W�͂��̂܂�
                otherBackground.transform.position.y + backgroundHeight, // Y���W����������̔w�i�̏�ɐݒ�
                currentBackground.transform.position.z // Z���W�͂��̂܂�
            );
        }
    }
}
