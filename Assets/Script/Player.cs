using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    // �v���C���[�̊�{�ݒ�
    public float moveSpeed = 5f;           // �ړ����x
    public GameObject bulletPrefab;        // �e�̃v���n�u
    public GameObject explosionPrefab;     // �����G�t�F�N�g�̃v���n�u

    // �T�E���h
    public AudioClip playerShootClip;      // �e���ˉ�
    public AudioClip playerHitClip;        // ��e��
    private AudioSource audioSource;       // �I�[�f�B�I�Đ��p

    // ��ʐ���
    private Vector2 screenBounds;          // ��ʒ[�̃��[���h���W

    // ��ԊǗ�
    public static bool isDead = false;     // �v���C���[���S�t���O
    private bool isResultScreen = false;   // ���U���g��ʕ\�����t���O

    // ���U���g���UI
    public GameObject resultPanel;         // ���U���g�p�l��
    public Text resultScoreText;           // �X�R�A�\���e�L�X�g
    public GameObject pressStartText;      // "PRESS START KEY" �e�L�X�g

    // ���Ő���
    private bool isPressStartVisible = true;
    private float blinkInterval = 0.5f;
    private float nextBlinkTime;

    void Start() {
        // ��ʂ̋��E���擾
        Camera mainCamera = Camera.main; // ���C���J�������擾
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,mainCamera.transform.position.z)); // ��ʂ̒[�̃��[���h���W���v�Z
        audioSource = GetComponent<AudioSource>(); // AudioSource�R���|�[�l���g���擾

        // �v���C���[����ʉ��������ɔz�u
        //transform.position = new Vector3(0, -screenBounds.y + 1, 0); // �R�����g�A�E�g����Ă��邪�A�v���C���[�̏����ʒu��ݒ肷��R�[�h
        resultPanel.SetActive(false);
        pressStartText.SetActive(false);
    }

    void Update() {
        if (isResultScreen) {
            HandleBlinking(); // ���ŏ��������s
            HandleRestart();  // ���X�^�[�g���������s
            return;
        }

        if (isDead) {
            return;
        }


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
            StartCoroutine(HandleDeath()); // ���S�������R���[�`���Ŏ��s
        }
    }

    private IEnumerator HandleDeath() {
        // �����G�t�F�N�g�𐶐�
        Instantiate(explosionPrefab,transform.position,Quaternion.identity);

        isDead = true; // ���S�t���O�𗧂Ă�
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // �X�v���C�g�����_���[���\���ɂ���
        // �����G�t�F�N�g�I����30�t���[���ҋ@
        yield return new WaitForSeconds(30f / 60f); // 30�t���[�����̎��Ԃ�ҋ@
        GameManager.Instance.audioSource.enabled = false; //// TODO: AudioSource�𖳌���
        yield return new WaitForSeconds(1f); // 1�b�ҋ@
        ShowResultScreen();
        yield return new WaitForSeconds(1f); // 1�b�ҋ@
        isResultScreen = true; // 

    }

    private void ShowResultScreen() {
        resultPanel.SetActive(true); // ���U���g��ʂ�\��
        int finalScore = GameManager.Instance.GetScore(); // GameManager����X�R�A���擾
        resultScoreText.text = "���Ȃ��̊l���X�R�A��: " + finalScore + "�_"; // �X�R�A��\��
    }

    public void HandleBlinking() {
        if (Time.time >= nextBlinkTime) {
            isPressStartVisible = !isPressStartVisible;
            pressStartText.SetActive(isPressStartVisible);
            nextBlinkTime = Time.time + blinkInterval;
        }
    }

    public void HandleRestart() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}