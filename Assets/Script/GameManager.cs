using UnityEngine;
using UnityEngine.UI;

// �Q�[���S�̂̊Ǘ����s���N���X
public class GameManager : MonoBehaviour {
    public GameObject background1; // 1���ڂ̔w�i�摜�I�u�W�F�N�g
    public GameObject background2; // 2���ڂ̔w�i�摜�I�u�W�F�N�g
    [SerializeField] private float scrollSpeed = 2f; // �w�i�摜�̃X�N���[�����x�iInspector�Œ����\�j
    private float backgroundHeight; // �w�i�摜�̍������i�[����ϐ�

    public Image scoreImage; // �X�R�A��\������摜
    public Text scoreText; // �X�R�A��\������e�L�X�g
    private int score = 0; // ���݂̃X�R�A


    public AudioClip bgmClip; // BGM�̃N���b�v
    private AudioSource audioSource; // AudioSource�R���|�[�l���g
    public AudioClip enemyHitClip; // �G���jSE

    public static GameManager Instance; // �V���O���g���C���X�^���X

    void Awake() {
        // �V���O���g���̏�����
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); // �����̃C���X�^���X�����݂��Ȃ��悤�ɂ���
        }
    }

    void Start() {
        // �w�i�摜�̍������擾
        backgroundHeight = background1.GetComponentInChildren<SpriteRenderer>().bounds.size.y * 4; // TODO: 1���ڂ̔w�i�̍������擾���A4�{����
        audioSource = GetComponent<AudioSource>(); // AudioSource�R���|�[�l���g���擾
        PlayBGM(); // BGM���Đ�
        PlayerPrefs.SetInt("FinalScore",0); // �X�R�A��ۑ�

    }

    [SerializeField] private GameObject enemyPrefab; // �G�̃v���n�u
    [SerializeField] private float spawnInterval = 2f; // �G�𐶐�����Ԋu
    private float nextSpawnTime; // ���ɓG�𐶐����鎞��

    void Update() {
        // ���t���[���A�w�i�摜���X�N���[��������
        ScrollBackground(background1); // 1���ڂ̔w�i���X�N���[��
        ScrollBackground(background2); // 2���ڂ̔w�i���X�N���[��

        // �w�i�摜����ʊO�ɏo���ꍇ�A�ʒu�����Z�b�g����
        ResetPositionIfNeeded(background1,background2); // 1���ڂ̔w�i�����Z�b�g
        ResetPositionIfNeeded(background2,background1); // 2���ڂ̔w�i�����Z�b�g

        // �G����莞�Ԃ��Ƃɐ���
        if (Time.time >= nextSpawnTime) {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval; // ���̐������Ԃ�ݒ�
        }
    }

    void SpawnEnemy() {
        float randomX = Random.Range(-8f, 8f - 4f); // ��ʂ̕��ɉ����Ē���
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0f); // Y���W�͉�ʊO�㕔�ɐݒ�
        Instantiate(enemyPrefab,spawnPosition,Quaternion.identity); // �G�𐶐�
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

    void PlayBGM() {
        audioSource.clip = bgmClip; // BGM�N���b�v��ݒ�
        audioSource.Play(); // BGM���Đ�
    }

    public void PlayEnemyHitSound() { // �G���jSE���Đ����郁�\�b�h
        audioSource.PlayOneShot(enemyHitClip); // �G���jSE���Đ�
    }

    public void AddScore(int points) { // �X�R�A�����Z���郁�\�b�h
        score += points; // �X�R�A�����Z
        UpdateScoreText(); // �X�R�A�\�����X�V
        PlayerPrefs.SetInt("FinalScore",score); // �X�R�A��ۑ�
    }

    private void UpdateScoreText() { // �X�R�A�\�����X�V���郁�\�b�h
        scoreText.text = "Score: " + score; // �X�R�A���e�L�X�g�ɔ��f
    }
}