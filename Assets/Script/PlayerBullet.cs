using UnityEngine;

// �v���C���[�̒e�𐧌䂷��N���X
public class PlayerBullet : MonoBehaviour {
    [Header("Bullet Settings")]
    public float speed = 10f; // �e�̈ړ����x
    public float lifetime = 5f; // �e�����ł���܂ł̎���

    void Start() {
        // ��莞�Ԍ�ɒe�������I�ɔj��
        Destroy(gameObject,lifetime);
    }

    void Update() {
        // �e��������Ɉړ�
        MoveBullet();
    }

    private void MoveBullet() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // �G�ƏՓ˂����ꍇ�̏���
        if (collision.CompareTag("Enemy")) {
            Destroy(gameObject); // �e��j��
            Destroy(collision.gameObject); // �G��j��
        }
    }
}
