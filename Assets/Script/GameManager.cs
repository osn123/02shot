using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ゲーム全体の管理を行うクラス
public class GameManager : MonoBehaviour
{

    // 背景スクロール関連
    public GameObject background1; // 1枚目の背景画像
    public GameObject background2; // 2枚目の背景画像
    [SerializeField] private float scrollSpeed = 2f; // 背景スクロール速度
    private float backgroundHeight; // 背景画像の高さ

    // スコア表示関連
    public Image scoreImage; // スコア画像UI
    public Text scoreText;   // スコアテキストUI
    public Text scoreText21;   // スコアテキストUI
    public Text scoreText22;   // スコアテキストUI
    private int score = 0;   // 現在のスコア

    public TextMeshProUGUI targetText; // インスペクターに表示される
    public TextMeshProUGUI targetText10; // インスペクターに表示される

    // サウンド関連
    public AudioClip bgmClip;        // BGMクリップ
    public AudioSource audioSource;  // AudioSource
    public AudioClip enemyHitClip;   // 敵撃破SE
    public AudioClip scoreClip;        // 

    // シングルトン
    public static GameManager Instance; // シングルトンインスタンス

    public GameObject resultPanel;         // リザルトパネル
    public bool isResultScreen = false;   // リザルト画面表示中フラグ

    void Awake()
    {
        // シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 複数のインスタンスが存在しないようにする
        }
    }

    void Start()
    {
        // 背景画像の高さを取得
        backgroundHeight = background1.GetComponentInChildren<SpriteRenderer>().bounds.size.y * 4; // TODO: 1枚目の背景の高さを取得し、4倍する
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
        PlayBGM(); // BGMを再生
        UpdateScoreText();
    }

    [SerializeField] private GameObject enemyPrefab; // 敵のプレハブ
    [SerializeField] private float spawnInterval = 2f; // 敵を生成する間隔
    private float nextSpawnTime; // 次に敵を生成する時間

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();  // ESCキーでアプリ終了
        }
        // 毎フレーム、背景画像をスクロールさせる
        ScrollBackground(background1); // 1枚目の背景をスクロール
        ScrollBackground(background2); // 2枚目の背景をスクロール

        // 背景画像が画面外に出た場合、位置をリセットする
        ResetPositionIfNeeded(background1, background2); // 1枚目の背景をリセット
        ResetPositionIfNeeded(background2, background1); // 2枚目の背景をリセット

        // 敵を一定時間ごとに生成
        if (Time.time >= nextSpawnTime && !Player.isDead)
        {
            SpawnEnemy();
            spawnInterval = Random.Range(0.5f, 2f);
            nextSpawnTime = Time.time + spawnInterval; // 次の生成時間を設定
        }
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(-8f, 8f - 4f); // 画面の幅に応じて調整
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0f); // Y座標は画面外上部に設定
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // 敵を生成
    }

    void ScrollBackground(GameObject background)
    {
        // 現在の位置を取得
        Vector3 position = background.transform.position;

        // 下方向に移動（スクロール速度に応じて移動量を計算）
        position.y -= scrollSpeed * Time.deltaTime; // スクロール速度に基づいてY座標を減少させる

        // 計算した位置を背景オブジェクトに適用
        background.transform.position = position; // 新しい位置を背景オブジェクトに設定
    }

    void ResetPositionIfNeeded(GameObject currentBackground, GameObject otherBackground)
    {
        // 現在の背景が画面外（-backgroundHeight以下）に出た場合
        if (currentBackground.transform.position.y <= -backgroundHeight)
        {
            // もう一方の背景の上に現在の背景を移動させる
            currentBackground.transform.position = new Vector3(
                currentBackground.transform.position.x, // X座標はそのまま
                otherBackground.transform.position.y + backgroundHeight, // Y座標をもう一方の背景の上に設定
                currentBackground.transform.position.z // Z座標はそのまま
            );
        }
    }

    void PlayBGM()
    {
        audioSource.clip = bgmClip; // BGMクリップを設定
        audioSource.Play(); // BGMを再生
    }

    public void PlayEnemyHitSound()
    { // 敵撃破SEを再生するメソッド
        audioSource.PlayOneShot(enemyHitClip); // 敵撃破SEを再生
    }

    public void AddScore(int points)
    { // スコアを加算するメソッド
        score += points; // スコアを加算
        UpdateScoreText(); // スコア表示を更新
    }

    public int GetScore()
    {
        return score; // スコアを取得するメソッド
    }

    private void UpdateScoreText()//todo: 5keta hyouzi
    { // スコア表示を更新するメソッド
        scoreText.text = score.ToString();
        targetText.text = "<sprite=" + score % 10 + ">";
        if (score < 10)
        {
            targetText10.text = "";
        }
        else
        {
            targetText10.text = "<sprite=" + score / 10 + ">";
        }
    }

    public IEnumerator OnGameOver()
    {
        // 爆発エフェクト終了後30フレーム待機
        yield return new WaitForSeconds(30f / 60f); // 30フレーム分の時間を待機

        resultPanel.SetActive(true); // リザルト画面を表示
        scoreText21.text = "あなたの獲得スコアは: "; // スコアをテキストに反映
        scoreText21.enabled = true;
        audioSource.PlayOneShot(scoreClip); // SEを再生

        yield return new WaitForSeconds(1f); // 1秒待機
        scoreText22.text = GetScore() + "点"; // スコアをテキストに反映
        scoreText22.enabled = true;
        audioSource.PlayOneShot(scoreClip); // SEを再生

        audioSource.enabled = false; //// TODO: AudioSourceを無効化
        isResultScreen = true; // 
    }
}