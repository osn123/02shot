using UnityEngine;

// プレイヤーの弾を制御するクラス
public class PlayerBullet : MonoBehaviour {
    [Header("Bullet Settings")]
    public float speed = 10f; // 弾の移動速度
    public float lifetime = 5f; // 弾が消滅するまでの時間

    void Start() {
        // 一定時間後に弾を自動的に破棄
        Destroy(gameObject,lifetime);
    }

    void Update() {
        // 弾を上方向に移動
        MoveBullet();
    }

    private void MoveBullet() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // 敵と衝突した場合の処理
        if (collision.CompareTag("Enemy")) {
            Destroy(gameObject); // 弾を破棄
            Destroy(collision.gameObject); // 敵を破棄
        }
    }
}
