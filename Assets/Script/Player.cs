using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public GameObject bulletPrefab; // �e�̃v���n�u���i�[����ϐ�
    public GameObject explosionPrefab; // �����G�t�F�N�g�̃v���n�u���i�[����ϐ�
    private Vector2 screenBounds; // ��ʂ̋��E���i�[����ϐ�

    public AudioClip playerShootClip; // �v���C���[�̒e����SE
    public AudioClip playerHitClip; // �v���C���[���jSE


    private bool isDead = false; // �v���C���[�����S���Ă��邩�ǂ����𔻒肷��t���O
    private AudioSource audioSource; // AudioSource�R���|�[�l���g


    void Start() {
        // ��ʂ̋��E���擾
        Camera mainCamera = Camera.main; // ���C���J�������擾
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,mainCamera.transform.position.z)); // ��ʂ̒[�̃��[���h���W���v�Z
        audioSource = GetComponent<AudioSource>(); // AudioSource�R���|�[�l���g���擾

        // �v���C���[����ʉ��������ɔz�u
        //transform.position = new Vector3(0, -screenBounds.y + 1, 0); // �R�����g�A�E�g����Ă��邪�A�v���C���[�̏����ʒu��ݒ肷��R�[�h
    }

    void Update() {
        if (isDead)
            return; // �v���C���[�����S���Ă���ꍇ�A�ȍ~�̏������X�L�b�v

        HandleMovement(); // �v���C���[�̈ړ��������Ăяo��
        HandleShooting(); // �v���C���[�̒e���ˏ������Ăяo��
    }

    void HandleMovement() {
        // ���͎擾
        float moveX = Input.GetAxis("Horizontal"); // ���������̓��͂��擾
        float moveY = Input.GetAxis("Vertical"); // ���������̓��͂��擾

        // �ړ��v�Z
        Vector3 move = new Vector3(moveX, moveY, 0).normalized * moveSpeed * Time.deltaTime; // ���͂Ɋ�Â��Ĉړ��ʂ��v�Z

        // �V�����ʒu���v�Z
        Vector3 newPosition = transform.position + move; // ���݂̈ʒu�Ɉړ��ʂ����Z

        // ��ʊO�ɏo�Ȃ��悤�ɐ����iUI�������l���j
        newPosition.x = Mathf.Clamp(newPosition.x,-screenBounds.x + 0.8f,screenBounds.x - 0.8f - 4.3f); // TODO:X���W����ʓ��ɐ���
        newPosition.y = Mathf.Clamp(newPosition.y,-screenBounds.y + 0.8f,screenBounds.y - 0.8f); // Y���W����ʓ��ɐ���

        // �v���C���[�̈ʒu���X�V
        transform.position = newPosition; // �v�Z�����ʒu���v���C���[�ɓK�p
    }

    void HandleShooting() {
        // Z�L�[�Œe�𔭎�
        if (Input.GetKeyDown(KeyCode.Z)) // Z�L�[�������ꂽ�ꍇ
        {
            Instantiate(bulletPrefab,transform.position,Quaternion.identity); // �e�̃v���n�u���v���C���[�̈ʒu�ɐ���
            audioSource.PlayOneShot(playerShootClip); // �v���C���[�̒e����SE���Đ�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // �����蔻��Ŏ��S����
        if (!isDead && collision.CompareTag("Enemy")) // �v���C���[�����S���Ă��炸�A�G�ƏՓ˂����ꍇ
        {

            audioSource.PlayOneShot(playerHitClip); // �v���C���[���jSE���Đ�
            isDead = true; // ���S�t���O�𗧂Ă�
            StartCoroutine(HandleDeath()); // ���S�������R���[�`���Ŏ��s
        }
    }

    private IEnumerator HandleDeath() {
        // �����G�t�F�N�g�𐶐�
        // TODO:�����G�t�F�N�g���v���C���[�̈ʒu�ɐ���
        // Instantiate(explosionPrefab,transform.position,Quaternion.identity); 

        // �v���C���[���\���ɂ���
        //gameObject.SetActive(false); // �v���C���[���A�N�e�B�u��
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // �X�v���C�g�����_���[���\���ɂ���
        // �����G�t�F�N�g�I����30�t���[���ҋ@
        yield return new WaitForSeconds(30f / 60f); // 30�t���[�����̎��Ԃ�ҋ@

        // ���U���g��ʂɈڍs
        SceneManager.LoadScene("ResultScene"); // �Q�[���V�[���ɑJ��
    }
}
