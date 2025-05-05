using UnityEngine;
using UnityEngine.Audio;

// 敵キャラクターを管理するクラス
public class Enemy : MonoBehaviour {

    // 敵キャラクターの設定
    public GameObject explosionEffect;    // 爆発エフェクトのプレハブ
    public AudioClip enemyShootClip;      // 敵の弾撃ちSE

    private float speed = 5f;             // 敵の移動速度

    void Start() {
    }

    void Update() {
        // 敵を下に移動
        transform.Translate(Vector3.down * speed * Time.deltaTime); // 敵を下方向に移動
        // 画面外に出たら消滅
        if (transform.position.y < -6f) { // Y座標が-6未満の場合
            Destroy(gameObject); // 敵オブジェクトを破棄
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // プレイヤーの弾に当たった場合
        if (other.CompareTag("PlayerBullet")) { // 衝突したオブジェクトがプレイヤーの弾か確認
                                                // 爆発エフェクトを生成
            Instantiate(explosionEffect,transform.position,Quaternion.identity);
            GameManager.Instance.PlayEnemyHitSound(); // GameManagerで敵撃破SEを再生
                                                      // スコアを+1する
            GameManager.Instance.AddScore(1); // GameManagerのインスタンスを通じてスコアを加算

            // 敵を消滅
            Destroy(gameObject); // 敵オブジェクトを破棄
        }
    }
}
