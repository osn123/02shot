using UnityEngine;

public class PlayerBullet : MonoBehaviour // PlayerBullet�N���X���`
{
    public float speed = 10f; // �e�̈ړ����x
    public float lifetime = 5f; // �e����ʊO�ŏ��ł���܂ł̎���

    void Start() // �X�^�[�g���\�b�h
    {
        // ��莞�Ԍ�ɒe�������I�ɔj��
        Destroy(gameObject,lifetime); // lifetime��ɒe��j��
    }

    void Update() // �X�V���\�b�h
    {
        // �e��������Ɉړ�
        transform.Translate(Vector3.up * speed * Time.deltaTime); // �e����Ɉړ�
    }

    private void OnTriggerEnter2D(Collider2D collision) // �Փˎ��̏���
    {
        // �G�ƏՓ˂����ꍇ
        if (collision.CompareTag("Enemy")) // �Փ˂����I�u�W�F�N�g���G���m�F
        {
            Destroy(gameObject); // �e��j��
            Destroy(collision.gameObject); // �G��j���i�K�v�ɉ����ĕύX�j
        }
    }
}
