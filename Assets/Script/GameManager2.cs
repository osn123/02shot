using UnityEngine;

public class GameManager2 : MonoBehaviour {
    public SpriteRenderer background1; // 1���ڂ̔w�i
    public SpriteRenderer background2; // 2���ڂ̔w�i
    public float scrollSpeed = 2f; // �X�N���[�����x
    private float backgroundHeight; // �w�i�摜�̍���

    void Start() {
        // �w�i�摜�̍������擾
        backgroundHeight = background1.bounds.size.y;
    }

    void Update() {
        // �w�i���X�N���[��
        ScrollBackground(background1);
        ScrollBackground(background2);

        // �w�i�����Z�b�g
        ResetPositionIfNeeded(background1, background2);
        ResetPositionIfNeeded(background2, background1);
    }

    void ScrollBackground(SpriteRenderer background) {
        // ���݂̈ʒu���擾
        Vector3 position = background.transform.position;

        // �������Ɉړ�
        position.y -= scrollSpeed * Time.deltaTime;

        // �ʒu���X�V
        background.transform.position = position;
    }

    void ResetPositionIfNeeded(SpriteRenderer currentBackground, SpriteRenderer otherBackground) {
        // ���݂̔w�i����ʊO�ɏo����A��������̔w�i�̏�Ɉړ�
        if (currentBackground.transform.position.y <= -backgroundHeight) {
            currentBackground.transform.position = new Vector3(
                currentBackground.transform.position.x,
                otherBackground.transform.position.y + backgroundHeight,
                currentBackground.transform.position.z
            );
        }
    }
}
