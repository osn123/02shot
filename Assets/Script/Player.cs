using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public GameObject bulletPrefab; // 弾のプレハブを格納する変数
    public GameObject explosionPrefab; // 爆発エフェクトのプレハブを格納する変数
    private Vector2 screenBounds; // 画面の境界を格納する変数

    public AudioClip playerShootClip; // プレイヤーの弾撃ちSE
    public AudioClip playerHitClip; // プレイヤー撃破SE


    private bool isDead = false; // プレイヤーが死亡しているかどうかを判定するフラグ
    private AudioSource audioSource; // AudioSourceコンポーネント


    void Start() {
        // 画面の境界を取得
        Camera mainCamera = Camera.main; // メインカメラを取得
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,mainCamera.transform.position.z)); // 画面の端のワールド座標を計算
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得

        // プレイヤーを画面下部中央に配置
        //transform.position = new Vector3(0, -screenBounds.y + 1, 0); // コメントアウトされているが、プレイヤーの初期位置を設定するコード
    }

    void Update() {
        if (isDead)
            return; // プレイヤーが死亡している場合、以降の処理をスキップ

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
            isDead = true; // 死亡フラグを立てる
            StartCoroutine(HandleDeath()); // 死亡処理をコルーチンで実行
        }
    }

    private IEnumerator HandleDeath() {
        // 爆発エフェクトを生成
        // TODO:爆発エフェクトをプレイヤーの位置に生成
        // Instantiate(explosionPrefab,transform.position,Quaternion.identity); 

        // プレイヤーを非表示にする
        //gameObject.SetActive(false); // プレイヤーを非アクティブ化
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // スプライトレンダラーを非表示にする
        // 爆発エフェクト終了後30フレーム待機
        yield return new WaitForSeconds(30f / 60f); // 30フレーム分の時間を待機

        // リザルト画面に移行
        SceneManager.LoadScene("ResultScene"); // ゲームシーンに遷移
    }
}
