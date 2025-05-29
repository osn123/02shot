using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    // プレイヤーの基本設定
    public float moveSpeed = 5f;           // 移動速度
    public GameObject bulletPrefab;        // 弾のプレハブ
    public GameObject explosionPrefab;     // 爆発エフェクトのプレハブ

    // サウンド
    public AudioClip playerShootClip;      // 弾発射音
    public AudioClip playerHitClip;        // 被弾音
    public AudioClip scoreClip;        // 被弾音
    private AudioSource audioSource;       // オーディオ再生用

    // 画面制御
    private Vector2 screenBounds;          // 画面端のワールド座標

    // 状態管理
    public static bool isDead = false;     // プレイヤー死亡フラグ
    //private bool isResultScreen = false;   // リザルト画面表示中フラグ

    // リザルト画面UI
    public GameObject resultPanel;         // リザルトパネル
    public Text resultScoreText;           // スコア表示テキスト
    public GameObject pressStartText;      // "PRESS START KEY" テキスト

    // 明滅制御
    private bool isPressStartVisible = true;
    private float blinkInterval = 0.5f;
    private float nextBlinkTime;

    void Start() {
        // 画面の境界を取得
        Camera mainCamera = Camera.main; // メインカメラを取得
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,mainCamera.transform.position.z)); // 画面の端のワールド座標を計算
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得

        // プレイヤーを画面下部中央に配置
        //transform.position = new Vector3(0, -screenBounds.y + 1, 0); // コメントアウトされているが、プレイヤーの初期位置を設定するコード
        resultPanel.SetActive(false);
        pressStartText.SetActive(false);

        GameManager.Instance.scoreText21.enabled = false;
        GameManager.Instance.scoreText22.enabled = false;
    }

    void Update() {
        if (GameManager.Instance.isResultScreen) {
            HandleBlinking(); // 明滅処理を実行
            HandleRestart();  // リスタート処理を実行
            return;
        }

        if (isDead) {
            return;
        }

        HandleMovement(); // プレイヤーの移動処理を呼び出す
        HandleShooting(); // プレイヤーの弾発射処理を呼び出す
    }

    void HandleMovement() {
        // 入力取得
        float moveX = Input.GetAxis("Horizontal"); // 水平方向の入力を取得
        float moveY = Input.GetAxis("Vertical"); // 垂直方向の入力を取得

        // 移動計算
        Vector3 move = new Vector3(moveX, moveY, 0).normalized * moveSpeed * Time.deltaTime; // 入力に基づいて移動量を計算

        // 新しい位置を計算
        Vector3 newPosition = transform.position + move; // 現在の位置に移動量を加算

        // 画面外に出ないように制限（UI部分を考慮）
        newPosition.x = Mathf.Clamp(newPosition.x,-screenBounds.x + 0.8f,screenBounds.x - 0.8f - 4.3f); // TODO:X座標を画面内に制限
        newPosition.y = Mathf.Clamp(newPosition.y,-screenBounds.y + 0.8f,screenBounds.y - 0.8f); // Y座標を画面内に制限

        // プレイヤーの位置を更新
        transform.position = newPosition; // 計算した位置をプレイヤーに適用
    }

    void HandleShooting() {
        // Zキーで弾を発射
        if (Input.GetKeyDown(KeyCode.Z)) // Zキーが押された場合
        {
            Instantiate(bulletPrefab,transform.position,Quaternion.identity); // 弾のプレハブをプレイヤーの位置に生成
            audioSource.PlayOneShot(playerShootClip); // プレイヤーの弾撃ちSEを再生
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // 当たり判定で死亡処理
        if (!isDead && collision.CompareTag("Enemy")) // プレイヤーが死亡しておらず、敵と衝突した場合
        {
            audioSource.PlayOneShot(playerHitClip); // プレイヤー撃破SEを再生
            HandleDeath(); // 死亡処理
        }
    }

    private void HandleDeath() {
        // 爆発エフェクトを生成
        GameObject effect = Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(effect, 1f);

        isDead = true; // 死亡フラグを立てる
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // スプライトレンダラーを非表示にする

        StartCoroutine(GameManager.Instance.OnGameOver());
    }

    private void ShowResultScreen() {
        resultPanel.SetActive(true); // リザルト画面を表示
        int finalScore = GameManager.Instance.GetScore(); // GameManagerからスコアを取得
        resultScoreText.text = "あなたの獲得スコアは: " + finalScore + "点"; // スコアを表示
    }

    public void HandleBlinking() {
        if (Time.time >= nextBlinkTime) {
            isPressStartVisible = !isPressStartVisible;
            pressStartText.SetActive(isPressStartVisible);
            nextBlinkTime = Time.time + blinkInterval;
        }
    }

    public void HandleRestart() {
        if (Input.GetKeyDown(KeyCode.S)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}