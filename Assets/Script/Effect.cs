using UnityEngine;

public class Effect : MonoBehaviour
{
    int lifetime = 1; // �e�̃��C�t�^�C���i�b�j
    void Start() // �X�^�[�g���\�b�h
    {
        // ��莞�Ԍ�ɒe�������I�ɔj��
        Destroy(gameObject,lifetime); // lifetime��ɒe��j��
    }
}
